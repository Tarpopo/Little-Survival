using Game.Scripts.Interfaces;
using SquareDino;
using SquareDino.Scripts.MyAnalytics;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IPositionChanger
{
    public WeaponTypes Item => WeaponTypes.Axe;
    [SerializeField] private PlayerData _playerData;
    public Transform Target => transform;

    private StateMachine<PlayerData> _stateMachine;

    //private IDamageable _lastDamageable;
    private Vector3 _startPosition;
    public bool IsDeath => _playerData.Health.IsDeath == false;

    private void Start()
    {
        _playerData.GetAllData();
        ResourceCollector.Instance.SetCollectPoint(transform);
        _startPosition = transform.position;
        _stateMachine = new StateMachine<PlayerData>();
        _stateMachine.AddState(new Attack(_playerData, _stateMachine));
        _stateMachine.AddState(new Idle(_playerData, _stateMachine));
        _stateMachine.AddState(new Move(_playerData, _stateMachine));
        _stateMachine.AddState(new Upgrade(_playerData, _stateMachine));
        _stateMachine.AddState(new Run(_playerData, _stateMachine));
        _stateMachine.Initialize<Idle>();
        _playerData.PlayerInput.OnMove += () => _stateMachine.ChangeState<Move>();
        _playerData.PlayerInput.OnTouchUp += () => _stateMachine.ChangeState<Idle>();
        _playerData.AnimationComponent.SetParameters();
        _playerData.Health.ResetHealth();
        _playerData.Weapon.ResetWeapon();
        _playerData.Weapon.OnUpgrade += () =>
        {
            _stateMachine.ChangeState<Upgrade>();
            ParticleManager.Instance.PlayParticle(ParticleTypes.Upgrade, _playerData.Weapon.UpgradePoint, Vector3.zero,
                Vector3.one * 0.6f, null);
        };
        // _playerData.DamageableChecker.OnDamageableChange += () =>
        //     _playerData.Weapon.SwitchWeapon(_playerData.DamageableChecker.GetWeaponType);
        _playerData.Health.OnDeath += () =>
        {
            ParticleManager.Instance.PlayParticle(ParticleTypes.Death, transform.position, Vector3.zero,
                Vector3.one * 1.5f, null);
            _playerData.DamageableChecker.ResetAll();
            _stateMachine.ChangeState<Idle>();
            gameObject.SetActive(false);
            ResourceCollector.Instance.ClearAllResources();
            Invoke(nameof(OnDeath), 1);
        };
        ResetPlayer();
        //FindObjectOfType<LevelManager>().OnLevelLoaded += ResetPlayer;
    }

    private void OnDeath()
    {
        FindObjectOfType<LevelManager>().LoadLevel();
        MyAnalyticsManager.LevelFailed();
        MyAnalyticsManager.LevelRestart();
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        gameObject.SetActive(true);
        _playerData.Health.ResetHealth();
        ResetPosition();
        _playerData.Weapon.ResetWeapon();
        //ResourceCollector.Instance.ClearAllResources();
    }

    private void ResetPosition() => transform.position = _startPosition;

    private void Update() => _stateMachine.CurrentState.LogicUpdate();

    private void FixedUpdate() => _stateMachine.CurrentState.PhysicsUpdate();

    public void ChangePosition(Vector3 position)
    {
        _playerData.Rigidbody.useGravity = false;
        _playerData.Transform.eulerAngles = new Vector3(0, 180, 0);
        _playerData.Transform.position = position;
    }

    public void ResetPosition(Vector3 position)
    {
        _playerData.Rigidbody.useGravity = true;
        _playerData.Transform.position = position;
    }

    public void ApplyDamage()
    {
        MyVibration.Haptic(MyHapticTypes.LightImpact);
        _playerData.DamageableChecker.TryApplyDamage(_playerData.Weapon.Damage);
    }

    public void TakeDamage(Vector3 position, int damage)
    {
        _playerData.Health.ReduceHealth(damage);
        MyVibration.Haptic(MyHapticTypes.Failure);
    }
}

public enum WeaponTypes
{
    PickAxe,
    Axe,
    Stick,
    Arm
}