using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fp_CrouchState : State
{
    public Fp_CrouchState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    {
        Player.transform.localScale = Player.GetData.crouchSize;
        Player.SetCurrentState = E_PlayerState.CROUCHING;
    }

    public override void IsCrouch(bool value)
    {
        if (value == false)
        {
            Player.SetState(new Fp_WalkState(Player));
        }
    }

    public override void Move(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            Player.SetState(new Fp_IdleCrouchState(Player));
        }

        float speed = Player.MovementData.CrouchSpeed;

        Player.GetData.cameraController.GetData.headBobbing.OnCrouch();

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
