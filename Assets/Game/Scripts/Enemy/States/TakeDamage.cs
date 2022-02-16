using UnityEngine;

public class TakeDamage : State<EnemyData>
{
    public TakeDamage(EnemyData data, StateMachine<EnemyData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Data.UnitNavMesh.ResetDestination();
        Data.UnitNavMesh.SetAcceleration(false);
        Data.UnitNavMesh.SetSpeed(0);
        Data.AnimationComponent.PlayAnimation(UnitAnimations.TakeDamage, true);
        var damageForceDirection = Data.Transform.position - Data.Health.DamagePosition;
        damageForceDirection.y = 0;
        Data.Rigidbody.velocity = damageForceDirection.normalized * Data.DamageForce;
        Data.StateTimer.StartTimer(Data.TakeDamageTime, () =>
        {
            Data.Rigidbody.velocity = Vector3.zero;
            Machine.ChangeState<EnemyWalk>();
        });
    }

    public override void Exit()
    {
        Data.UnitNavMesh.SetSpeed(Data.MoveSpeed);
        Data.UnitNavMesh.SetAcceleration(true);
    }

    public override bool IsStatePlay() => Data.StateTimer.IsTick;

    public override void PhysicsUpdate() => Data.StateTimer.UpdateTimer();
}