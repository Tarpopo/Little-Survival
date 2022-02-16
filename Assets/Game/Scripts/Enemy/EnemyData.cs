using System;
using Game.Scripts.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyData : BaseUnitData<UnitAnimations>
{
    [SerializeField] private VisibleZoneChecker _visibleZoneChecker;
    [SerializeField] private UnitNavMesh unitNavMesh;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Vector2 _maxMinStateTime;
    [SerializeField] private int _damage;
    [SerializeField] private float _takeDamageTime;
    [SerializeField] private float _damageForce;

    public float TakeDamageTime => _takeDamageTime;
    public float DamageForce => _damageForce;
    public int Damage => _damage;
    private Timer _stateTimer = new Timer();
    public Timer StateTimer => _stateTimer;
    public PathCreator PathCreator => _pathCreator;
    public VisibleZoneChecker VisibleZoneChecker => _visibleZoneChecker;
    public UnitNavMesh UnitNavMesh => unitNavMesh;

    public void StartTimer(Action action) =>
        _stateTimer.StartTimer(Random.Range(_maxMinStateTime.x, _maxMinStateTime.y), action);
}