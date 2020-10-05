using UnityEngine;

public class FP_Idle : FP_State
{
    private Rigidbody _rb = null;

    public FP_Idle(FP_Controller controller) : base (controller)
    {
        
    }

    public override void Start()
    {
        _rb = Controller.GetBody.rb;
        InputManager.Instance.UpdateDirection += Movement;
    }

    public override void Exit()
    {
        InputManager.Instance.UpdateDirection -= Movement;
    }

    public override void Movement(Vector3 direction)
    {
        if(direction != Vector3.zero)
        {
            Controller.SetState(new FP_Walking(Controller));
        }

        _rb.MovePosition(direction);
    }

    public override void Interact()
    {

    }

    public override void Throw()
    {

    }
}
