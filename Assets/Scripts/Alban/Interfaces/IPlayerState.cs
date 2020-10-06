public interface IPlayerState
{
    void Init(FP_Controller self);
    void Enter();
    void Tick();
    void Exit();
}
