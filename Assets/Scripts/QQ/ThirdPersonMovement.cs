using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : Tp_StateMachine
{

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _jumpSpeed = 3.5f;
    private float _directionY = 0.0f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    #region Variables
    [SerializeField] private float _timing = 2.0f;
    [SerializeField] private bool _isPushing = false;
    [SerializeField] private Transform _t = null;
    [SerializeField] private Vector3 _tInit = Vector3.zero;
    [SerializeField] private bool _isClimbing = false;
    [SerializeField] private bool _isFalling = false;
    #endregion Variables

    [SerializeField] private Transform camera0, camera1, camera2, camera3, camera4, camera5, camera6, camera7, camera8;

    void Start()
    {
        _camera.position = camera0.position;
    }

    void Update()
    {
        switchCamera();
        timing();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(_isClimbing == false)
        {
            Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

            if ((_controller.isGrounded == true) && (Input.GetKeyDown(KeyCode.Space)))
            {
                Debug.Log("Jump");
                _directionY = _jumpSpeed;
            }

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            

                Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;


                _directionY -= _gravity * Time.deltaTime;
                moveDir.y = _directionY;
                _controller.Move(moveDir * _speed * Time.deltaTime);
                
                if (_isPushing == true)
                {
                    _t.position = transform.position + _tInit;
                }
            }
            else
            {
                Vector3 moveDir = new Vector3(0, 0, 0);
                _directionY -= _gravity * Time.deltaTime;
                moveDir.y = _directionY;
                _controller.Move(moveDir * _speed * Time.deltaTime);
            }


            if (_controller.isGrounded == false)
            {

                _isFalling = true;
            }
            else
            {
                _isFalling = false;
            }
            return;
        }
        else
        {
            Vector3 moveDir = new Vector3(0.0f, vertical, 0.0f);
            _controller.Move(moveDir * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((Input.GetKeyDown(KeyCode.E)) && (_timing <= 0.0f))
        {
            if (other.tag == "Push")
            {
                Debug.Log("Push");
                if (_isPushing == false)
                {
                    _timing = 2.0f;
                    _t = other.gameObject.transform;
                    _tInit = _t.position - transform.position;
                    _isPushing = true;
                    //State.IsPushing(_isPushing);
                }
                else
                {
                    _timing = 2.0f;
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
                    _timing = 2.0f;
                    _isClimbing = true;
                    //State.IsPushing(_isClimbing);
                }
                else
                {
                    _timing = 2.0f;
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

    void timing()
    {
        if (_timing > 0.0f)
        {
            _timing -= Time.deltaTime;
        }
    }

    void switchCamera()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("C");
            cameraDecCounter();
        }

        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            Debug.Log("V");
            cameraIncCounter();
        }
    }

    void cameraIncCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }

    void cameraDecCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter--;
        cameraPositionChange(cameraPositionCounter);
    }

    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 8)
        {
            camPosition = 0;
        }
        else if (camPosition < 0)
        {
            camPosition = 8;
        }


        PlayerPrefs.SetInt("CameraPosition", camPosition);

        #region Switch Cam
        if (camPosition == 0)
        {
            _camera.position = camera0.position;
        }

        if (camPosition == 1)
        {
            _camera.position = camera1.position;
        }

        if (camPosition == 2)
        {
            _camera.position = camera2.position;
        }

        if (camPosition == 3)
        {
            _camera.position = camera3.position;
        }

        if (camPosition == 4)
        {
            _camera.position = camera4.position;
        }

        if (camPosition == 5)
        {
            _camera.position = camera5.position;
        }

        if (camPosition == 6)
        {
            _camera.position = camera6.position;
        }

        if (camPosition == 7)
        {
            _camera.position = camera7.position;
        }

        if (camPosition == 8)
        {
            _camera.position = camera8.position;
        }
        #endregion
    }
}
