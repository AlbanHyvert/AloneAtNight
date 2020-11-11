using UnityEngine;

public abstract class Tp_State
{
    protected ThirdPersonMovement Player = null;

    public Tp_State(ThirdPersonMovement player)
    {
        Player = player;
    }

    public virtual void Start()
    {

    }

    public virtual void IsPushing(bool value)
    {

    }

    public virtual void IsClimbing(bool value)
    {

    }

    public virtual void IsThrowing(bool value)
    {

    }
}
