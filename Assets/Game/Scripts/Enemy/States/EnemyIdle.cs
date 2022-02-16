namespace Game.Scripts.Enemy.States
{
    public class EnemyIdle : State<EnemyData>
    {
        public EnemyIdle(EnemyData data, StateMachine<EnemyData> stateMachine) : base(data, stateMachine)
        {
        }

        public override void Enter()
        {
            Data.AnimationComponent.PlayAnimation(UnitAnimations.SearchIdle, true);
            Data.StartTimer(() => Machine.ChangeState<EnemyWalk>());
        }

        public override void Exit()
        {
            Data.UnitNavMesh.SetSpeed(Data.MoveSpeed);
        }

        public override void LogicUpdate()
        {
            Data.StateTimer.UpdateTimer();
            if (Data.VisibleZoneChecker.CheckTarget()) Machine.ChangeState<EnemyMoveToPlayer>();
        }
    }
}