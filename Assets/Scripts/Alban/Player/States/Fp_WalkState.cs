using UnityEngine;

public class Fp_WalkState : State
{
    public Fp_WalkState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    { 
        Player.transform.localScale = Player.GetData.standingSize;
        Player.SetCurrentState = E_PlayerState.WALKING;
    }

    public override void IsCrouch(bool value)
    {
        if(value == true)
        {
            Player.SetState(new Fp_CrouchState(Player));
        }
    }

    public override void Move(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            Player.SetState(new Fp_IdleState(Player));
        }

        float speed = Player.MovementData.MovingSpeed;

        Player.GetData.cameraController.GetData.headBobbing.OnWalk();

        Player.Controller.Move(dir * speed * Time.deltaTime);
    }

    public override void IsGrounded(bool value)
    {
        if (value == false)
        {
            Player.SetState(new Fp_FallState(Player));
        }
    }
}
