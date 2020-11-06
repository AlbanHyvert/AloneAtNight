using UnityEngine;
using UnityEngine.Windows.Speech;

public class Fp_FallState : State
{
    private float _time = 0f;
    private float _checkTime = 2f;
    private Vector3 _oldPos = Vector3.zero;

    public Fp_FallState(FP_Controller player) : base(player)
    {
    }

    public override void Start()
    {
        _time = 0f;
        Player.SetIsCrouch = false;
        Player.transform.localScale = Player.GetData.standingSize;
        _oldPos = Player.transform.position;
        Player.SetCurrentState = E_PlayerState.INAIR;
    }

    public override void Move(Vector3 dir)
    {
        _time += 1 * Time.deltaTime;

        dir = Vector3.down;

        float fallSpeed = Player.MovementData.FallSpeed;

        Player.Controller.Move(dir * fallSpeed * Time.deltaTime);

        if(_time > _checkTime)
        {
            _time = 0;

            float dist = Vector3.Distance(_oldPos, Player.transform.position);

            if(dist > -0.5f && dist < 1f)
            {
                Player.SetIsGrounded = true;
            }
            else
            {
                _oldPos = Player.transform.position;
            }
        }
    }

    public override void IsGrounded(bool value)
    {
        if (value == true)
        {
            Player.SetState(new Fp_IdleState(Player));
        }
    }
}