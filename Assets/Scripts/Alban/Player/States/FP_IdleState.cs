using UnityEngine;

public class Fp_IdleState : State
{
    public Fp_IdleState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    {
        Player.transform.localScale = Player.GetData.standingSize;
        Player.Animator.SetBool("isCrouch", false);
        Player.Animator.SetFloat("WalkSpeed", 0);
        Player.SetCurrentState = E_PlayerState.IDLE;
    }

    public override void IsCrouch(bool value)
    {
        if (value == true)
        {
            Player.Animator.SetBool("isCrouch", true);
            Player.SetState(new Fp_IdleCrouchState(Player));
        }
    }

    public override void Move(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            Player.SetState(new Fp_WalkState(Player));
        }

        Player.Animator.SetFloat("WalkSpeed", 0);

        Player.GetData.cameraController.GetData.headBobbing.OnIdle();
    }

    public override void IsGrounded(bool value)
    {
        if(value == false)
        {
            Player.Animator.SetBool("isFalling", true);
            Player.SetState(new Fp_FallState(Player));
        }
        else
        {
            Player.Animator.SetBool("isFalling", false);
        }
    }
}
