using UnityEngine;

public class Fp_IdleCrouchState : State
{
    public Fp_IdleCrouchState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    {
        Player.transform.localScale = Player.GetData.crouchSize;
        Player.Animator.SetBool("isCrouch", true);
        Player.SetCurrentState = E_PlayerState.CROUCHING;
    }

    public override void IsCrouch(bool value)
    {
        if (value == false)
        {
            Player.Animator.SetBool("isCrouch", false);
            Player.SetState(new Fp_IdleState(Player));
        }
    }

    public override void Move(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Player.SetState(new Fp_CrouchState(Player));
        }

        Player.GetData.cameraController.GetData.headBobbing.OnIdle();
    }

    public override void IsGrounded(bool value)
    {
        if (value == false)
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
