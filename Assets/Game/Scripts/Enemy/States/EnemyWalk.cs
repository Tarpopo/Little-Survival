using Game.Scripts.Enemy.States;
using UnityEngine;

public class EnemyWalk : State<EnemyData>
{
    public EnemyWalk(EnemyData data, StateMachine<EnemyData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        var targetPoint = Data.PathCreator.GetRandomPoint();
        targetPoint.y = Data.Transform.position.y;
        Data.UnitNavMesh.SetDestination(targetPoint);
        Data.AnimationComponent.PlayAnimation(UnitAnimations.Run, true);
        Data.StateTimer.StartTimer(40, ChangeState);
    }

    public override void LogicUpdate()
    {
        Data.StateTimer.UpdateTimer();
        if (Data.PathCreator.IsClosePoint(Data.Transform.position)) ChangeState();
        else if (Data.VisibleZoneChecker.CheckTarget()) Machine.ChangeState<EnemyMoveToPlayer>();
    }

    public override void Exit() => Data.UnitNavMesh.SetSpeed(Data.MoveSpeed);

    private void ChangeState()
    {
        Data.UnitNavMesh.ResetDestination();
        Machine.ChangeState<EnemyIdle>();
    }
}