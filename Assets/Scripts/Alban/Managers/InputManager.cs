using System;
using UnityEngine;
using Engine.Singleton;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private Keyboard _keyboard;

    private Vector3 _direction = Vector3.zero;
    private Rigidbody _player = null;

    private event Action<float, float> _updateMouseXY = null;
    public event Action<float, float> UpdateMouseXY
    {
        add
        {
            _updateMouseXY -= value;
            _updateMouseXY += value;
        }
        remove
        {
            _updateMouseXY -= value;
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

    #region Structs
    [System.Serializable]
    private struct Keyboard
    {
        public KeyCode forward;
        public KeyCode left;
        public KeyCode right;
        public KeyCode backward;
        public KeyCode interact;
        public KeyCode launch;
        public KeyCode crouch;
    }
    #endregion Structs

    private void Start()
    {
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

    private void Tick()
    {
        if (_updateMouseXY != null)
            UpdateMousePosition();

        if (_updateDirection != null)
            UpdateMovement();

        if (_onInteract != null)
            UpdateInteract();

        if (_onThrow != null)
            UpdateThrow();
    }

    private void UpdateMovement()
    {
        _direction = Vector3.zero;

        if (Input.GetKey(_keyboard.forward))
        {
            _direction += _player.transform.forward;
        }

        if(Input.GetKey(_keyboard.backward))
        {
            _direction += -_player.transform.forward;
        }

        if(Input.GetKey(_keyboard.left))
        {
            _direction += -_player.transform.right;
        }

        if(Input.GetKey(_keyboard.right))
        {
            _direction += _player.transform.right;
        }

        _updateDirection(_direction);
    }

    private void UpdateMousePosition()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        _updateMouseXY(mouseX, mouseY);
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

    public void UpdateKeys(e_Keycode key, KeyCode keycode)
    {
        switch (key)
        {
            case e_Keycode.FORWARD:
                _keyboard.forward = keycode;
                break;
            case e_Keycode.LEFT:
                _keyboard.left = keycode;
                break;
            case e_Keycode.RIGHT:
                _keyboard.right = keycode;
                break;
            case e_Keycode.BACKWARD:
                _keyboard.backward = keycode;
                break;
            case e_Keycode.INTERACT:
                _keyboard.interact = keycode;
                break;
            case e_Keycode.THROW:
                _keyboard.launch = keycode;
                break;
            case e_Keycode.CROUCH:
                _keyboard.crouch = keycode;
                break;
        }
    }
}