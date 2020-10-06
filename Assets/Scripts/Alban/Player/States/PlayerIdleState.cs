using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IPlayerState
{
    private PlayerController _self = null;
    private CharacterController _selfController = null;

    void IPlayerState.Enter()
    {
        InputManager.Instance.UpdateDirection += Move;
        CheckGround(_self.GetIsGrounded);
        _self.UpdateIsGrounded += CheckGround;
    }

    void IPlayerState.Exit()
    {
        _self.UpdateIsGrounded -= CheckGround;
        InputManager.Instance.UpdateDirection -= Move;
    }

    private void CheckGround(bool isGrounded)
    {
        if(isGrounded == false)
        {
            InputManager.Instance.UpdateDirection -= Move;
            _self.ChangeState(E_PlayerState.INAIR);
        }
    }

    void IPlayerState.Init(PlayerController self)
    {
        _self = self;
        _selfController = self.GetData.controller;
    }

    private void Move(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            _self.ChangeState(E_PlayerState.WALKING);
        }
    }

    void IPlayerState.Tick()
    {
        _self.GetData.cameraController.GetData.headBobbing.OnIdle();
    }
}