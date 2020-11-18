using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : Tp_StateMachine
{

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    #region Variables
    [SerializeField] private bool _isPushing = false;
    [SerializeField] private Transform _t = null;
    [SerializeField] private Vector3 _tInit = Vector3.zero;
    [SerializeField] private bool _isClimbing = false;
    [SerializeField] private bool _isFalling = false;
    #endregion Variables

    [SerializeField] private GameObject camera0, camera1, camera2, camera3, camera4, camera5, camera6, camera7, camera8;
    AudioListener camera0AudioLis, camera1AudioLis, camera2AudioLis, camera3AudioLis, camera4AudioLis, camera5AudioLis, camera6AudioLis, camera7AudioLis, camera8AudioLis;

    void Start()
    {
        camera0AudioLis = camera0.GetComponent<AudioListener>();
        camera1AudioLis = camera1.GetComponent<AudioListener>();
        camera2AudioLis = camera2.GetComponent<AudioListener>();
        camera3AudioLis = camera3.GetComponent<AudioListener>();
        camera4AudioLis = camera4.GetComponent<AudioListener>();
        camera5AudioLis = camera5.GetComponent<AudioListener>();
        camera6AudioLis = camera6.GetComponent<AudioListener>();
        camera7AudioLis = camera7.GetComponent<AudioListener>();
        camera8AudioLis = camera8.GetComponent<AudioListener>();

        camera1AudioLis.enabled = false;
        camera1.SetActive(false);
        camera2AudioLis.enabled = false;
        camera2.SetActive(false);
        camera3AudioLis.enabled = false;
        camera3.SetActive(false);
        camera4AudioLis.enabled = false;
        camera4.SetActive(false);
        camera5AudioLis.enabled = false;
        camera5.SetActive(false);
        camera6AudioLis.enabled = false;
        camera6.SetActive(false);
        camera7AudioLis.enabled = false;
        camera7.SetActive(false);
        camera8AudioLis.enabled = false;
        camera8.SetActive(false);

        //cameraPositionChange(PlayerPrefs.GetInt("CameraPosition"));
    }

    void Update()
    {
        switchCamera();

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
                moveDir.y -= _gravity * Time.deltaTime;
                _controller.Move(moveDir.normalized * _speed * Time.deltaTime);

                if (_isPushing == true)
                {
                    _t.position = transform.position + _tInit;
                }
            }
            else
            {
                Vector3 moveDir = new Vector3(0, 0, 0);
                moveDir.y -= _gravity * Time.deltaTime;
                _controller.Move(moveDir.normalized * _speed * Time.deltaTime);
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
            _controller.Move(moveDir.normalized * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.tag == "Push")
            {
                Debug.Log("Push");
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
                Debug.Log("Climb");
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

    public void cameraPositonM()
    {
        cameraIncCounter();
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
            camera0.SetActive(true);
            camera0AudioLis.enabled = true;
            _camera = camera0.transform;

            camera8AudioLis.enabled = false;
            camera8.SetActive(false);

            camera1AudioLis.enabled = false;
            camera1.SetActive(false);
        }

        if (camPosition == 1)
        {
            camera1.SetActive(true);
            camera1AudioLis.enabled = true;
            _camera = camera1.transform;

            camera0AudioLis.enabled = false;
            camera0.SetActive(false);

            camera2AudioLis.enabled = false;
            camera2.SetActive(false);
        }

        if (camPosition == 2)
        {
            camera2.SetActive(true);
            camera2AudioLis.enabled = true;
            _camera = camera2.transform;

            camera1AudioLis.enabled = false;
            camera1.SetActive(false);

            camera3AudioLis.enabled = false;
            camera3.SetActive(false);
        }

        if (camPosition == 3)
        {
            camera3.SetActive(true);
            camera3AudioLis.enabled = true;
            _camera = camera3.transform;

            camera2AudioLis.enabled = false;
            camera2.SetActive(false);

            camera4AudioLis.enabled = false;
            camera4.SetActive(false);
        }

        if (camPosition == 4)
        {
            camera4.SetActive(true);
            camera4AudioLis.enabled = true;
            _camera = camera4.transform;

            camera3AudioLis.enabled = false;
            camera3.SetActive(false);

            camera5AudioLis.enabled = false;
            camera5.SetActive(false);
        }

        if (camPosition == 5)
        {
            camera5.SetActive(true);
            camera5AudioLis.enabled = true;
            _camera = camera5.transform;

            camera4AudioLis.enabled = false;
            camera4.SetActive(false);

            camera6AudioLis.enabled = false;
            camera6.SetActive(false);
        }

        if (camPosition == 6)
        {
            camera6.SetActive(true);
            camera6AudioLis.enabled = true;
            _camera = camera6.transform;

            camera5AudioLis.enabled = false;
            camera5.SetActive(false);

            camera7AudioLis.enabled = false;
            camera7.SetActive(false);
        }

        if (camPosition == 7)
        {
            camera7.SetActive(true);
            camera7AudioLis.enabled = true;
            _camera = camera7.transform;

            camera6AudioLis.enabled = false;
            camera6.SetActive(false);

            camera8AudioLis.enabled = false;
            camera8.SetActive(false);
        }

        if (camPosition == 8)
        {
            camera8.SetActive(true);
            camera8AudioLis.enabled = true;
            _camera = camera8.transform;

            camera7AudioLis.enabled = false;
            camera7.SetActive(false);

            camera0AudioLis.enabled = false;
            camera0.SetActive(false);
        }
        #endregion
    }
}
