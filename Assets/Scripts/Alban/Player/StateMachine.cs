using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State State = null;

    public void SetState(State state)
    {
        State = state;

        State.Start();
    }
}