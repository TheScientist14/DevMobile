using System;

public interface IGameStateMachine
{
	public void SetState(IGameState iState);
}

public class GameStateMachine : IGameStateMachine
{
	private IGameState m_CurrentState;

	/// <summary>
	/// Expected callback :
	/// void OnStateChange(IGameState iPrevious, IGameState iCurrent)
	/// </summary>
	public event Action<IGameState, IGameState> OnStateChange;

	public GameStateMachine()
	{
		m_CurrentState = new MenuGameState();
	}

	public void SetState(IGameState iState)
	{
		IGameState previous = m_CurrentState;
		previous.Exit();

		m_CurrentState = iState;
		OnStateChange?.Invoke(previous, m_CurrentState);

		m_CurrentState.Enter();
	}

	public void Update(float iDeltaTime)
	{
		m_CurrentState.Update(iDeltaTime);
	}

	public bool TryGetState<T>(out T currentState) where T : class, IGameState
	{
		currentState = m_CurrentState as T;
		return currentState != null;
	}

	public IGameState GetState()
	{
		return m_CurrentState;
	}
}
