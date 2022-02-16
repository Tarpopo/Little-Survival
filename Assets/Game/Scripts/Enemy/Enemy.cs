using System;
using Game.Scripts.Enemy.States;
using Game.Scripts.Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private bool _trySetTarget;
    [SerializeField] private bool _save;
    [SerializeField] private int _saveID;

    public WeaponTypes Item => WeaponTypes.Arm;
    public Transform Target => transform;
    private StateMachine<EnemyData> _stateMachine;
    private IDamageable _lastDamageable;
    public bool IsDeath => _enemyData.Health.IsDeath;

    private void Start()
    {
        _stateMachine = new StateMachine<EnemyData>();
        _stateMachine.AddState(new EnemyAttack(_enemyData, _stateMachine));
        _stateMachine.AddState(new EnemyIdle(_enemyData, _stateMachine));
        _stateMachine.AddState(new EnemyWalk(_enemyData, _stateMachine));
        _stateMachine.AddState(new TakeDamage(_enemyData, _stateMachine));
        _stateMachine.AddState(new EnemyMoveToPlayer(_enemyData, _stateMachine));
        _stateMachine.Initialize<EnemyIdle>();
        _enemyData.AnimationComponent.SetParameters();
        _enemyData.UnitNavMesh.SetSpeed(_enemyData.MoveSpeed);
        if (_trySetTarget) _enemyData.VisibleZoneChecker.SetTarget(FindObjectOfType<Player>().transform);
        _enemyData.Health.OnTakeDamage += () => _stateMachine.ChangeState<TakeDamage>();
        _enemyData.Health.OnDeath += OnDeath;
        if (_save == false) return;
        if (ItemsSaver.Instance.GetItemState(_saveID) != ItemState.Disable) return;
        _enemyData.Health.OnDeath -= OnDeath;
        _enemyData.Health.ReduceHealth(3);
        gameObject.SetActive(false);
    }

    private void OnDeath()
    {
        ParticleManager.Instance.PlayParticle(ParticleTypes.Death, transform.position, Vector3.zero,
            Vector3.one * 1.5f, null);
        gameObject.SetActive(false);
    }

    private void OnEnable() => _enemyData.Health.ResetHealth();

    private void OnDisable()
    {
        _enemyData.PathCreator.OnDisable();
        if (_save == false) return;
        ItemsSaver.Instance.AddItem(_saveID, ItemState.Disable);
    }

    private void Update() => _stateMachine.CurrentState.LogicUpdate();

    private void FixedUpdate() => _stateMachine.CurrentState.PhysicsUpdate();

    public void ApplyDamage()
    {
        _enemyData.VisibleZoneChecker.GetDamageable().TakeDamage(transform.position, _enemyData.Damage);
    }

    public void SetActive(bool active)
    {
        if (_enemyData.Health.IsDeath) return;
        gameObject.SetActive(active);
    }
    //public void TakeDamage(Vector3 position, int damage) => _enemyData.Health.ReduceHealth(damage);
}