using UnityEngine;

public class Fp_FallState : State
{
    public Fp_FallState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    {
        Player.SetIsCrouch = false;
        Player.transform.localScale = Player.GetData.standingSize;
    }

    public override void Move(Vector3 dir)
    {
        dir = -Vector3.up;

        float fallSpeed = Player.MovementData.FallSpeed;

        Player.Controller.Move(dir * fallSpeed * Time.deltaTime);
    }
}