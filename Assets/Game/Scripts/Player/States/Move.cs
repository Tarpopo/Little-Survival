using UnityEngine;

public class Move : State<PlayerData>
{
    public Move(PlayerData data, StateMachine<PlayerData> stateMachine) : base(data, stateMachine)
    {
    }

    public override bool IsStatePlay() => Data.PlayerInput.IsMove;
    public override void Enter() => Data.Weapon.SwitchWeapon(WeaponTypes.Arm);

    public override void Exit()
    {
        Data.AnimationComponent.SetSpeed(0);
        //Data.UnitNavMesh.SetDestination(Data.Rigidbody.position);
        
    }

    public override void PhysicsUpdate()
    {
        if (Data.PlayerInput.IsMove == false)
        {
            Machine.ChangeState<Idle>();
            return;
        }

        Data.AnimationComponent.PlayAnimation(UnitAnimations.Run, true);
        var lerpSpeed = Mathf.InverseLerp(0, 80, Data.PlayerInput.Distance);
        Data.AnimationComponent.SetSpeed(lerpSpeed);
        Data.Transform.rotation = Quaternion.AngleAxis(-Data.PlayerInput.Angle + Data.AngleOffset, Vector3.up);
        //if (Data.SurfaceMovement.CheckGround() == false) return;
        lerpSpeed = lerpSpeed < Data.MinWalkSpeed ? Data.MinWalkSpeed : lerpSpeed;
        //Data.Rigidbody.MovePosition(Data.Rigidbody.position + Data.Transform.forward * (Data.MoveSpeed * lerpSpeed));

        //Data.UnitNavMesh.SetSpeed(Data.MoveSpeed * lerpSpeed);
        //Data.UnitNavMesh.SetDestination(Data.Rigidbody.position + Data.Transform.forward * (Data.MoveSpeed * lerpSpeed));
        Data.UnitNavMesh.Move(Data.Transform.position + Data.Transform.forward * (Data.MoveSpeed * lerpSpeed));
    }
}