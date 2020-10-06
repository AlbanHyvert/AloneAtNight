public interface IPlayerState
{
    void Init(PlayerController self);
    void Enter();
    void Tick();
    void Exit();
}
