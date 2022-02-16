using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ZombieData : BaseUnitData<UnitAnimations>
{
    //[SerializeField] private VisibleZoneChecker _visibleZoneChecker;
    [SerializeField] private UnitNavMesh unitNavMesh;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Vector2 _maxMinStateTime;
    //[SerializeField] private int _damage;
    [SerializeField] private float _takeDamageTime;
    //[SerializeField] private float _damageForce;
    private Timer _stateTimer = new Timer();
    public float TakeDamageTime => _takeDamageTime;
    //public float DamageForce => _damageForce;
    //public int Damage => _damage;
    public Timer Timer => _stateTimer;
    public PathCreator PathCreator => _pathCreator;
    //public VisibleZoneChecker VisibleZoneChecker => _visibleZoneChecker;
    public UnitNavMesh UnitNavMesh => unitNavMesh;

    public void StartTimer(Action action) =>
        _stateTimer.StartTimer(Random.Range(_maxMinStateTime.x, _maxMinStateTime.y), action);
}