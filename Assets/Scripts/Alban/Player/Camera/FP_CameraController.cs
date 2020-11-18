﻿using cakeslice;
using UnityEngine;

public class FP_CameraController : MonoBehaviour
{
    [SerializeField] private Data _data = new Data();
    [SerializeField] private D_FpCamera _cameraData = null;
    [Space]
    [SerializeField] private LayerMask _interactables = 0;
    [SerializeField] private LayerMask _checkLayer = 0;
    [SerializeField] private float _maxInteractbleDistance = 10;
    [SerializeField] private OutlineEffect _outlineEffect = null;

    #region Variables
    private Vector3 _defaultPos = Vector3.zero;
    private float _currentX = 0;
    private float _currentY = 0;
    private float _rotationX = 0;
    private float _rotationY = 0;
    private float _currentInteractbleDist = 0;
    private FP_Controller _fpPlayer = null;
    private bool _canInteract = false;
    private bool _isInteracting = false;
    #endregion Variables

    public bool SetIsInteracting
    { 
        set
        {
            _isInteracting = value;
            
            if(value == false)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public Data GetData { get { return _data; } }
    public D_FpCamera GetMovementData { get { return _cameraData; } }

    #region Structs
    [System.Serializable]
    public struct Data
    {
        public Camera camera;
        public HeadBobbing headBobbing;
        public Transform body;
        public Transform hand;
    }
    #endregion Structs

    public void Init(FP_Controller player)
    {
        _defaultPos = transform.localPosition;

        _canInteract = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _currentInteractbleDist = _maxInteractbleDistance;

        GameLoopManager.Instance.UpdatePause += IsPaused;
        InputManager.Instance.UpdateMousePos += CameraRotation;

        _fpPlayer = player;
        
        _outlineEffect.ClearOutline();
        _outlineEffect.LineIntensity = 0;

        _fpPlayer.OnLookAt += IsLookingAt;
        _fpPlayer.OnStopEveryMovement += StopCamera;
    }

    private void IsPaused(bool value)
    {
        if(value == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            InputManager.Instance.UpdateMousePos -= CameraRotation;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            InputManager.Instance.UpdateMousePos += CameraRotation;
        }
    }

    private void StopCamera(bool value)
    {
        if(value == true)
        {
            InputManager.Instance.UpdateMousePos -= CameraRotation;
            InputManager.Instance.UpdateMousePos += MoveObjectAround;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _data.camera.gameObject.SetActive(false);
        }
        else
        {
            _data.camera.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            InputManager.Instance.UpdateMousePos -= MoveObjectAround;
            InputManager.Instance.UpdateMousePos += CameraRotation;
        }
    }

    private void IsLookingAt(bool value)
    {
        if(value == true)
        {
            InputManager.Instance.UpdateMousePos -= CameraRotation;
            InputManager.Instance.UpdateMousePos += RotateObject;
        }
        else
        {
            InputManager.Instance.UpdateMousePos -= RotateObject;
            InputManager.Instance.UpdateMousePos += CameraRotation;
        }
    }

    private void IsCrouch(bool value)
    {
        if(value == false)
        {
            transform.localPosition = _defaultPos;
            _data.headBobbing.CurrentPosY = _data.headBobbing.DefaultPosY;
        }
        else
        {
            transform.localPosition = new Vector3(0, 0.52f, 0.129f);
        }
    }

    private void MoveObjectAround()
    {

    }

    private void RotateObject()
    {
        float rotX = Input.GetAxis("Mouse X") * (float)_cameraData.RotationSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * (float)_cameraData.RotationSpeed * Mathf.Deg2Rad;

        _fpPlayer.GetLookable.Rotate(Vector3.up, -rotX);
        _fpPlayer.GetLookable.Rotate(Vector3.right, rotY);

        //CheckInteractable();

        Vector3 rotation = _fpPlayer.GetLookable.eulerAngles;
    }

    private void CameraRotation()
    {
        _rotationX = _data.body.localEulerAngles.y;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= (float)_cameraData.RotationSpeed;
        mouseY *= (float)_cameraData.RotationSpeed;

        CheckInteractable();

        _currentX = Mathf.Lerp(_currentX, mouseX, (float)_cameraData.SmoothTime * Time.deltaTime);
        _currentY = Mathf.Lerp(_currentY, mouseY, (float)_cameraData.SmoothTime * Time.deltaTime);

        _rotationX = _data.body.localEulerAngles.y + _currentX * (float)_cameraData.RotationSpeed;

        _rotationY += _currentY * (float)_cameraData.RotationSpeed;
        _rotationY = Mathf.Clamp(_rotationY, _cameraData.MinXRotation, _cameraData.MaxXRotation);

        _data.body.rotation = Quaternion.Euler(new Vector3(0, _rotationX, 0));
        _data.camera.transform.localRotation = Quaternion.Euler(-_rotationY, 0, 0);

        _rotationX = _data.body.localEulerAngles.y;
    }

    private void CheckInteractable()
    {
        RaycastHit hit;

        bool checkHit = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _currentInteractbleDist, _checkLayer);

        if(checkHit == true)
        {
            float diffBetweenValue = _currentInteractbleDist - hit.distance;

            if(diffBetweenValue > 0.4f)
            {
                _currentInteractbleDist = hit.distance;
            }
        }
        else
        {
            _currentInteractbleDist = _maxInteractbleDistance;
        }

        bool isInteractable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _currentInteractbleDist, _interactables);

        if (isInteractable != _canInteract)
        {
            _canInteract = isInteractable;

            if(isInteractable == true)
            {
                _currentInteractbleDist = hit.distance;

                Transform t = hit.transform;

                if (t.TryGetComponent(out Outline outline))
                {
                    outline.ActivateOutline();

                    _outlineEffect.LineIntensity = 2;
                }

                if (t.TryGetComponent(out Pickable pickable))
                {
                    _fpPlayer.CursorUI.SetPointerToOpenHand();

                    return;
                }

                if (t.TryGetComponent(out GlassWindow glass))
                {
                    _fpPlayer.CursorUI.SetPointerToEye();
                    
                    return;
                }

                if (t.TryGetComponent(out Plate plate))
                {
                    _fpPlayer.CursorUI.SetPointerToEye();

                    return;
                }

                if (t.TryGetComponent(out IInteractive interactive))
                {
                    _fpPlayer.CursorUI.SetPointerToOpenHand();

                    return;
                }
            }
            else
            {
                _outlineEffect.ClearOutline();
                _outlineEffect.LineIntensity = 0;

                _fpPlayer.CursorUI.SetPointerToDefault();
            }
        }
    }
    
    public Transform Interactive()
    {
        Transform interactive = null;
        RaycastHit hit;

        bool isInteractable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _maxInteractbleDistance, _interactables);

        if (isInteractable != _isInteracting)
        {
            _isInteracting = isInteractable;

            if (isInteractable == true)
            {
                interactive = hit.transform;
            }
        }

        return interactive;
    }
}