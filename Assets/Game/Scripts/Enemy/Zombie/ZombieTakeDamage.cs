public class ZombieTakeDamage : State<ZombieData>
{
    public ZombieTakeDamage(ZombieData data, StateMachine<ZombieData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Data.UnitNavMesh.ResetDestination();
        Data.AnimationComponent.PlayAnimation(UnitAnimations.TakeDamage, true);
        Data.Timer.StartTimer(Data.TakeDamageTime, () => Machine.ChangeState<ZombieIdle>());
    }

    public override void Exit()
    {
    }

    public override void PhysicsUpdate() => Data.Timer.UpdateTimer();
}