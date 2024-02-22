using UnityEngine;

public class PauseGameState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0.0f;
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
    }

    public void Update(float iDeltaTime)
    {

    }
}
