using UnityEngine;

public class ObstaclesChecker : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 300;
    [SerializeField] private LayerMask _obstaceLayers;
    private Obstacle _obstacle;
    private RaycastHit _hit;
    private Transform _transform;
    private void Start() => _transform = transform;

    private void Update() => CheckObstacles();

    private void CheckObstacles()
    {
        if (Physics.Raycast(_transform.position, _transform.forward, out var hit, _rayDistance,
            _obstaceLayers) == false)
        {
            if (_obstacle != null) _obstacle.SetObstacle(true);
            _obstacle = null;
            return;
        }
        if (hit.collider.TryGetComponent<Obstacle>(out var component) == false || _obstacle == component) return;
        if (_obstacle != null) _obstacle.SetObstacle(true);
        _obstacle = component;
        _obstacle.SetObstacle(false);
    }
}