using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance { get; private set; }

    [SerializeField] private GameObject _prefab;
    private float _spawnRate = 1f;
    private float _minHeight = -1f;
    private float _maxHeight = 2f;

    private List<GameObject> _pipes = new List<GameObject>();
    [SerializeField] private GameObject _currentPipe;
    public GameObject currentPipe => _currentPipe;

    private void Awake() => instance = this;

    private void Start()
    {
        //Spawn();
    }

    private void OnEnable() => InvokeRepeating(nameof(Spawn), _spawnRate, _spawnRate);

    private void OnDisable() => CancelInvoke(nameof(Spawn));

    private void Spawn()
    {
        GameObject pipe = Instantiate(_prefab, transform.position, Quaternion.identity);
        pipe.transform.position += Vector3.up * Random.Range(_minHeight, _maxHeight);

        if (_pipes.Count <= 0)
            _currentPipe = pipe;

        _pipes.Add(pipe);
    }

    public void DeletePipe()
    {
        _pipes.RemoveAt(0);
        if(_pipes.Count > 0)
            _currentPipe = _pipes[0];
    }

    public void ClearPipeList()
    {
        _currentPipe = null;
        _pipes.Clear();
    }
}
