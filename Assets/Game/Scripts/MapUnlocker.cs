using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MapUnlocker : MonoBehaviour
{
    [SerializeField] private int _itemSaveID;

    [Header("Segments Animation")] [SerializeField]
    private Vector3 _startScale;

    [SerializeField] private Vector3 _endScale;
    [SerializeField] private int _scaleSpeed;
    [SerializeField] private UnityEvent _onUnlock;
    [SerializeField] private UnityEvent _onCountUpdate;
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private MapUnlocker _segment;
    [SerializeField] private ResourceKit _resourceKit;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private GameObject[] _disapearObjects;
    [SerializeField] private NavMeshObstacle _obstacle;
    [SerializeField] private GameObject _items;
    private MeshRenderer _meshRenderer;
    private int _currentResourceCount;
    private bool _isUnlock;

    private Coroutine _coroutine;
    //private bool _isUnlock;

    public void TryGetResource()
    {
        if (_coroutine != null) return;
        _coroutine = StartCoroutine(ReduceResource());
    }

    public void StopGetting()
    {
        if (_coroutine == null) return;
        StopCoroutine(_coroutine);
        _coroutine = null;
        //StopAllCoroutines();
    }

    private void Start()
    {
        UpdateUI();
        _meshRenderer = GetComponent<MeshRenderer>();
        TryLoadSave();
    }

    private void TryLoadSave()
    {
        var state = ItemsSaver.Instance.GetItemState(_itemSaveID);
        if (state != ItemState.Enable && state != ItemState.EnableWithInclude) return;
        transform.localScale = _endScale;
        _items.SetActive(true);
        _obstacle.enabled = false;
        _meshRenderer.enabled = true;
        foreach (var disapearObject in _disapearObjects) disapearObject.SetActive(state == ItemState.Enable);
    }

    private void UnlockNextSegment()
    {
        ItemsSaver.Instance.AddItem(_itemSaveID, ItemState.EnableWithInclude);
        _segment.UnlockSelf();
        foreach (var disapearObject in _disapearObjects) disapearObject.SetActive(false);
    }

    public void UnlockSelf()
    {
        ItemsSaver.Instance.AddItem(_itemSaveID, ItemState.Enable);
        if (_isUnlock) return;
        transform.localScale = _startScale;
        _isUnlock = true;
        _items.SetActive(true);
        _obstacle.enabled = false;
        _meshRenderer.enabled = true;
        foreach (var disapearObject in _disapearObjects) disapearObject.SetActive(true);
        transform.DOScale(_endScale, 0.5f).onComplete = () => _onUnlock?.Invoke();
    }

    private void UpdateUI()
    {
        if (_text == null) return;
        _text.text = _currentResourceCount + "/" + _resourceKit.Count;
        _onCountUpdate?.Invoke();
    }

    private IEnumerator ReduceResource()
    {
        var count = _resourceKit.Count - _currentResourceCount;
        for (int i = 0; i < count; i++)
        {
            if (ResourceCollector.Instance.IsHaveResource(_resourceKit.ResourceType) == false) yield break;
            ResourceCollector.Instance.ReduceResource(_resourceKit.ResourceType);
            _spawner.SpawnFromPlayer(_resourceKit.ResourceType, () =>
            {
                _currentResourceCount++;
                UpdateUI();
                //ResourceCollector.Instance.UpdateAllUI();
                if (_currentResourceCount >= _resourceKit.Count)
                {
                    UnlockNextSegment();
                    _coroutine = null;
                }
            });
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}