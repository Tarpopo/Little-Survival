using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResourcePointCollector : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCountUpdate;
    [SerializeField] private UnityEvent _onAllCollect;
    [SerializeField] private ResourceCollectorData[] _resources;
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private GameObject[] _disapearObjects;
    [SerializeField] private float _spawnDelay;
    [SerializeField] protected bool _saveItem;
    [SerializeField] private bool _moto;
    protected bool IsAllCollected => _resources.All(item => item.IsFull);
    private Coroutine _coroutine;

    public event UnityAction OnAllCollect
    {
        add => _onAllCollect.AddListener(value);
        remove => _onAllCollect.RemoveListener(value);
    }

    public event UnityAction OnCountUpdate
    {
        add => _onCountUpdate.AddListener(value);
        remove => _onCountUpdate.RemoveListener(value);
    }

    public void TryGetResource()
    {
        if (_coroutine != null) return;
        _coroutine = StartCoroutine(ReduceResource());
    }

    public void SetDisapearObjects(bool active)
    {
        if (active && IsAllCollected) return;
        foreach (var dispearObject in _disapearObjects) dispearObject.SetActive(active);
    }

    public void StopGetting()
    {
        StopAllCoroutines();
        _coroutine = null;
    }

    protected virtual void Start()
    {
        if (_saveItem)
        {
            if (_moto)
            {
                _resources[0].SetResource(ResourceType.Gear, ItemsSaver.Instance.GetMoto());
                Debug.Log("GetMoto" + ItemsSaver.Instance.GetMoto());
            }
            else
            {
                foreach (var resource in _resources)
                    resource.SetResource(resource.ResourceType,
                        ItemsSaver.Instance.GetTowerResources(resource.ResourceType));
            }
        }

        foreach (var resource in _resources) resource.UpdateUI();
    }

    protected void ResetAllResources()
    {
        foreach (var resource in _resources) resource.ResetResource();
    }

    protected void OnAllCollected()
    {
        foreach (var disapearObject in _disapearObjects) disapearObject.SetActive(false);
        _onAllCollect?.Invoke();
    }

    public int GetAllCurrentResource() => _resources.Sum(resource => resource.CurrentResource);
    public int GetAllResourceCount() => _resources.Sum(resource => resource.ResourceCount);

    private IEnumerator ReduceResource()
    {
        foreach (var resource in _resources)
        {
            if (ResourceCollector.Instance.IsHaveResource(resource.ResourceType) == false) continue;
            var count = ResourceCollector.Instance.GetResourceCount(resource.ResourceType) - resource.CountToFull > 0
                ? resource.CountToFull
                : ResourceCollector.Instance.GetResourceCount(resource.ResourceType);
            //print(ResourceCollector.Instance.GetResourceCount(resource.ResourceType));

            for (int i = 0; i < count; i++)
            {
                ResourceCollector.Instance.ReduceResource(resource.ResourceType);
                //ResourceCollector.Instance.UpdateAllUI();
                _spawner.SpawnFromPlayer(resource.ResourceType, () =>
                {
                    resource.AddResource(_saveItem, _moto);
                    resource.UpdateUI();
                    _onCountUpdate?.Invoke();
                    if (_resources.All(element => element.IsFull) == false) return;
                    OnAllCollected();
                });
                yield return new WaitForSeconds(_spawnDelay);
            }

            yield return new WaitForSeconds(_spawnDelay);
        }

        _coroutine = null;
    }
}

[Serializable]
public class ResourceCollectorData
{
    [SerializeField] private ResourceKit _resourceKit;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _currentResourceCount;
    [SerializeField] private PunchScaleAnimation _punchScaleAnimation;
    public ResourceType ResourceType => _resourceKit.ResourceType;
    public int CurrentResource => _currentResourceCount;
    public int ResourceCount => _resourceKit.Count;
    public bool IsFull => _currentResourceCount == _resourceKit.Count;
    public int CountToFull => _resourceKit.Count - _currentResourceCount;

    public void SetResource(ResourceType type, int count)
    {
        if (type != _resourceKit.ResourceType) return;
        _currentResourceCount = count;
    }

    public void AddResource(bool save, bool moto)
    {
        if (IsFull) return;
        _currentResourceCount++;
        if (save)
        {
            if (moto)
            {
                Debug.Log("setMoto");
                ItemsSaver.Instance.SetMoto(_currentResourceCount);
            }
            else ItemsSaver.Instance.SetTowerResources(_resourceKit.ResourceType, _currentResourceCount);
        }
    }

    public void UpdateUI()
    {
        _text.text = _currentResourceCount + "/" + _resourceKit.Count;
        _punchScaleAnimation.PlayAnimation();
    }

    public void ResetResource()
    {
        _currentResourceCount = 0;
        UpdateUI();
    }
}