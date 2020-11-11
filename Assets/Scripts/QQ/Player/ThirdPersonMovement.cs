using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : StateMachine
{

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * _speed * Time.deltaTime);
        }
    }
}
