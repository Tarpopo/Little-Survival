public class ZombieMove : State<ZombieData>
{
    public ZombieMove(ZombieData data, StateMachine<ZombieData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Data.UnitNavMesh.SetSpeed(Data.MoveSpeed);

        var targetPoint = Data.PathCreator.GetRandomPoint();
        targetPoint.y = Data.Transform.position.y;
        Data.UnitNavMesh.SetDestination(targetPoint);

        Data.AnimationComponent.PlayAnimation(UnitAnimations.Run, true);
        Data.Timer.StartTimer(40, ChangeState);
    }

    public override void LogicUpdate()
    {
        Data.Timer.UpdateTimer();
        if (Data.PathCreator.IsClosePoint(Data.Transform.position)) ChangeState();
    }

    public override void Exit() => Data.UnitNavMesh.SetSpeed(0);

    private void ChangeState()
    {
        Data.UnitNavMesh.ResetDestination();
        Machine.ChangeState<ZombieIdle>();
    }
}