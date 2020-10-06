using UnityEngine;

public interface IInteractive
{
    void Enter(Transform parent);
    void Exit();

    void OnSeen();
    void OnUnseen();
}
