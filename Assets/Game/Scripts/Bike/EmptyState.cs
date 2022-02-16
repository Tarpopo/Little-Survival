using DG.Tweening;

public class EmptyState : State<BikeData>
{
    public EmptyState(BikeData data, StateMachine<BikeData> stateMachine) : base(data, stateMachine)
    {
    }

    public override bool IsStatePlay() => true;
}