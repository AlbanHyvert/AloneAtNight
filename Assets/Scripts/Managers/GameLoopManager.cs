using Engine.Singleton;
using System;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{

    private bool _isPaused = false;

    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            SetPause(value);

            if(_updatePause != null)
                _updatePause(_isPaused);
        }
    }

    #region EVENTS

    private event Action<bool> _updatePause = null;
    public event Action<bool> UpdatePause
    {
        add
        {
            _updatePause -= value;
            _updatePause += value;
        }
        remove
        {
            _updatePause -= value;
        }
    }

    private event Action _updateManager = null;
    public event Action UpdateManager
    {
        add
        {
            _updateManager -= value;
            _updateManager += value;
        }
        remove
        {
            _updateManager -= value;
        }
    }

    private event Action _updatePlayer = null;
    public event Action UpdatePlayer
    {
        add
        {
            _updatePlayer -= value;
            _updatePlayer += value;
        }
        remove
        {
            _updatePlayer -= value;
        }
    }

    private event Action _updatePuzzles = null;
    public event Action UpdatePuzzles
    {
        add
        {
            _updatePuzzles -= value;
            _updatePuzzles += value;
        }
        remove
        {
            _updatePuzzles -= value;
        }
    }

    private event Action _updateCamera = null;
    public event Action UpdateCamera
    {
        add
        {
            _updateCamera -= value;
            _updateCamera += value;
        }
        remove
        {
            _updateCamera -= value;
        }
    }

    private event Action _updatePlatform = null;
    public event Action UpdatePlatform
    {
        add
        {
            _updatePlatform -= value;
            _updatePlatform += value;
        }
        remove
        {
            _updatePlatform -= value;
        }
    }

    private event Action _updateUI = null;
    public event Action UpdateUI
    {
        add
        {
            _updateUI -= value;
            _updateUI += value;
        }
        remove
        {
            _updateUI -= value;
        }
    }

    #endregion EVENTS

    private void Start()
    {
        _isPaused = false;
    }

    private void SetPause(bool value)
    {
        _isPaused = value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            IsPaused = !IsPaused;
        }

        if (_updateManager != null)
        {
            _updateManager();
        }

        if (_updatePlayer != null)
        {
            _updatePlayer();
        }

        if(_updatePlatform != null)
        {
            _updatePlatform();
        }

        if(_updatePuzzles != null)
        {
            _updatePuzzles();
        }

        if (_updateCamera != null)
        {
            _updateCamera();
        }

        if (_updateUI != null)
        {
            _updateUI();
        }
    }
}