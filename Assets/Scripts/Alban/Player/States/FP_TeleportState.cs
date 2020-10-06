using UnityEngine;

public class FP_TeleportState : MonoBehaviour, IPlayerState
{
    private FP_Controller _self = null;
    private CharacterController _selfController = null;

    void IPlayerState.Enter()
    {
        _selfController.enabled = false;
    }

    void IPlayerState.Exit()
    {
        _selfController.enabled = true;
    }

    void IPlayerState.Init(FP_Controller self)
    {
        _self = self;
        _selfController = self.GetData.controller;
    }

    void IPlayerState.Tick()
    {

    }
}
