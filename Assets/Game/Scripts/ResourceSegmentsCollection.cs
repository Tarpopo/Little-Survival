using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceSegmentsCollection : MonoBehaviour
{
    [SerializeField] private bool _save = true;
    [SerializeField] private int _itemSaveID;
    [SerializeField] private int _resourceCount;
    [SerializeField] private int _emptyResources;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private float _spawnForce;
    [SerializeField] private Health _health;
    [SerializeField] private ParticleSystem _resourcesClear;
    [SerializeField] private ParticleSystem _takeDamage;
    [SerializeField] private Transform _forceTransform;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<GameObject> _prefabs;
    [SerializeField] private NavMeshObstacle _obstacle;
    [SerializeField] private Collider _collider;
    [SerializeField] private PunchScaleAnimation _punchScaleAnimation;
    private GameObject _currentPrefab;
    private Vector3 _forceDirection => (_forceTransform.position - _spawnPoint.position).normalized;
    private int _disableCount;

    private void Start()
    {
        //SetItemSaveID();
        if (_save)
        {
            var itemState = ItemsSaver.Instance.GetItemState(_itemSaveID);
            if (itemState == ItemState.Disable)
            {
                _prefabs[0].SetActive(false);
                _prefabs.GetLastElement().SetActive(true);
                DisableResource();
                return;
            }

            _health.OnDeath += () => ItemsSaver.Instance.AddItem(_itemSaveID, ItemState.Disable);
        }

        _health.OnDeath += DisableResource;
        _health.OnTakeDamage += () =>
        {
            SpawnResource();
            ChangePrefab();
            _punchScaleAnimation.PlayAnimation();
        };
        _health.ResetHealth();
        _currentPrefab = _prefabs[0];
        _currentPrefab.SetActive(true);
    }

    // private void SetItemSaveID()
    // {
    //     var position = transform.localPosition;
    //     _itemSaveID = (int) ((position.x + position.y) * 10000);
    // }

    private void DisableResource()
    {
        _collider.enabled = false;
        _obstacle.enabled = false;
    }

    private void ChangePrefab()
    {
        if (_prefabs.Count <= 1)
        {
            _prefabs[0].SetActive(false);
            return;
        }

        _currentPrefab.SetActive(false);
        _prefabs.RemoveAt(0);
        _currentPrefab = _prefabs[0];
        _currentPrefab.SetActive(true);
    }

    private void SpawnResource()
    {
        for (int i = 0; i < _resourceCount; i++)
        {
            var resource = ResourcesPrefabs.Instance.SpawnResource(_resourceType, transform.position);
            resource.position = _spawnPoint.position;
            resource.GetComponent<Resource>()
                .MoveResource((Quaternion.AngleAxis(Random.Range(10, 350), Vector3.up) * _forceDirection) *
                              (_spawnForce + Random.Range(0, _spawnForce / 4)), false);
        }

        for (int i = 0; i < _emptyResources; i++)
        {
            var resource = ResourcesPrefabs.Instance.SpawnResource(_resourceType, transform.position);
            resource.position = _spawnPoint.position;
            resource.GetComponent<Resource>()
                .MoveResource((Quaternion.AngleAxis(Random.Range(10, 350), Vector3.up) * _forceDirection) *
                              (_spawnForce + Random.Range(0, _spawnForce / 4)), true);
        }
    }
}