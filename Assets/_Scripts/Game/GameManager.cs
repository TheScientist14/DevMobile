using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Singleton;

    private GameStateMachine m_GameStateMachine;

    [SerializeField] private GameStatistics m_GameStatistics;

    private void Awake()
    {
        if (m_Singleton == null)
        {
            m_Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (m_Singleton != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        m_GameStateMachine = new GameStateMachine();
        m_GameStateMachine.OnStateChange += OnGameStateChanged;
    }

    private void OnDisable()
    {
        m_GameStateMachine.OnStateChange -= OnGameStateChanged;
        m_GameStateMachine = null;
    }

    private void Update()
    {
        m_GameStateMachine.Update(Time.deltaTime);
    }

    public void OnGameStateChanged(IGameState iPrevious, IGameState iCurrent)
    {
        if (iCurrent is PlayGameState)
        {
            m_GameStatistics.Register(true);
        }
        else if (iPrevious is PlayGameState)
        {
            m_GameStatistics.Register(false);
        }

        Debug.Log("Changing GameState : " + nameof(iPrevious) + " => " + nameof(iCurrent));
    }

    public static GameManager Get()
    {
        return m_Singleton;
    }

    public GameStatistics GetStatistics()
    {
        return m_GameStatistics;
    }
}
