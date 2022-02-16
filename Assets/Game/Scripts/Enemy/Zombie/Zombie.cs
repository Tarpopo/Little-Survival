using System;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private ZombieData _zombieData;
    private StateMachine<ZombieData> _stateMachine;

    private void Start()
    {
        _stateMachine = new StateMachine<ZombieData>();
        _stateMachine.AddState(new ZombieMove(_zombieData, _stateMachine));
        //_stateMachine.AddState(new ZombieDeath(_zombieData, _stateMachine));
        _stateMachine.AddState(new ZombieTakeDamage(_zombieData, _stateMachine));
        _stateMachine.AddState(new ZombieIdle(_zombieData, _stateMachine));
        _stateMachine.Initialize<ZombieIdle>();
        _zombieData.AnimationComponent.SetParameters();
        //_zombieData.Health.ResetHealth();
        _zombieData.Health.OnTakeDamage += () => _stateMachine.ChangeState<ZombieTakeDamage>();
        _zombieData.Health.OnDeath += () =>
        {
            ParticleManager.Instance.PlayParticle(ParticleTypes.Death, transform.position, Vector3.zero,
                Vector3.one * 1.5f, null);
            gameObject.SetActive(false);
        };
    }

    private void OnDisable() => _zombieData.PathCreator.OnDisable();

    private void Update() => _stateMachine.CurrentState.LogicUpdate();
    private void FixedUpdate() => _stateMachine.CurrentState.PhysicsUpdate();
}