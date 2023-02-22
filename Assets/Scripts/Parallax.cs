using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    [SerializeField] private float _animationSpeed = 1f;

    private void Awake() => _meshRenderer = GetComponent<MeshRenderer>();

    private void Update() => _meshRenderer.material.mainTextureOffset += new Vector2(_animationSpeed * Time.deltaTime, 0);

}
