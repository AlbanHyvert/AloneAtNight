using UnityEngine;

public class PlayerTeleportState : MonoBehaviour, IPlayerState
{
    private PlayerController _self = null;
    private CharacterController _selfController = null;

    void IPlayerState.Enter()
    {
        _selfController.enabled = false;
    }

    void IPlayerState.Exit()
    {
        _selfController.enabled = true;
    }

    void IPlayerState.Init(PlayerController self)
    {
        _self = self;
        _selfController = self.GetData.controller;
    }

    void IPlayerState.Tick()
    {

    }
}
