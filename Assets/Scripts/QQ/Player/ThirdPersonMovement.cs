using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : Tp_StateMachine
{

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    #region Variables
    [SerializeField] private bool _isPushing = false;
    [SerializeField] private Transform _t = null;
    [SerializeField] private Vector3 _tInit = Vector3.zero;
    [SerializeField] private bool _isClimbing = false;
    #endregion Variables

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(_isClimbing == false)
        {
            Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            

                Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                _controller.Move(moveDir.normalized * _speed * Time.deltaTime);

                if (_isPushing == true)
                {
                    _t.position = transform.position + _tInit;
                }
            }
            return;
        }
        else
        {
            Vector3 moveDir = new Vector3(0.0f, vertical, 0.0f);
            _controller.Move(moveDir.normalized * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.tag == "Push")
            {
                if (_isPushing == false)
                {
                    _t = other.gameObject.transform;
                    _tInit = _t.position - transform.position;
                    _isPushing = true;
                    //State.IsPushing(_isPushing);
                }
                else
                {
                    _t = null;
                    _tInit = Vector3.zero;
                    _isPushing = false;
                    //State.IsPushing(_isPushing);
                }
                return;
            }
            else if (other.tag == "Climb")
            {
                if (_isClimbing == false)
                {

                    _isClimbing = true;
                    //State.IsPushing(_isClimbing);
                }
                else
                {

                    _isClimbing = false;
                    //State.IsPushing(_isClimbing);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climb")
        {
            _isClimbing = false;
        }
    }
}
