using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class ResourcesPrefabs : Singleton<ResourcesPrefabs>
{
    [SerializeField] private ResourceSets[] _resources;
    private ManagerPool _managerPool;

    private void Awake()
    {
        _managerPool = ManagerPool.Instance;
        foreach (var resource in _resources)
            _managerPool.AddPool(PoolType.Entities).PopulateWith(resource.Prefab, resource.Count);
    }

    public Transform SpawnResource(ResourceType type, Vector3 position)
    {
        var resource = _resources.FirstOrDefault(item => item.ResourceType == type);
        return _managerPool.Spawn<Transform>(PoolType.Entities, resource.Prefab, position);
    }
}

[Serializable]
public struct ResourceSets
{
    public ResourceType ResourceType;
    public GameObject Prefab;
    public int Count;
}

public enum ResourceType
{
    Stone,
    Wood,
    Gear
}