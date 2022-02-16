using UnityEngine;

public class BikeMove : BaseBikeMove
{
    protected float _distance;
    private int _direction = 1;

    public BikeMove(BikeData data, StateMachine<BikeData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Data.PlayerInput.OnFingerMove += SetDirection;
        Data.PlayerInput.OnFingerStop += SetSitPose;
        Data.PlayerInput.OnTouchDown += SetDistance;
        SetDistance();
        //StartRotateTimer();
        Data.ResetSpeed();
    }

    public override void Exit()
    {
        Data.PlayerInput.OnFingerStop -= SetSitPose;
        Data.PlayerInput.OnFingerMove -= SetDirection;
        Data.PlayerInput.OnTouchDown -= SetDistance;
    }

    private void SetSitPose()
    {
        Data.AnimationComponent.PlayAnimation(UnitAnimations.BikeSit, true);
    }

    // private void StartRotateTimer() => Data.Timer.StartTimer(Data.RotateTime,
    //     () => Data.AnimationComponent.PlayAnimation(UnitAnimations.BikeSit, true));

    private float GetLerpSpeed() => Mathf.InverseLerp(0, 110, Data.PlayerInput.FingerDistance) *
                                    Data.PlayerInput.XFingerMoveDirection;

    private void SetDirection()
    {
        Data.AnimationComponent.PlayAnimation(UnitAnimations.BikeLeft, true);
        Data.AnimationComponent.SetSpeed(GetLerpSpeed());
        //Debug.Log(Data.PlayerInput.FingerDistance);
    }

    private void SetDistance() =>
        _distance = Data.Transform.position.z - Data.PlayerInput.GetTouchOnPlane(Data.Transform.position).z;

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Data.Timer.UpdateTimer();
        if (Data.PlayerInput.IsActiveRadius == false)
        {
            //Data.AnimationComponent.PlayAnimation(UnitAnimations.BikeSit, true);
            return;
        }

        var position = Data.Transform.position;
        position.z = Data.PlayerInput.GetTouchOnPlane(position).z + _distance;
        position.z = Mathf.Clamp(position.z, Data.PositionBorders.x, Data.PositionBorders.y);
        Data.Transform.position = position;
    }
}