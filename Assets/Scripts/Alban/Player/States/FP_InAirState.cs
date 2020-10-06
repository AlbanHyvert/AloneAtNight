using UnityEngine;

public class FP_InAirState : MonoBehaviour, IPlayerState
{
    private FP_Controller _self = null;
    private CharacterController _selfController = null;
    private float _fallSpeed = 1;
    private Vector3 _lastDir = -Vector3.up;

    void IPlayerState.Enter()
    {
        InputManager.Instance.UpdateDirection += Move;

        CheckGround(_selfController.isGrounded);
        _self.UpdateIsGrounded += CheckGround;

        _lastDir = -Vector3.up;
    }

    void IPlayerState.Exit()
    {
        _lastDir = -Vector3.up;
        _self.UpdateIsGrounded -= CheckGround;
        InputManager.Instance.UpdateDirection -= Move;
    }

    private void CheckGround(bool isGrounded)
    {
        if(isGrounded == true)
        {
            _self.ChangeState(E_PlayerState.IDLE);
        }
    }

    void IPlayerState.Init(FP_Controller self)
    {
        _self = self;
        _selfController = self.GetData.controller;
        _fallSpeed = _self.GetMovementData.fallSpeed;
    }

    private void Move(Vector3 dir)
    {
        _lastDir += dir;

        InputManager.Instance.UpdateDirection -= Move;
    }

    void IPlayerState.Tick()
    {
        _self.GetData.cameraController.GetData.headBobbing.OnIdle();

        _selfController.Move(_lastDir * (_fallSpeed * Time.deltaTime));
    }
}
