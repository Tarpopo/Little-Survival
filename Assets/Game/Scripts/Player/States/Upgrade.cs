public class Upgrade : State<PlayerData>
{
    private Timer _timer;

    public Upgrade(PlayerData data, StateMachine<PlayerData> stateMachine) : base(data, stateMachine) =>
        _timer = new Timer();

    public override void Enter()
    {
        _timer.StartTimer(Data.UpgradeTime, null);
        Data.AnimationComponent.PlayAnimation(UnitAnimations.Upgrade);
    }

    public override bool IsStatePlay() => _timer.IsTick;

    public override void LogicUpdate() => _timer.UpdateTimer();
}