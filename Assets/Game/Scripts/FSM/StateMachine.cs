using System.Collections.Generic;
using System.Linq;

public class StateMachine<T>
{
    public State<T> CurrentState { get; private set; }
    private List<State<T>> _states;

    public StateMachine()
    {
        _states = new List<State<T>>();
    }

    public void Initialize<V>() where V : State<T>
    {
        CurrentState = _states.FirstOrDefault(elem => elem is V);
        CurrentState.Enter();
    }

    public void AddState(State<T> state)
    {
        _states.Add(state);
    }

    public void ChangeState<V>() where V : State<T>
    {
        if (CurrentState.IsStatePlay()) return;
        var newState = _states.FirstOrDefault(elem => elem is V);
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }

    public void ChangeStateImmediately<V>() where V : State<T>
    {
        var newState = _states.FirstOrDefault(elem => elem is V);
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}