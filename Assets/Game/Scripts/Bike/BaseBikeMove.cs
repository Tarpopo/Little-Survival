public class BaseBikeMove : State<BikeData>
{
    public BaseBikeMove(BikeData data, StateMachine<BikeData> stateMachine) : base(data, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void PhysicsUpdate()
    {
        Data.transform.position += -Data.Transform.right * Data.MoveSpeed;
    }

    protected float ClampAngle(float angle) => angle >= 180 ? angle -= 360 : angle;
}