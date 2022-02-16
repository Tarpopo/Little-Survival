using System;
using Game.Scripts;
using UnityEngine;

[Serializable]
public abstract class BaseUnitData<T> where T : Enum
{
    public AnimationComponent<T> AnimationComponent => _animationComponent;
    public DamageableTriggerChecker DamageableChecker => _damageableChecker;
    public Health Health => health;
    public SurfaceMovement SurfaceMovement => _surfaceMovement;
    public Rigidbody Rigidbody => _rigidbody;
    public Transform Transform => _transform;
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private Health health;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private AnimationComponent<T> _animationComponent;
    [SerializeField] private DamageableTriggerChecker _damageableChecker;
    [SerializeField] private SurfaceMovement _surfaceMovement;
}