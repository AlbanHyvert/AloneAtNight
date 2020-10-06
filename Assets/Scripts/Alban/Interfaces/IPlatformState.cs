using UnityEngine;

public interface IPlatformState
{
    void Init(Transform self);
    void Enter();
    void Tick();
    void Exit();
}