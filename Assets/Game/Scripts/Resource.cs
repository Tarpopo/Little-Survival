using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Resource : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMoveEnd;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private float _delay;
    [SerializeField] private float _flyDelay;
    [SerializeField] private int _frames = 10;
    private Vector3 _target = Vector3.zero;
    private Rigidbody _rigidbody;
    private Collider _collider;

    public event UnityAction OnMoveEnd
    {
        add => _onMoveEnd.AddListener(value);
        remove => _onMoveEnd.RemoveListener(value);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void SetEndPoint(Vector3 position) => _target = position;
    public void PlayMove() => MoveResource(Vector3.up, true);

    public void MoveResource(Vector3 startForceDirection, bool isEmpty) =>
        StartCoroutine(Move(startForceDirection, isEmpty));

    private IEnumerator Move(Vector3 forceDirection, bool isEmpty)
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        _rigidbody.velocity = forceDirection;
        _rigidbody.angularVelocity = forceDirection;
        yield return new WaitForSeconds(_delay + Random.Range(0, _flyDelay));
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        if (_target == Vector3.zero) _target = ResourceCollector.Instance.CollectPosition;
        var delta = (_target - transform.position) / _frames;
        for (int i = 0; i < _frames; i++)
        {
            transform.position += delta;
            yield return null;
        }

        _target = Vector3.zero;
        _onMoveEnd?.Invoke();
        _onMoveEnd?.RemoveAllListeners();

        ManagerPool.Instance.Despawn(PoolType.Entities, gameObject);
        if (isEmpty) yield break;
        ResourceCollector.Instance.AddResource(_resourceType);
    }
}