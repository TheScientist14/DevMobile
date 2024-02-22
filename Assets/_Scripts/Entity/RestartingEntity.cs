using UnityEngine;

public interface IRestartable
{
    public void Restart();
}

public class RestartingEntity : MonoBehaviour
{
    GameStateMachine m_GameStateMachine;

    IRestartable[] m_ToRestart;

    private void Awake()
    {
        m_GameStateMachine = GameManager.Get().GetStateMachine();
        m_ToRestart = GetComponents<IRestartable>();
    }

    private void OnEnable()
    {
        m_GameStateMachine.OnStateChange += OnStateChange;
    }

    private void OnDisable()
    {
        m_GameStateMachine.OnStateChange -= OnStateChange;
    }

    private void OnStateChange(IGameState iPrevious, IGameState iCurrent)
    {
        if (iCurrent is PlayGameState)
        {
            foreach (IRestartable restartable in m_ToRestart)
                restartable.Restart();
        }
    }
}
