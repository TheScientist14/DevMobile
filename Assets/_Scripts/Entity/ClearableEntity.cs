using UnityEngine;

// Should be factorised with RestartingEntity somehow
public class ClearableEntity : MonoBehaviour
{
    private GameStateMachine m_GameStateMachine;
    private IRecyclable[] m_ToRecycle;


    private void Awake()
    {
        m_GameStateMachine = GameManager.Get().GetStateMachine();
        m_ToRecycle = GetComponents<IRecyclable>();
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
        if (iPrevious is EndGameState)
        {
            foreach (IRecyclable recyclable in m_ToRecycle)
                recyclable.Recycle();
        }
    }
}
