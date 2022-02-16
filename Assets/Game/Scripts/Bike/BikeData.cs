using Game.Scripts.Bike;
using UnityEngine;

public class BikeData : MonoBehaviour
{
    public AnimationComponent<UnitAnimations> AnimationComponent => _animationComponent;
    public DamagableChecker DamageableChecker => _damageableChecker;
    public PlayerInput PlayerInput => _playerInput;
    public Rigidbody Rigidbody => _rigidbody;
    public Transform Transform => _transform;
    public Vector3 FinishPosition => _finishPoint.position;
    public float HorizontalMoveSpeed => _horizontalMoveSpeed;
    public Vector2 PositionBorders => _positionBorders;
    public Vector2 XRotateBorders => _xRotateBorders;
    public Vector2 YRotateBorders => _yRotateBorders;
    public float XRotateSpeed => _xRotateSpeed;
    public float YRotateSpeed => _yRotateSpeed;
    public float MoveSpeed => _moveSpeed;
    public float RotateTime => _rotateTime;
    public float DamageTime => _damageTime;
    public float FingerMoveDelay => _fingerMoveDelay;
    public float DistanceDelta => _distanceDelta;
    public Timer Timer { get; } = new Timer();
    public Timer DamageTimer { get; } = new Timer();

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector2 _xRotateBorders;
    [SerializeField] private Vector2 _yRotateBorders;
    [SerializeField] private Vector2 _positionBorders;
    [SerializeField] private float _horizontalMoveSpeed;
    [SerializeField] private float _forwardMoveSpeed;
    [SerializeField] private float _damageSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _xRotateSpeed;
    [SerializeField] private float _yRotateSpeed;
    [SerializeField] private float _distanceDelta;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _damageTime;
    [SerializeField] private float _fingerMoveDelay;
    [SerializeField] private AnimationComponent<UnitAnimations> _animationComponent;
    [SerializeField] private DamagableChecker _damageableChecker;
    [SerializeField] private Transform _finishPoint;
    public void SetDamageSpeed() => _moveSpeed = _damageSpeed;
    public void ResetSpeed() => _moveSpeed = _forwardMoveSpeed;

    public void GetAllData()
    {
        ResetSpeed();
        _playerInput = PlayerInput.Instance;
    }
}