namespace Game.Scripts.Enemy.States
{
    public class EnemyAttack : State<EnemyData>
    {
        public EnemyAttack(EnemyData data, StateMachine<EnemyData> stateMachine) : base(data, stateMachine)
        {
        }

        public override void Enter() => Data.AnimationComponent.PlayAnimation(UnitAnimations.MeleeAttack,true);

        public override void LogicUpdate()
        {
            if (Data.VisibleZoneChecker.IsTargetExist == false)
            {
                Machine.ChangeState<EnemyWalk>();
                return;
            }
            Data.Transform.forward = Data.VisibleZoneChecker.TargetDirection;
            if (Data.VisibleZoneChecker.IsCloseDistance == false) Machine.ChangeState<EnemyMoveToPlayer>();
        }
    }
}