using UnityEngine;

public abstract class State
{
    protected FP_Controller Player = null;

    public State(FP_Controller player)
    {
        Player = player;
    }

    public virtual void Start()
    {

    }

    public virtual void IsCrouch(bool value)
    {

    }

    public virtual void IsLookingAt(bool value)
    {

    }

    public virtual void Move(Vector3 dir)
    {

    }

    public virtual void Tick()
    {

    }
}
