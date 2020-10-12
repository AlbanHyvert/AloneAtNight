using UnityEngine;

public interface IInteractive
{
    void Enter(Transform parent = null);
    void Exit();

    void OnSeen();
    void OnUnseen();
}
