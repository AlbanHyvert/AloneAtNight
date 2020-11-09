using Engine.Singleton;
using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private Players _players = new Players();
    [SerializeField] private FpControllerUI _fpCursorUI = null;

    private Players _playersInstance = new Players();

    public Players GetPlayersInstance { get { return _playersInstance; } }
    public Players SetPlayers
    {
        set
        {
            _playersInstance = value;

            if (_updatePlayer != null)
            {
                _updatePlayer(_playersInstance);
            }

            _players = _playersInstance;
        }
    }
    public Players GetPlayersPrefab { get { return _players; } }
    public FpControllerUI GetFpCursorUI { get { return _fpCursorUI; } }

    [System.Serializable]
    public struct Players
    {
        public FP_Controller fpsPlayer;
        public ThirdPersonMovement tpsPlayer;
    }

    private event Action<Players> _updatePlayer = null;
    public event Action<Players> UpdatePlayer
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