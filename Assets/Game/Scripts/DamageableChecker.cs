using System;
using Game.Scripts.Interfaces;
using UnityEngine;

[Serializable]
public class DamageableChecker
{
    [SerializeField] private int _damage;
    [SerializeField] private Transform _rayPoint;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _damageableLayer;
    private RaycastHit _raycastHit;
    private IDamageable _damageable;

    public void ApplyDamage() => _damageable?.TakeDamage(_rayPoint.position, _damage);

    public bool CheckDamageable()
    {
        if (Physics.Raycast(_rayPoint.position, _rayPoint.forward, out _raycastHit, _rayDistance,
            _damageableLayer) == false) return false;
        _damageable = _raycastHit.collider.GetComponent<IDamageable>();
        return true;
    }
}