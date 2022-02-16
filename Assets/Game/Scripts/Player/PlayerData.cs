using System;
using Game.Scripts;
using UnityEngine;

[Serializable]
public class PlayerData : BaseUnitData<UnitAnimations>
{
    public Weapon Weapon => _weapon;
    public PlayerInput PlayerInput => _playerInput;
    public UnitNavMesh UnitNavMesh => _unitNavMesh;
    public float AngleOffset => _angleOffset;
    public float IdleTime => _idleTime;
    public float RunSpeed => _runSpeed;
    public float MinWalkSpeed => _minWalkSpeed;
    public float WalkDistance => _walkDistance;
    public float UpgradeTime => _upgradeTime;
    [SerializeField] private float _upgradeTime;
    [SerializeField] private float _angleOffset;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _walkDistance;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _minWalkSpeed;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private UnitNavMesh _unitNavMesh;
    public void GetAllData()
    {
        _playerInput = PlayerInput.Instance;
    }
}