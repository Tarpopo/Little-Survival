using UnityEngine;

namespace Game.Scripts.Bike
{
    public class BikeDamage : BikeMove
    {
        public BikeDamage(BikeData data, StateMachine<BikeData> stateMachine) : base(data, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // Data.PlayerInput.OnFingerMove -= SetDirection;
            // Data.PlayerInput.OnFingerMove += SetDirectionWithoutTimer;
            //Data.AnimationComponent.PlayAnimation(UnitAnimations.TakeDamage);
            Data.DamageTimer.StartTimer(Data.DamageTime, () =>
            {
                Machine.ChangeState<BikeMove>();
                // if (Data.PlayerInput.IsMouseDown) Machine.ChangeState<BikeMove>();
                // else Machine.ChangeState<BikeReturn>();
            });
            Data.SetDamageSpeed();
        }

        public override bool IsStatePlay() => Data.Timer.IsTick;

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Data.DamageTimer.UpdateTimer();
            // if (Data.PlayerInput.IsMouseDown) return;
            // var target = Data.Transform.eulerAngles;
            // var current = target;
            // current.x = ClampAngle(current.x);
            // target.x = 0;
            // Data.Transform.eulerAngles = Vector3.MoveTowards(current, target, Data.XRotateSpeed * 1.5f);
        }

        public override void Exit()
        {
            base.Exit();
            //Data.PlayerInput.OnFingerMove -= SetDirectionWithoutTimer;
            Data.ResetSpeed();
        }
    }
}