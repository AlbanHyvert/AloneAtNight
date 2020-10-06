using UnityEngine;

public class FP_CameraController : MonoBehaviour
{
    [SerializeField] private Data _data;
    [Space]
    [SerializeField] private LayerMask _interactables = 0;
    [SerializeField] private int _interactDist = 10;

    private Vector3 _startPos = Vector3.zero;
    private MovementData _movementData;
    private float _currentX = 0;
    private float _currentY = 0;
    private float _rotationX = 0;
    private float _rotationY = 0;

    private IInteractive _interactable = null;
    private bool _canInteract = false;
    private bool _isInteracting = false;

    public bool SetIsInteracting { set { _isInteracting = value; } }
    public Data GetData { get { return _data; } }

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

        InputManager.Instance.UpdateMousePos += Tick;
    }

    private void Tick(Vector3 mousePos)
    {
        CameraRotation();

        CheckInteractable();
    }

    private void CameraRotation()
    {
        _rotationX = _data.body.localEulerAngles.y;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= (float)_movementData.rotationSpeed;
        mouseY *= (float)_movementData.rotationSpeed;

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

        bool isPickable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _interactDist, _interactables); ;

        if (isPickable != _canInteract)
        {
            _canInteract = isPickable;

            if (isPickable == true)
            {
                _interactable = hit.transform.GetComponent<IInteractive>();
                _interactable.OnSeen();
            }
            else
            {
                _interactable.OnUnseen();
                _interactable = null;
            }
        }
    }

    public Pickable Interactable()
    {
        Pickable pickable = null;
        RaycastHit hit;

        bool isPickable = Physics.Raycast(_data.camera.transform.position, _data.camera.transform.forward, out hit, _interactDist, _interactables);

        if (isPickable != _isInteracting)
        {
            _isInteracting = isPickable;

            if(isPickable == true)
            {
                pickable = hit.transform.GetComponent<Pickable>();
            }
        }

        return pickable;
    }
}
