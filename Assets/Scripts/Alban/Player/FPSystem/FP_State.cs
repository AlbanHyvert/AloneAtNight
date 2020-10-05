using UnityEngine;

public abstract class FP_State
{
    protected FP_Controller Controller = null;

    public FP_State(FP_Controller controller)
    {
        Controller = controller;
    }

    public virtual void Start()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Movement(Vector3 direction)
    {

    }

    public virtual void Interact()
    {

    }

    public virtual void Throw()
    {

    }
}