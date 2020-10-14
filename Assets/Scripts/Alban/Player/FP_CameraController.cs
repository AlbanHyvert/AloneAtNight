using System;
using UnityEngine;

public class FP_CameraController : MonoBehaviour
{
    [SerializeField] private Data _data;
    [Space]
    [SerializeField] private LayerMask _interactables = 0;
    [SerializeField] private int _interactDist = 10;

    #region Variables
    private Vector3 _startPos = Vector3.zero;
    private MovementData _movementData;
    private float _currentX = 0;
    private float _currentY = 0;
    private float _rotationX = 0;
    private float _rotationY = 0;
    private FP_Controller _fpPlayer = null;
    private IInteractive _interactable = null;
    private bool _canInteract = false;
    private bool _isInteracting = false;
    #endregion Variables

    public bool SetIsInteracting { set { _isInteracting = value; } }
    public Data GetData { get { return _data; } }

    private event Action<bool> _updateIsLookable = null;
    public event Action<bool> UpdateIsLookable
    {
        add
        {
            _updateIsLookable -= value;
            _updateIsLookable += value;
        }
        remove
        {
            _updateIsLookable -= value;
        }
    }

    #region Structs
    [System.Serializable]
    public struct Data
    {
        public Camera camera;
        public HeadBobbing headBobbing;
        public double smoothTime;
        public Transform body;
        public Transform hand;
    }

    private struct MovementData
    {
        public double rotationSpeed;
        public int maxXRotation;
        public int minXRotation;
    }
    #endregion Structs

    private void Start()
    {
        _startPos = _data.camera.transform.localPosition;
        _movementData.rotationSpeed = PlayerManager.Instance.GetPlayer.GetMovementData.rotationSpeed;
        _movementData.maxXRotation = PlayerManager.Instance.GetPlayer.GetMovementData.maxXRotation;
        _movementData.minXRotation = PlayerManager.Instance.GetPlayer.GetMovementData.minXRotation;

        Cursor.lockState = CursorLockMode.Locked;

        InputManager.Instance.UpdateMousePos += CameraRotation;

        _fpPlayer = this.GetComponentInParent<FP_Controller>();

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
        float rotX = Input.GetAxis("Mouse X") * (float)_movementData.rotationSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * (float)_movementData.rotationSpeed * Mathf.Deg2Rad;

        _fpPlayer.GetPickable.transform.Rotate(Vector3.up, -rotX);
        _fpPlayer.GetPickable.transform.Rotate(Vector3.right, rotY);

        CheckInteractable();

        Vector3 rotation = _fpPlayer.GetPickable.transform.eulerAngles;

        Debug.Log("Rotation: " + rotation);
    }

    private void CameraRotation(Vector3 mousePos)
    {
        _rotationX = _data.body.localEulerAngles.y;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= (float)_movementData.rotationSpeed;
        mouseY *= (float)_movementData.rotationSpeed;

        CheckInteractable();

        _currentX = Mathf.Lerp(_currentX, mouseX, (float)_data.smoothTime * Time.deltaTime);
        _currentY = Mathf.Lerp(_currentY, mouseY, (float)_data.smoothTime * Time.deltaTime);

        _rotationX = _data.body.localEulerAngles.y + _currentX * (float)_movementData.rotationSpeed;

        _rotationY += _currentY * (float)_movementData.rotationSpeed;
        _rotationY = Mathf.Clamp(_rotationY, _movementData.minXRotation, _movementData.maxXRotation);

        _data.body.rotation = Quaternion.Euler(new Vector3(0, _rotationX, 0));
        _data.camera.transform.localRotation = Quaternion.Euler(-_rotationY, 0, 0);

        _rotationX = _data.body.localEulerAngles.y;
    }

    private void CheckInteractable()
    {
        RaycastHit hit;

        bool isInteractable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _interactDist, _interactables); ;

        if (isInteractable != _canInteract)
        {
            _canInteract = isInteractable;

            if (isInteractable == true)
            {
                Transform t = hit.transform;

                IInteractive interactive = t.GetComponent<IInteractive>();
                Pickable pickable = t.GetComponent<Pickable>();

                _interactable = interactive;

                if(_interactable != null)
                    _interactable.OnSeen();
            
                if(pickable != null)
                {
                    _updateIsLookable(true);
                }
            }
            else
            {
                if(_interactable != null)
                    _interactable.OnUnseen();

                _updateIsLookable(false);

                _interactable = null;
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