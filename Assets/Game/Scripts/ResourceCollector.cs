using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class ResourceCollector : MySingleton<ResourceCollector>
{
    [SerializeField] private ResourceUI[] _resourceUI;
    [SerializeField] private Transform _collectPoint;
    [SerializeField] private LevelManager _levelManager;
    private readonly Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();
    public Vector3 CollectPosition => _collectPoint.position;
    public bool IsHaveAnyResource => _resources.Any(item => item.Value > 0);
    public void SetCollectPoint(Transform point) => _collectPoint = point;

    public bool IsHaveResource(ResourceType resourceType) =>
        _resources.ContainsKey(resourceType) && _resources[resourceType] > 0;

    public int GetResourceCount(ResourceType type) => _resources[type];

    private void Start()
    {
        //_resources.Add(ResourceType.Stone, Statistics.Stone);
        //_resources.Add(ResourceType.Wood, Statistics.Wood);
        _levelManager.OnLevelLoaded += CheckUI;
        AddResource(ResourceType.Stone, ResourceSaver.GetResource(ResourceType.Stone));
        AddResource(ResourceType.Wood, ResourceSaver.GetResource(ResourceType.Wood));
        AddResource(ResourceType.Gear, ResourceSaver.GetResource(ResourceType.Gear));
        DisableIfEmpty(ResourceType.Stone);
        DisableIfEmpty(ResourceType.Wood);
        DisableIfEmpty(ResourceType.Gear);
        if (IsHaveAnyResource == false) return;
        UpdateAllUI();
    }

    private void CheckUI() => SetAllUI(Statistics.CurrentLevelNumber == 0);

    // private void OnDisable() =>
    //     _resources.ForEach(resource => ResourceSaver.SaveResource(resource.Key, resource.Value));

    // private void OnDestroy() =>
    //     _resources.ForEach(resource => ResourceSaver.SaveResource(resource.Key, resource.Value));

    public void AddResource(ResourceType type, int count = 1)
    {
        if (_resources.ContainsKey(type))
        {
            _resources[type] += count;
            _resourceUI[(int) type].UpdateText(_resources[type]);
            ResourceSaver.SaveResource(type, _resources[type]);
            return;
        }

        _resources.Add(type, count);
        _resourceUI[(int) type].Enable(true);
        _resourceUI[(int) type].UpdateText(_resources[type]);
        ResourceSaver.SaveResource(type, _resources[type]);
    }

    public bool HaveResources(IEnumerable<ResourceKit> resourceKits)
    {
        // return resourceKits.Any(resourceKit =>
        //     _resources.ContainsKey(resourceKit.ResourceType) &&
        //     _resources[resourceKit.ResourceType] - resourceKit.Count >= 0);
        foreach (var resourceKit in resourceKits)
        {
            if (_resources.ContainsKey(resourceKit.ResourceType) == false ||
                _resources[resourceKit.ResourceType] - resourceKit.Count < 0) return false;
        }

        return true;
    }

    // public void ReduceResources(IEnumerable<ResourceKit> resourceKits)
    // {
    //     foreach (var resourceKit in resourceKits) _resources[resourceKit.ResourceType] -= resourceKit.Count;
    //     UpdateAllUI();
    // }

    public void ReduceResource(ResourceType resourceType)
    {
        _resources[resourceType]--;
        _resourceUI[(int) resourceType].UpdateText(_resources[resourceType]);
        ResourceSaver.SaveResource(resourceType, _resources[resourceType]);
        DisableIfEmpty(resourceType);
    }

    private void DisableIfEmpty(ResourceType resourceType)
    {
        if (_resources[resourceType] > 0) return;
        ResourceSaver.SaveResource(resourceType, _resources[resourceType]);
        _resourceUI[(int) resourceType].Enable(false);
        _resources.Remove(resourceType);
    }

    public void ClearAllResources()
    {
        foreach (var UI in _resourceUI)
        {
            UI.UpdateText(0);
            UI.Enable(false);
        }

        _resources.Clear();
    }

    public void UpdateAllUI() =>
        _resources.ForEach(resource => _resourceUI[(int) resource.Key].UpdateText(resource.Value));

    private void SetAllUI(bool active) =>
        _resources.ForEach(resource => _resourceUI[(int) resource.Key].Enable(active));
    // {
    //     foreach (var resource in _resources) _resourceUI[(int) resource.Key].UpdateText(resource.Value);
    // }
}

[Serializable]
public struct ResourceUI
{
    [SerializeField] private GameObject _resourceObject;
    [SerializeField] private TMP_Text _text;
    public void Enable(bool isActive) => _resourceObject.SetActive(isActive);
    public void UpdateText(int count) => _text.text = count.ToString();
}

[Serializable]
public struct ResourceKit
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _count;
    public ResourceType ResourceType => _resourceType;
    public int Count => _count;
}