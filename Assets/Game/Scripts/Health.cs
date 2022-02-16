using System;
using System.Collections;
using Game.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem _damageParticle;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private WeaponTypes _item;
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private float _health = 3;
    [SerializeField] private int _delayFrames;
    [SerializeField] private int _healthFrames;
    [SerializeField] private bool _isRegenerate = true;
    [SerializeField] private bool _resetOnEnable;
    private float _currentHealth;
    public Transform Target => transform;
    public Vector3 DamagePosition { get; private set; }

    public WeaponTypes Item => _item;
    public float CurrentHealth => _currentHealth;
    public bool IsDeath => _currentHealth <= 0;

    // public void SetDeath(bool isDeath)
    // {
    //     _currentHealth = isDeath ? 0 : _health;
    // }

    public void TakeDamage(Vector3 position, int damage)
    {
        DamagePosition = position;
        if (_damageParticle != null)
        {
            _damageParticle.transform.position = position;
            _damageParticle.Play();
        }

        ReduceHealth(damage);
    }

    public event UnityAction OnDeath
    {
        add => _onDeath.AddListener(value);
        remove => _onDeath.RemoveListener(value);
    }

    public event UnityAction OnTakeDamage
    {
        add => _onTakeDamage.AddListener(value);
        remove => _onTakeDamage.RemoveListener(value);
    }

    public void ResetHealth()
    {
        _currentHealth = _health;
        if (_healthUI.IsHaveUI == false) return;
        _healthUI.UpdateUI(1, false);
    }

    public void ReduceHealth(int value)
    {
        if (IsDeath) return;
        _onTakeDamage?.Invoke();
        _currentHealth -= value;
        TryUpdateUI(_currentHealth * ((float) 1 / _health), true);
        if (IsDeath == false) return;
        if (_deathParticle != null) _deathParticle.Play();
        _onDeath?.Invoke();
    }

    private void OnEnable()
    {
        if (_resetOnEnable) ResetHealth();
    }

    private IEnumerator UpdateHealth()
    {
        for (int i = 0; i < _delayFrames; i++)
        {
            yield return null;
        }

        if (_isRegenerate == false)
        {
            _healthUI.UpdateUI(_currentHealth * (float) 1 / _health, false);
            yield break;
        }

        var delta = (_health - _currentHealth) / _healthFrames;
        for (int i = 0; i < _healthFrames; i++)
        {
            _currentHealth += delta;
            _healthUI.UpdateUI(_currentHealth * (float) 1 / _health, true);
            yield return null;
        }

        _healthUI.UpdateUI(1, false);
    }

    private void TryUpdateUI(float value, bool showUI)
    {
        if (_healthUI.IsHaveUI == false) return;
        _healthUI.UpdateUI(value, showUI);
        StopAllCoroutines();
        StartCoroutine(UpdateHealth());
    }
}