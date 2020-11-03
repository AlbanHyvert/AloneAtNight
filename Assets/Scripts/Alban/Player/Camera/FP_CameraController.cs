using cakeslice;
using System;
using UnityEngine;

public class FP_CameraController : MonoBehaviour
{
    [SerializeField] private Data _data = new Data();
    [SerializeField] private D_FpCamera _cameraData = null;
    [Space]
    [SerializeField] private LayerMask _interactables = 0;
    [SerializeField] private int _interactDist = 10;
    [SerializeField] private OutlineEffect _outlineEffect = null;
    
    #region Variables
    private float _currentX = 0;
    private float _currentY = 0;
    private float _rotationX = 0;
    private float _rotationY = 0;
    private FP_Controller _fpPlayer = null;
    private bool _canInteract = false;
    private bool _isInteracting = false;
    #endregion Variables

    public bool SetIsInteracting { set { _isInteracting = value; } }
    public Data GetData { get { return _data; } }

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

    private void Start()
    {
        _canInteract = false;

        Cursor.lockState = CursorLockMode.Locked;

        InputManager.Instance.UpdateMousePos += CameraRotation;

        _fpPlayer = this.GetComponentInParent<FP_Controller>();

        _outlineEffect.ClearOutline();
        _outlineEffect.LineIntensity = 0;

        _fpPlayer.OnLookAt += IsLookingAt;
        _fpPlayer.OnStopEveryMovement += StopCamera;
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

    private void MoveObjectAround(Vector3 mousePos)
    {

    }

    private void RotateObject(Vector3 mousePos)
    {
        float rotX = Input.GetAxis("Mouse X") * (float)_cameraData.RotationSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * (float)_cameraData.RotationSpeed * Mathf.Deg2Rad;

        _fpPlayer.GetLookable.Rotate(Vector3.up, -rotX);
        _fpPlayer.GetLookable.Rotate(Vector3.right, rotY);

        CheckInteractable();

        Vector3 rotation = _fpPlayer.GetLookable.eulerAngles;
    }

    private void CameraRotation(Vector3 mousePos)
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

        bool isInteractable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _interactDist, _interactables);

        if (isInteractable != _canInteract)
        {
            _canInteract = isInteractable;

            if(isInteractable == true)
            {
                Transform t = hit.transform;

                if(t.TryGetComponent(out Outline outline))
                {
                    outline.ActivateOutline();

                    _outlineEffect.LineIntensity = 2;
                }
            }
            else
            {
                _outlineEffect.ClearOutline();
                _outlineEffect.LineIntensity = 0;
            }
        }
    }
    
    public Transform Interactive()
    {
        Transform interactive = null;
        RaycastHit hit;

        bool isInteractable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _interactDist, _interactables);

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