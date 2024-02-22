public interface IGameState
{
    public void Enter();
    public void Exit();
    public void Update(float iDeltaTime);
}
