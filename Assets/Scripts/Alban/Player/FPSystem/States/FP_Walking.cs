using UnityEngine;

public class FP_Walking : FP_State
{
    private Data _data;
    private float _time = 0;

    private struct Data
    {
        public int speed;
        public float smoothSpeed;
        public Rigidbody rb;
        public FP_Camera cameraController;
    }

    public FP_Walking(FP_Controller controller) : base(controller)
    {

    }

    public override void Start()
    {
        _time = 0;
        _data.rb = Controller.GetBody.rb;
        _data.cameraController = Controller.GetBody.cameraController;
        _data.speed = Controller.GetData.GetSpeed;
        _data.smoothSpeed = (float)Controller.GetData.GetSmoothSpeed;

        InputManager.Instance.UpdateDirection += Movement;
    }

    public override void Exit()
    {
        InputManager.Instance.UpdateDirection -= Movement;
    }

    public override void Movement(Vector3 direction)
    {
        if(direction == Vector3.zero)
        {
            Controller.SetState(new FP_Idle(Controller));
        }

        _time += _data.smoothSpeed * Time.deltaTime;

        direction *= _data.speed * _time;

        _data.rb.MovePosition(direction);
    }

    public override void Interact()
    {
        
    }

    public override void Throw()
    {
        
    }
}
