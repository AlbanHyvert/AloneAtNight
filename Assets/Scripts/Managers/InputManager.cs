using System;
using UnityEngine;
using Engine.Singleton;


public class InputManager : Singleton<InputManager>
{
    [SerializeField] private Keyboard _keyboard;

    private Vector3 _direction = Vector3.zero;
    private Vector3 _mousePosition = Vector3.zero;
    private FP_Controller _player = null;
    private Camera _camera = null;
    private bool _isCrouch = false;

    #region Events
    private event Action<Vector3> _updateMousePos = null;
    public event Action<Vector3> UpdateMousePos
    {
        add
        {
            _updateMousePos -= value;
            _updateMousePos += value;
        }
        remove
        {
            _updateMousePos -= value;
        }
    }

    private event Action<Vector3> _updateDirection = null;
    public event Action<Vector3> UpdateDirection
    {
        add
        {
            _updateDirection -= value;
            _updateDirection += value;
        }
        remove
        {
            _updateDirection -= value;
        }
    }

    private event Action<bool> _updateCrouch = null;
    public event Action<bool> UpdateCrouch
    {
        add
        {
            _updateCrouch -= value;
            _updateCrouch += value;
        }
        remove
        {
            _updateCrouch -= value;
        }
    }

    private event Action _onInteract = null;
    public event Action OnInteract
    {
        add
        {
            _onInteract -= value;
            _onInteract += value;
        }
        remove
        {
            _onInteract -= value;
        }
    }

    private event Action _onLookAt = null;
    public event Action OnLookAt
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

    private event Action _onAddToInventory = null;
    public event Action OnAddToInventory
    {
        add
        {
            _onAddToInventory -= value;
            _onAddToInventory += value;
        }
        remove
        {
            _onAddToInventory -= value;
        }
    }

    private event Action _onRemoveFromInventory = null;
    public event Action OnRemoveFromInventory
    {
        add
        {
            _onRemoveFromInventory -= value;
            _onRemoveFromInventory += value;
        }
        remove
        {
            _onRemoveFromInventory -= value;
        }
    }

    private event Action _onThrow = null;
    public event Action OnThrow
    {
        add
        {
            _onThrow -= value;
            _onThrow += value;
        }
        remove
        {
            _onThrow -= value;
        }
    }
    #endregion Events

    public bool GetIsCrouch { get { return _isCrouch; } }

    #region Structs
    [System.Serializable]
    private struct Keyboard
    {
        public KeyCode forward;
        public KeyCode left;
        public KeyCode backward;
        public KeyCode right;
        public KeyCode crouch;
        public KeyCode interact;
        public KeyCode lookAt;
        public KeyCode launch;
    }
    #endregion Structs

    private void Start()
    {
        PlayerManager.Instance.UpdatePlayer += CheckPlayer;
        GameLoopManager.Instance.UpdatePause += GamePaused;
    }

    private void GamePaused(bool isPaused)
    {
        if(isPaused == true)
        {
            GameLoopManager.Instance.UpdateManager -= Tick;
        }
        else
        {
            GameLoopManager.Instance.UpdateManager += Tick;
        }
    }

    private void CheckPlayer(FP_Controller player)
    {
        if(player != null)
        {
            _player = player;
            _camera = player.GetData.cameraController.GetData.camera;
            GameLoopManager.Instance.UpdateManager += Tick;
        }
        else
        {
            GameLoopManager.Instance.UpdateManager -= Tick;
        }
    }

    private void Tick()
    {
        if (_updateMousePos != null)
            UpdateMousePosition();

        if (_updateDirection != null)
            UpdateMovement();

        if (_onInteract != null)
            UpdateInteract();

        if (_onThrow != null)
            UpdateThrow();

        if(_updateCrouch != null)
        {
            if (Input.GetKeyDown(_keyboard.crouch))
            {
                _isCrouch = !_isCrouch;

                _updateCrouch(_isCrouch);
            }
        }
    
        if(_onLookAt != null)
        {
            if(Input.GetKeyDown(_keyboard.lookAt))
            {
                _onLookAt();
            }
        }
    
        if(_onAddToInventory != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                _onAddToInventory();
            }
        }
        
        if(_onRemoveFromInventory != null)
        {
            if(Input.GetMouseButtonDown(1))
            {
                _onRemoveFromInventory();
            }
        }
    }

    private void UpdateMovement()
    {
        _direction = Vector3.zero;

        //FORWARD
        if (Input.GetKey(_keyboard.forward))
        {
            _direction += _player.transform.forward;
        }

        //BACKWARD
        if(Input.GetKey(_keyboard.backward))
        {
            _direction += -_player.transform.forward;
        }

        //LEFT
        if(Input.GetKey(_keyboard.left))
        {
            _direction += -_player.transform.right;
        }

        //RIGHT
        if(Input.GetKey(_keyboard.right))
        {
            _direction += _player.transform.right;
        }

        _updateDirection(_direction);
    }

    private void UpdateMousePosition()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition = _camera.ScreenToViewportPoint(_mousePosition + Vector3.forward * 10f);
        _updateMousePos(_mousePosition);
    }

    private void UpdateInteract()
    {
        if(Input.GetKeyDown(_keyboard.interact))
        {
            _onInteract();
        }
    }

    private void UpdateThrow()
    {
        if(Input.GetKeyDown(_keyboard.launch))
        {
            _onThrow();
        }
    }

    public void UpdateKeys(E_Key key, KeyCode keycode)
    {
        switch (key)
        {
            case E_Key.FORWARD:
                _keyboard.forward = keycode;
                break;
            case E_Key.LEFT:
                _keyboard.left = keycode;
                break;
            case E_Key.RIGHT:
                _keyboard.right = keycode;
                break;
            case E_Key.BACKWARD:
                _keyboard.backward = keycode;
                break;
            case E_Key.CROUCH:
                _keyboard.crouch = keycode;
                break;
            case E_Key.INTERACT:
                _keyboard.interact = keycode;
                break;
            case E_Key.LOOKAT:
                _keyboard.lookAt = keycode;
                break;
            case E_Key.THROW:
                _keyboard.launch = keycode;
                break;
            default:
                break;
        }
    }
}