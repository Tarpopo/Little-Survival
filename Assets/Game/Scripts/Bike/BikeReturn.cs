using UnityEngine;

public class BikeReturn : BaseBikeMove
{
    public BikeReturn(BikeData data, StateMachine<BikeData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //if (Data.PlayerInput.IsMouseDown == false) return;
        //Data.PlayerInput.OnFingerMove += ChangeState;
    }

    //public override void Exit() => Data.PlayerInput.OnFingerMove -= ChangeState;

   //private void ChangeState() => Machine.ChangeState<BikeMove>();

    // public override void PhysicsUpdate()
    // {
    //     base.PhysicsUpdate();
    //     var target = Data.Transform.eulerAngles;
    //     var current = target;
    //     current.x = ClampAngle(current.x);
    //     //current.y = ClampAngle(current.y);
    //     target.x = 0;
    //     //target.y = 90;
    //     Data.Transform.eulerAngles = Vector3.MoveTowards(current, target, Data.XRotateSpeed * 1.5f);
    // }
}