using UnityEngine;

public class Run : State<PlayerData>
{
    public Run(PlayerData data, StateMachine<PlayerData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("run");
    }

    //public override bool IsStatePlay() => Data.PlayerInput.IsMove;

    public override void PhysicsUpdate()
    {
        if (Data.PlayerInput.IsMove == false)
        {
            Machine.ChangeState<Idle>();
            return;
        }

        // if (Data.PlayerInput.Distance < Data.WalkDistance)
        // {
        //     Machine.ChangeState<Move>();
        //     return;
        // }

        Data.AnimationComponent.PlayAnimation(UnitAnimations.Run);
        Data.Transform.rotation = Quaternion.AngleAxis(-Data.PlayerInput.Angle + Data.AngleOffset, Vector3.up);
        Data.Rigidbody.MovePosition(Data.Rigidbody.position + Data.Transform.forward * Data.RunSpeed);
        // if (Data.SurfaceMovement.CheckGround() == false) return;
        // Data.Rigidbody.MovePosition(Data.Rigidbody.position +
        //                             Data.SurfaceMovement.Project(Data.Transform.forward) * Data.MoveSpeed);
    }
}