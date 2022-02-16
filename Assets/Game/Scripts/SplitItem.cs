using System;
using Game.Scripts.Interfaces;
using SquareDino;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SplitItem : MonoBehaviour //, IDamageable
{
    [SerializeField] private Transform _parent;
    [SerializeField] private UnityEvent _onItemSplit;
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private float _destroyVelocity = 1;
    [SerializeField] private float _rotationVelocity = 1;
    [SerializeField] private float _health;
    [SerializeField] private float _deactiveDelay;
    [SerializeField] private bool _spawnResource;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private bool _damageable = true;
    private Vector3 _forceDirection = new Vector3(0.5f, 0.5f, 0.5f);
    public Transform Target => transform;
    public WeaponTypes Item => WeaponTypes.Axe;
    public bool IsDeath => _health <= 0;

    public void TakeDamage(Vector3 position, int damage)
    {
        if (_damageable == false || IsDeath) return;
        _health -= damage;
        if (IsDeath == false) return;
        Invoke(nameof(DisableItem), _deactiveDelay);
        SplitObject();
    }

    public void TakeDamage() => TakeDamage(Vector3.zero, 1);

    public void SplitObject()
    {
        if (_spawnResource) _resourceSpawner.SpawnResource();
        MyVibration.Haptic(MyHapticTypes.LightImpact);
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.gameObject.SetActive(true);
            rigidbody.transform.parent = _parent;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            //var direction = (rigidbody.position - transform.position).normalized;
            var direction = Quaternion.AngleAxis(Random.Range(10, 350), Vector3.up) * _forceDirection;
            rigidbody.velocity = direction * _destroyVelocity;
            rigidbody.angularVelocity = direction * _destroyVelocity;
        }

        _onItemSplit?.Invoke();
    }

    private void DisableItem()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (var rigidbody in _rigidbodies) Destroy(rigidbody.gameObject);
    }
}