using UnityEngine;

public class ZombieDeath : State<ZombieData>
{
    public ZombieDeath(ZombieData data, StateMachine<ZombieData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        // Data.AnimationComponent.PlayAnimation(UnitAnimations.TakeDamage, true);
        // Data.Timer.StartTimer(Data.DeathDelay, DestroyZombie);
    }

    //public override void PhysicsUpdate() => Data.StateTimer.UpdateTimer();
    
}