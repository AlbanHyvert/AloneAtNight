using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(CharacterController))]
public class FP_Controller : MonoBehaviour
{
    [SerializeField] private MovementData _movementData;
    [SerializeField] private Data _data;
    [Space]
    [SerializeField] private E_PlayerState _startingState = E_PlayerState.IDLE;
    [Space]
    [SerializeField] private Transform _objectAnchor = null;
    [SerializeField] private Inventory _inventory = null;

    private GameObject _currentObjectItem = null;
    private E_PlayerState _currentState = E_PlayerState.IDLE;
    private Dictionary<E_PlayerState, IPlayerState> _states = null;
    private bool _isGrounded = false;
    private bool _isLookAt = false;
    private bool _handFull = false;
    private Pickable _pickable = null;
    private InventoryUI _inventoryUI = null;

    #region Properties
    public MovementData GetMovementData { get { return _movementData; } }
    public Data GetData { get { return _data; } }
    public Pickable GetPickable { get { return _pickable; } }
    public bool GetIsLookAt { get { return _isLookAt; } }
    public bool GetHandFull { get { return _handFull; } }
    public bool SetHandFull
    {
        set
        {
            _handFull = value;
            if (_handFull == false && _pickable != null)
            {
                _pickable.GetComponent<IInteractive>().Exit();
                _pickable = null;
            }
        } 
    }
    public bool GetIsGrounded { get { return _isGrounded; } }
    public bool SetIsGrounded
    {
        set
        {
            _isGrounded = value;
            if (_updateIsGrounded != null)
                _updateIsGrounded(_isGrounded);
        }
    }
    #endregion Properties

    private event Action<bool> _updateIsGrounded = null;
    public event Action<bool> UpdateIsGrounded
    {
        add
        {
            _updateIsGrounded -= value;
            _updateIsGrounded += value;
        }
        remove
        {
            _updateIsGrounded -= value;
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

    #region Structs
    [System.Serializable]
    public struct MovementData
    {
        public float speed;
        public float crouchSpeed;
        public float smoothTime;
        public float fallSpeed;
        public float rotationSpeed;
        public int maxXRotation;
        public int minXRotation;
    }
    [System.Serializable]
    public struct Data
    {
        public CharacterController controller;
        public FP_CameraController cameraController;
    }
    #endregion Structs

    private void Awake()
    {
        PlayerManager.Instance.SetPlayer = this;

        if (_data.controller == null)
            _data.controller = this.GetComponent<CharacterController>();
    }

    private void Start()
    {
        _states = new Dictionary<E_PlayerState, IPlayerState>();

        _inventory.InitInventory(this);
        //_inventory.OpenInventoryUI();

        InitDictionnary();

        ChangeState(_currentState);

        GameLoopManager.Instance.UpdatePlayer += Tick;
        InputManager.Instance.OnInteract += OnInteract;
        InputManager.Instance.OnInventory += AddOrRemoveToInventory;
        InputManager.Instance.UpdateCrouch += CheckCrouch;
        CheckCrouch(InputManager.Instance.GetIsCrouch);

        _data.cameraController.UpdateIsLookable += IsLookable;
    }

    private void IsLookable(bool value)
    {
        if(value == true)
        {
            InputManager.Instance.OnLookAt += LookAt;
        }
        else
        {
            InputManager.Instance.OnLookAt -= LookAt;
        }
    }

    private void InitDictionnary()
    {
        _states.Add(E_PlayerState.IDLE, new FP_IdleState());
        _states.Add(E_PlayerState.INAIR, new FP_InAirState());
        _states.Add(E_PlayerState.TELEPORT, new FP_TeleportState());
        _states.Add(E_PlayerState.DEAD, new FP_DeadState());
        _states.Add(E_PlayerState.WALKING, new FP_WalkingState());
        _states.Add(E_PlayerState.CROUCHING, new FP_CrouchingState());

        _states[E_PlayerState.IDLE].Init(this);
        _states[E_PlayerState.INAIR].Init(this);
        _states[E_PlayerState.DEAD].Init(this);
        _states[E_PlayerState.TELEPORT].Init(this);
        _states[E_PlayerState.WALKING].Init(this);
        _states[E_PlayerState.CROUCHING].Init(this);

        _currentState = _startingState;
    }

    private void CheckCrouch(bool value)
    {
        if(value == true)
        {
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void CreateObjectInstance(ObjectItem objectItem)
    {
        DestroyIfNotNull(_currentObjectItem);
        _currentObjectItem = CreateNewItemInstance(objectItem, _objectAnchor);

        _inventory.RemoveItem(objectItem, 1);

        _currentObjectItem.transform.SetParent(null);
    }

    private void DestroyIfNotNull(GameObject obj)
    {
        if(obj)
        {
            Destroy(obj);
        }
    }

    private GameObject CreateNewItemInstance(ObjectItem objectItem, Transform objectAnchor)
    {
        GameObject itemInstance = Instantiate(objectItem.GetPrefab(), objectAnchor);

        itemInstance.transform.localPosition = objectItem.GetLocalPosition();
        itemInstance.transform.localRotation = objectItem.GetLocalRotation();

        return itemInstance;
    }

    private void Tick()
    {
        if(_data.controller.isGrounded == RaycastGround())
        {
            if (_data.controller.isGrounded != _isGrounded)
                SetIsGrounded = _data.controller.isGrounded;
        }

        _states[_currentState].Tick();
    }

    public void ChangeState(E_PlayerState nextState)
    {
        _states[_currentState].Exit();

        _currentState = nextState;

        _states[nextState].Enter();
    }

    private void OnInteract()
    {
        Transform interactable = _data.cameraController.Interactive();
        Pickable pickable = null;
        IInteractive interactive = null;

        if(_handFull == false)
        {
            if (interactable != null)
            {
                pickable = interactable.GetComponent<Pickable>();

                if (pickable == null)
                {
                    interactive = interactable.GetComponent<IInteractive>();

                    interactive.Enter();

                    _data.cameraController.SetIsInteracting = false;
                }
                else
                {
                    _pickable = pickable;

                    _pickable.GetComponent<IInteractive>().Enter(_data.cameraController.GetData.camera.transform);

                    _handFull = true;
                }
            }
        }
        else
        {
            _pickable.GetComponent<IInteractive>().Exit();
            _data.cameraController.SetIsInteracting = false;
            SetHandFull = false;
        }
    }

    private void LookAt()
    {
        Transform t = _data.cameraController.Interactive();
        
        if(t != null)
            _pickable = t.GetComponent<Pickable>();

        if(_pickable != null)
        {
            _isLookAt = !_isLookAt;

            if (_isLookAt == true)
            {
                _pickable.GetComponent<IInteractive>().Enter();
                _pickable.transform.position = _objectAnchor.position;
            }
            else
            {
                _pickable.GetComponent<IInteractive>().Exit();
            }

            if(_onLookAt != null)
            {
                _onLookAt(_isLookAt);
            }
        }
    }

    private void AddOrRemoveToInventory()
    {
        if (_inventory.GetPlayerItems().Count > 0 && _inventory.GetPlayerItems()[0] != null)
        {
            ObjectItem objectItem = _inventory.GetPlayerItems()[0].GetObjectItem();

            CreateObjectInstance(objectItem);
            _inventory.RemoveItem(objectItem, 1);
            _inventory.GetPlayerItems().Remove(objectItem);
        }
        else
        {
            if (_handFull == true)
            {
                _pickable.GetComponent<IInteractive>().Exit();
                _inventory.AddItem(_pickable.GetItem(), 1);

                _handFull = false;
                _data.cameraController.SetIsInteracting = false;

                _isLookAt = false;
                _onLookAt(_isLookAt);

                Destroy(_pickable.gameObject);
            }
            else
            {
                Transform t = _data.cameraController.Interactive();

                if(t != null)
                    _pickable = t.GetComponent<Pickable>();

                if (_pickable != null)
                {
                    _inventory.AddItem(_pickable.GetItem(), 1);

                    _handFull = false;
                    _data.cameraController.SetIsInteracting = false;

                    _isLookAt = false;
                    _onLookAt(_isLookAt);

                    Destroy(_pickable.gameObject);
                }
            }
        }

        _pickable = null;
    }

    private bool RaycastGround()
    {
        bool isGrounded = Physics.Raycast(transform.position, -transform.up, 0.5f);

        return isGrounded;
    }

    public InventoryUI GetInventoryUI()
    {
        return _inventoryUI;
    }
}