using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FP_StateMachine : MonoBehaviour
{
    protected FP_State State = null;

    public void SetState(FP_State state)
    {
        State = state;

        State.Exit();

        State.Start();
    }
}