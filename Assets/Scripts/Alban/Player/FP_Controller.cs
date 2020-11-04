using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FP_Controller : StateMachine
{
    [SerializeField] private D_FpController _movementData = null;
    [SerializeField] private Data _data = new Data();
    [Space]
    [SerializeField] private Inventory _inventory = null;
    [Space]
    [SerializeField] private LayerMask _groundLayer = 0;
    [SerializeField] private float _groundCheckDist = 0.05f;

    #region Variables
    private bool _isCrouch = false;
    private bool _stopEveryMovement = false;
    private bool _isGrounded = false;
    private bool _isLookingAt = false;
    private Pickable _pickable = null;
    private GlassWindow _glass = null;
    private Transform _lookable = null;
    private Plate _plate = null;
    private CharacterController _controller = null;
    #endregion Variables

    #region Structs
    [System.Serializable]
    public struct Data
    {
        public FP_CameraController cameraController;
        public Vector3 crouchSize;
        public Vector3 standingSize;
    }
    #endregion Structs

    #region Properties
    public CharacterController Controller { get { return _controller; } }
    public D_FpController MovementData { get { return _movementData; } }
    public Inventory GetInventory { get { return _inventory; } }
    public bool SetIsLookingAt
    {
        set
        {
            _isLookingAt = value;

            if (_onLookAt != null)
                _onLookAt(value);
        }
    }
    public bool SetIsCrouch
    {
        set
        {
            _isCrouch = value;

            if (_crouch != null)
                _crouch(value);
        }
    }
    public Data GetData { get { return _data; } }
    public Transform GetLookable { get { return _lookable; } }
    public bool SetStopEveryMovement
    {
        set
        {
            _stopEveryMovement = value;

            StopEveryMovement(value);

            if (_onStopEveryMovement != null)
                _onStopEveryMovement(value);
        }
    }
    #endregion Properties 

    #region Events
    private event Action<bool> _crouch = null;
    public event Action<bool> Crouch
    {
        add
        {
            _crouch -= value;
            _crouch += value;
        }
        remove
        {
            _crouch -= value;
        }
    }

    private event Action<bool> _onLookAt = null;
    public event Action<bool> OnLookAt
    {
        add
        {
            _onLookAt -= value;
            _onLookAt += value;
        }
        remove
        {
            _onLookAt -= value;
        }
    }

    private event Action<bool> _onStopEveryMovement = null;
    public event  Action<bool> OnStopEveryMovement
    {
        add
        {
            _onStopEveryMovement -= value;
            _onStopEveryMovement += value;
        }
        remove
        {
            _onStopEveryMovement -= value;
        }
    }
    #endregion Events

    private void Awake()
    {
        _controller = this.GetComponent<CharacterController>();

        transform.localScale = _data.standingSize;

        PlayerManager.Instance.SetPlayer = this;

        SetState(new Fp_IdleState(this));

        _isGrounded = IsGrounded();

        if (_isGrounded == false)
        {
            SetState(new Fp_FallState(this));
        }
        else
        {
            SetState(new Fp_IdleState(this));
        }

        InputManager.Instance.UpdateDirection += Direction;
        InputManager.Instance.UpdateCrouch += IsCrouch;
        InputManager.Instance.OnLookAt += LookAt;
        InputManager.Instance.OnInteract += Interact;
        InputManager.Instance.OnAddToInventory += AddToInventory;
        InputManager.Instance.OnRemoveFromInventory += RemoveFromInventory;
    }

    private void Direction(Vector3 dir)
    {
        if (_isGrounded != IsGrounded())
        {
            _isGrounded = IsGrounded();

            if (_isGrounded == false)
            {
                SetState(new Fp_FallState(this));
            }
            else
            {
                SetState(new Fp_IdleState(this));
            }
        }

        State.Move(dir);
    }

    private void IsCrouch(bool value)
    {
        SetIsCrouch = value;

        State.IsCrouch(value);
    }
    
    private bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, -transform.up, _groundCheckDist);

        return isGrounded;
    }

    private void StopEveryMovement(bool value)
    {
        if(value == true)
        {
            InputManager.Instance.UpdateDirection -= Direction;
            InputManager.Instance.UpdateCrouch -= IsCrouch;
            InputManager.Instance.OnLookAt -= LookAt;
            InputManager.Instance.OnInteract -= Interact;
            InputManager.Instance.OnAddToInventory -= AddToInventory;
            InputManager.Instance.OnRemoveFromInventory -= RemoveFromInventory;
        }
        else
        {
            InputManager.Instance.UpdateDirection += Direction;
            InputManager.Instance.UpdateCrouch += IsCrouch;
            InputManager.Instance.OnLookAt += LookAt;
            InputManager.Instance.OnInteract += Interact;
            InputManager.Instance.OnAddToInventory += AddToInventory;
            InputManager.Instance.OnRemoveFromInventory += RemoveFromInventory;
        }
    }

    private void Interact()
    {
        _data.cameraController.SetIsInteracting = false;

        Transform t = _data.cameraController.Interactive();

        if (t == null)
            return;

        if(t.TryGetComponent(out Pickable pickable))
        {
            pickable.Enter(_data.cameraController.GetData.camera.transform);

            _pickable = pickable;

            InputManager.Instance.OnInteract += Drop;
            InputManager.Instance.OnInteract -= Interact;
            
            return;
        }

        if(t.TryGetComponent(out GlassWindow glass))
        {
            glass.Enter();

            SetStopEveryMovement = true;

            _glass = glass;

            InputManager.Instance.OnInteract += LeaveGlass;
            InputManager.Instance.OnInteract -= Interact;

            return;
        }

        if(t.TryGetComponent(out Plate plate))
        {
            plate.Enter();

            SetStopEveryMovement = true;

            _plate = plate;

            InputManager.Instance.OnInteract += LeavePlate;
            InputManager.Instance.OnInteract -= Interact;

            return;
        }

        if(t.TryGetComponent(out IInteractive interactive))
        {
            interactive.Enter();

            _data.cameraController.SetIsInteracting = false;
        }
    }

    public void Drop()
    {
        _pickable?.Exit();

        _pickable = null;

        _data.cameraController.SetIsInteracting = false;

        InputManager.Instance.OnInteract += Interact;
        InputManager.Instance.OnInteract -= Drop;
    }

    private void LeaveGlass()
    {
        _glass.Exit();

        SetStopEveryMovement = false;

        _data.cameraController.SetIsInteracting = false;

        _glass = null;

        InputManager.Instance.OnInteract += Interact;
        InputManager.Instance.OnInteract -= LeaveGlass;
    }

    private void LeavePlate()
    {
        _plate.Exit();

        SetStopEveryMovement = false;

        _data.cameraController.SetIsInteracting = false;

        _plate = null;

        InputManager.Instance.OnInteract += Interact;
        InputManager.Instance.OnInteract -= LeavePlate;
    }

    private void LookAt()
    {
        Transform t = _data.cameraController.Interactive();

        if (t == null)
            return;

        if(t.TryGetComponent(out Lookable lookable))
        {
            lookable.Enter();
            _lookable = lookable.transform;
        }

        SetIsLookingAt = !_isLookingAt;
    }

    private void AddToInventory()
    {
        Transform t = _data.cameraController.Interactive();

        if (t == null)
            return;

        if (t.TryGetComponent(out Pickable pickable))
        {
            _inventory.AddItem(pickable.GetItem(), 1);

            Destroy(pickable.gameObject);
        }

        _data.cameraController.SetIsInteracting = false;
    }
    
    private void RemoveFromInventory()
    {
        InventoryItem item = _inventory.GetPlayerItems()[0];

        if (item != null)
        {
            CreateObjectInstance(item.GetObjectItem(), _data.cameraController.GetData.hand);

            _inventory.RemoveItem(item, 1);

            _data.cameraController.SetIsInteracting = false;
        }
    }

    public GameObject CreateObjectInstance(ObjectItem objectItem, Transform anchor)
    {
        return Instantiate(objectItem.GetPrefab(), anchor.position, objectItem.GetLocalRotation());
    }

    private void OnDestroy()
    {
        _inventory.GetPlayerItems().Clear();
        _inventory.GetAllItemsMap().Clear();
    }
}