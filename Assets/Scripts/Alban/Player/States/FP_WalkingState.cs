using UnityEngine;

public class FP_WalkingState : MonoBehaviour, IPlayerState
{
    private FP_Controller _self = null;
    private CharacterController _selfController = null;
    private float _currentSpeed = 0;

    void IPlayerState.Enter()
    {
        _self.OnLookAt += IsLookingAt;
        IsLookingAt(_self.GetIsLookAt);
        _self.OnStopEveryMovement += StopMovemevent;
        StopMovemevent(_self.GetStopEveryMovement);
        InputManager.Instance.UpdateCrouch += CheckCrouch;
        CheckCrouch(InputManager.Instance.GetIsCrouch);
        CheckGround(_self.GetIsGrounded);
        _self.UpdateIsGrounded += CheckGround;
    }

    void IPlayerState.Exit()
    {
        _self.UpdateIsGrounded -= CheckGround;
        InputManager.Instance.UpdateCrouch -= CheckCrouch;
        InputManager.Instance.UpdateDirection -= Move;
    }

    private void StopMovemevent(bool value)
    {
        if (value == true)
        {
            _self.OnLookAt -= IsLookingAt;
            InputManager.Instance.UpdateDirection -= Move;
            InputManager.Instance.UpdateCrouch -= CheckCrouch;
        }
        else
        {
            _self.OnLookAt += IsLookingAt;
            IsLookingAt(_self.GetIsLookAt);
            InputManager.Instance.UpdateCrouch += CheckCrouch;
            CheckCrouch(InputManager.Instance.GetIsCrouch);
        }
    }

    private void IsLookingAt(bool value)
    {
        if (value == true)
        {
            InputManager.Instance.UpdateDirection -= Move;
        }
        else
        {
            InputManager.Instance.UpdateDirection += Move;
        }
    }

    private void CheckGround(bool isGrounded)
    {
        if (isGrounded == false)
        {
            InputManager.Instance.UpdateDirection -= Move;
            _self.ChangeState(E_PlayerState.INAIR);
        }
    }

    private void CheckCrouch(bool value)
    {
        if(value == true)
        {
            _self.ChangeState(E_PlayerState.CROUCHING);
        }
    }

    void IPlayerState.Init(FP_Controller self)
    {
        _self = self;
        _selfController = self.GetData.controller;
    }

    private void Move(Vector3 dir)
    {
        if(dir == Vector3.zero)
        {
            _self.ChangeState(E_PlayerState.IDLE);
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed, _self.GetMovementData.speed, _self.GetMovementData.smoothTime * Time.deltaTime);

        dir *= _currentSpeed * Time.deltaTime;

        _self.GetData.cameraController.GetData.headBobbing.OnWalk();

        _selfController.Move(dir);
    }

    void IPlayerState.Tick()
    {

    }
}
