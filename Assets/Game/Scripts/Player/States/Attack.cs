using System;
using System.Collections.Generic;

public class Attack : State<PlayerData>
{
    private List<Type> _list = new List<Type>();

    public Attack(PlayerData data, StateMachine<PlayerData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
        Data.AnimationComponent.PlayAnimation(UnitAnimations.MeleeAttack, true);
    }

    public override void PhysicsUpdate()
    {
        if (Data.DamageableChecker.CheckDamageable())
        {
            Data.Weapon.SwitchWeapon(Data.DamageableChecker.GetWeaponType);
            // var direction = Data.DamageableChecker.TargetDirection;
            // direction.y = Data.Transform.forward.y;
            // Data.Transform.forward = direction;
            return;
        }

        Machine.ChangeState<Idle>();
    }
}