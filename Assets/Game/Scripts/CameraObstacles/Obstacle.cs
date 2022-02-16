using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Material _visible;
    [SerializeField] private Material _transparent;
    [SerializeField] private MeshRenderer[] _meshRenderers;
    private bool _isVisible = true;

    public void SetObstacle(bool isVisible)
    {
        if (_isVisible.Equals(isVisible)) return;
        _isVisible = isVisible;
        var material = isVisible ? _visible : _transparent;
        foreach (var meshRenderer in _meshRenderers) meshRenderer.material = material;
    }
}