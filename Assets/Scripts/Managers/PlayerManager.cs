using Engine.Singleton;
using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerController _playerPrefab = null;

    private PlayerController _playerInstance = null;

    public PlayerController GetPlayer { get { return _playerInstance; } }
    public PlayerController SetPlayer 
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

    private event Action<PlayerController> _updatePlayer = null;
    public event Action<PlayerController> UpdatePlayer
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