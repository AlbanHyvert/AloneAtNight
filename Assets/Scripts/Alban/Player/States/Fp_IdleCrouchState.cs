using UnityEngine;

public class Fp_IdleCrouchState : State
{
    public Fp_IdleCrouchState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    {
        Player.transform.localScale = Player.GetData.crouchSize;
    }

    public override void IsCrouch(bool value)
    {
        if (value == false)
        {
            Player.SetState(new Fp_IdleState(Player));
        }
    }

    public override void Move(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Player.SetState(new Fp_CrouchState(Player));
        }

        Player.GetData.cameraController.GetData.headBobbing.OnWalk();
    }
}
