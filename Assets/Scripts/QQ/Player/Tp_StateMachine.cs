using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tp_StateMachine : MonoBehaviour
{
    protected Tp_State State = null;

    public void SetState(Tp_State state)
    {
        State = state;

        State.Start();
    }
}