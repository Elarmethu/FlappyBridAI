using UnityEngine;

public class Player : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    private int _spriteIndex;

    [SerializeField] private float _strength = 5f;
    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _direction;

    // AI
    private NeuralNetwork _brain;
    private float _spawnTime;
    private bool _isAlive = true;
    [SerializeField] private float _fitness;

    public NeuralNetwork brain => _brain;
    public float fitness => _fitness;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnTime = Time.time;
    }

    public void Reset()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;

        _direction = Vector3.zero;
    }

    private void Update()
    {
        _direction.y += _gravity * Time.deltaTime;
        transform.position += _direction * Time.deltaTime;
        AnimateSprite();

        if (_isAlive)
        {
            _fitness += Time.deltaTime;
            UseNeuralNetwork();
        }
    }

    private void Jump() => _direction = Vector3.up * _strength;

    private void AnimateSprite()
    {
        _spriteIndex++;

        if (_spriteIndex >= _sprites.Length)
            _spriteIndex = 0;

        _spriteRenderer.sprite = _sprites[_spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Obstacle")
        {
            _isAlive = false;
            _spriteRenderer.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            NeuralManager.instance.BirdCrashed();
        }
    }

    public void SetBrain(NeuralNetwork brain, NeuralNetwork.NeuralNetworkType type)
    {
        _brain = brain;
        _brain.networkType = type;
    }

    private void UseNeuralNetwork()
    {
        float[] inputs = new float[4];

        if (Spawner.instance.currentPipe != null)
        {
            inputs[0] = Spawner.instance.currentPipe.transform.position.x;
            inputs[1] = Spawner.instance.currentPipe.GetComponent<Pipes>().highY;
            inputs[2] = Spawner.instance.currentPipe.GetComponent<Pipes>().lowY;
        }
        else
        {
            inputs[0] = 10.0f;
            inputs[1] = 1.5f;
            inputs[2] = -1.0f;
        }

        inputs[3] = transform.position.y;

        var output = _brain.FeedForward(inputs);

        if (output[0] > 0 && _brain.networkType != NeuralNetwork.NeuralNetworkType.Sigmoid)
            Jump();
        else if(output[0] > 0.5 && _brain.networkType == NeuralNetwork.NeuralNetworkType.Sigmoid)
            Jump();
    }
}
