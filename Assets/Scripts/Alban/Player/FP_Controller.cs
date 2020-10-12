using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private FP_Interactions _interactions = null;

    private GameObject _currentObjectItem = null;
    private E_PlayerState _currentState = E_PlayerState.IDLE;
    private Dictionary<E_PlayerState, IPlayerState> _states = null;
    private bool _isGrounded = false;
    private bool _handFull = false;
    private Pickable _pickable = null;
    private InventoryUI _inventoryUI = null;

    #region Properties
    public MovementData GetMovementData { get { return _movementData; } }
    public Data GetData { get { return _data; } }
    public Pickable GetPickable { get { return _pickable; } }
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

        if(_interactions == null)
        {
            _interactions = this.GetComponent<FP_Interactions>();

            if(_interactions != null)
            {
                _interactions.InitInteraction(this, _inventory);
            }
        }
        else
        {
            _interactions.InitInteraction(this, _inventory);
        }

        _inventory.InitInventory(this);
        //_inventory.OpenInventoryUI();

        InitDictionnary();

        ChangeState(_currentState);

        GameLoopManager.Instance.UpdatePlayer += Tick;
        InputManager.Instance.OnInteract += OnInteract;
        InputManager.Instance.UpdateCrouch += CheckCrouch;
        CheckCrouch(InputManager.Instance.GetIsCrouch);
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
        if(_handFull == false)
        {
            Pickable pickable = _data.cameraController.Interactive().GetComponent<Pickable>();

            _pickable = pickable;
        }

        _interactions.Interact(_data.cameraController.Interactive() , _handFull);
    }

    private void OnAddRemoveFromInventory()
    {
        if (_inventory.GetPlayerItems().Count > 0 && _inventory.GetPlayerItems()[0] != null)
        {
            CreateObjectInstance(_interactions.CreateObjectItem());
        }
        else
        {
            if(_pickable == null)
            {
                _interactions.AddToInventory(_data.cameraController.Pickable(), _data.cameraController.Pickable().GetItem());
            }
            else
            {
                _interactions.AddToInventory(_pickable, _pickable.GetItem());
            }
        }
    }

    private void LookObject()
    {
        _pickable = _data.cameraController.Pickable();

        _interactions.LookAt(_pickable);
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