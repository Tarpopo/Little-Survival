public class ZombieIdle : State<ZombieData>
{
    public ZombieIdle(ZombieData data, StateMachine<ZombieData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Data.AnimationComponent.PlayAnimation(UnitAnimations.SearchIdle, true);
        Data.StartTimer(() => Machine.ChangeState<ZombieMove>());
    }

    public override void LogicUpdate() => Data.Timer.UpdateTimer();

    public override void Exit()
    {
    }
}