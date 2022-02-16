using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ResourceSpawner
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _forceDirection;
    [SerializeField] private float _spawnForce;
    [SerializeField] private int _resourceCount;
    [SerializeField] private ResourceType _resourceType;

    public void SpawnResource()
    {
        for (int i = 0; i < _resourceCount; i++)
        {
            var resource = ResourcesPrefabs.Instance.SpawnResource(_resourceType, _spawnPoint.position)
                .GetComponent<Resource>();
            resource.SetEndPoint(_endPoint.position);
            resource.MoveResource(Quaternion.AngleAxis(Random.Range(10, 350), Vector3.up) *
                                  _forceDirection.localPosition.normalized *
                                  _spawnForce, false);
        }
    }

    public void SpawnFromPlayer(ResourceType type, Action onMoveEnd)
    {
        var resource = ResourcesPrefabs.Instance.SpawnResource(type, ResourceCollector.Instance.CollectPosition)
            .GetComponent<Resource>();
        resource.OnMoveEnd += () => onMoveEnd?.Invoke();
        resource.SetEndPoint(_endPoint.position);
        resource.MoveResource(
            Quaternion.AngleAxis(Random.Range(10, 350), Vector3.up) * _forceDirection.localPosition.normalized *
            _spawnForce, true);
    }
}