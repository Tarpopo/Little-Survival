using Game.Scripts.Bike;
using UnityEngine;
using UnityEngine.Events;

public class Bike : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMovedToFinish;
    [SerializeField] private BikeData _bikeData;
    [SerializeField] private int _finishFrames;
    private StateMachine<BikeData> _stateMachine;

    private void Start()
    {
        _bikeData.GetAllData();
        _stateMachine = new StateMachine<BikeData>();
        _stateMachine.AddState(new BikeMove(_bikeData, _stateMachine));
        //_stateMachine.AddState(new BikeReturn(_bikeData, _stateMachine));
        _stateMachine.AddState(new BikeDamage(_bikeData, _stateMachine));
        _stateMachine.AddState(new EmptyState(_bikeData, _stateMachine));
        _stateMachine.Initialize<BikeMove>();
        _bikeData.AnimationComponent.SetParameters();
        _bikeData.AnimationComponent.PlayAnimation(UnitAnimations.BikeSit);
        _bikeData.DamageableChecker.OnGetDamageable +=
            () => _bikeData.AnimationComponent.PlayAnimation(UnitAnimations.BikeAttack, true);
        //_bikeData.PlayerInput.OnMove += () => _stateMachine.ChangeState<BikeMove>();
        //_bikeData.PlayerInput.OnTouchDown += () => _stateMachine.ChangeState<BikeMove>();
        //_bikeData.PlayerInput.OnTouchUp += () => _stateMachine.ChangeState<BikeReturn>();
    }

    public void MoveToFinish()
    {
        _stateMachine.ChangeStateImmediately<EmptyState>();
        _bikeData.AnimationComponent.PlayAnimation(UnitAnimations.BikeWait);
        StartCoroutine(CorroutinesKid.FixedMove(transform, _bikeData.FinishPosition, _finishFrames,
            () => _onMovedToFinish?.Invoke()));
    }

    public void OnTouchObstacles() => _stateMachine.ChangeState<BikeDamage>();
    private void Update() => _stateMachine.CurrentState.LogicUpdate();
    private void FixedUpdate() => _stateMachine.CurrentState.PhysicsUpdate();

    private void OnDisable() => _stateMachine.CurrentState.Exit();
}