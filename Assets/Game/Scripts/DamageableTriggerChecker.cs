using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class DamageableTriggerChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent _onDamageableChange;
    [SerializeField] private int _damageLayerIndex;
    [SerializeField] private float _visibleAngle;

    private bool _isDamageableVisible => _damageables.Any(item =>
        Vector3.Angle(transform.forward, item.Target.position - transform.position) <=
        _visibleAngle);

    public event UnityAction OnDamageableChange
    {
        add => _onDamageableChange.AddListener(value);
        remove => _onDamageableChange.RemoveListener(value);
    }

    private readonly List<IDamageable> _damageables = new List<IDamageable>();
    private IDamageable _current;
    public Vector3 TargetDirection => (_damageables[0].Target.position - transform.position).normalized;
    public WeaponTypes GetWeaponType => _current.Item;

    public bool CheckDamageable() => _damageables.Count != 0 && _isDamageableVisible;

    public void TryApplyDamage(int damage)
    {
        for (int i = 0; i < _damageables.Count; i++)
        {
            if (Vector3.Angle(transform.forward, _damageables[i].Target.position - transform.position) >
                _visibleAngle) continue;
            _damageables[i].TakeDamage(transform.position, damage);
            if (_damageables[i].IsDeath)
            {
                _damageables.Remove(_damageables[i]);
                TrySetCurrent();
            }
        }
        // foreach (var damageable in _damageables)
        // {
        //     damageable.TakeDamage(transform.position, damage);
        //     if (damageable.IsDeath)
        //     {
        //         _damageables.Remove(damageable);
        //         TrySetCurrent();
        //     }
        // }
    }

    public void ResetAll()
    {
        _damageables.Clear();
        _current = null;
    }

    private void TrySetCurrent()
    {
        if (CheckDamageable() == false) return;
        _current = _damageables[0];
        _onDamageableChange?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _damageLayerIndex) return;
        _damageables.Add(other.GetComponent<IDamageable>());
        if (_damageables.Count == 1) TrySetCurrent();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != _damageLayerIndex) return;
        var damageable = other.GetComponent<IDamageable>();
        _damageables.Remove(damageable);
        if (_current.Equals(damageable)) TrySetCurrent();
    }
}