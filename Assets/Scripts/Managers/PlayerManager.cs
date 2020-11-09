using Engine.Singleton;
using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private FP_Controller _playerPrefab = null;
    [SerializeField] private FpControllerUI _fpCursorUI = null;

    private FP_Controller _playerInstance = null;

    public FP_Controller GetPlayer { get { return _playerInstance; } }
    public FP_Controller SetPlayer 
    {
        set
        {
            _playerInstance = value;

            if (_updatePlayer != null)
            {
                _updatePlayer(_playerInstance);
            }

            if (_playerPrefab == null)
            {
                _playerPrefab = _playerInstance;
            }
        }
    }
    public FpControllerUI GetFpCursorUI { get { return _fpCursorUI; } }

    private event Action<FP_Controller> _updatePlayer = null;
    public event Action<FP_Controller> UpdatePlayer
    {
        add
        {
            _updatePlayer -= value;
            _updatePlayer += value;
        }
        remove
        {
            _updatePlayer += value;
        }
    }
}