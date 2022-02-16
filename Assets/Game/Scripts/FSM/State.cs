public abstract class State<T>
{
    protected readonly StateMachine<T> Machine;
    protected readonly T Data;
    protected State(T data,StateMachine<T> stateMachine)
    {
        Machine = stateMachine;
        Data = data;
    }

    public virtual bool IsStatePlay()
    {
        return false;
    }

    public virtual void Enter()
    {
    }
    public virtual void HandleInput()
    {
    }
    public virtual void LogicUpdate()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }
    public virtual void Exit()
    {
    }
    
}