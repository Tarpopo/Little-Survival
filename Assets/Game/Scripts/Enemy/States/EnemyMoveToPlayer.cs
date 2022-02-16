using Game.Scripts.Enemy.States;

public class EnemyMoveToPlayer : State<EnemyData>
{
    public EnemyMoveToPlayer(EnemyData data, StateMachine<EnemyData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        Data.AnimationComponent.PlayAnimation(UnitAnimations.Run, true);
        if (Data.VisibleZoneChecker.IsTargetDistance == false)
        {
            Data.UnitNavMesh.ResetDestination();
            Machine.ChangeState<EnemyWalk>();
            return;
        }

        //Data.Transform.forward = Data.VisibleZoneChecker.TargetDirection;
        if (Data.VisibleZoneChecker.IsCloseDistance)
        {
            Data.UnitNavMesh.ResetDestination();
            Machine.ChangeState<EnemyAttack>();
            return;
        }

        Data.UnitNavMesh.SetDestination(Data.VisibleZoneChecker.TargetPosition);
    }

    public override void Exit() => Data.UnitNavMesh.ResetDestination();
}