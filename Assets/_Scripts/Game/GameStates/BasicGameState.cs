using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicGameState", menuName = "Gamestate", order = 1)]
public class BasicGameState : ScriptableObject
{
	public enum EGameState
	{
		Menu,
		Init,
		Play,
		Pause,
		End,
	}

	public EGameState GameState;
	private IGameState MachineState = null;

	public IGameState GetMachineState()
	{
		if(MachineState == null)
		{
			switch(GameState)
			{
				case EGameState.Menu:
					MachineState = new MenuGameState();
					break;
				case EGameState.Init:
					MachineState = new InitGameState();
					break;
				case EGameState.Play:
					MachineState = new PlayGameState();
					break;
				case EGameState.Pause:
					MachineState = new PauseGameState();
					break;
				case EGameState.End:
					MachineState = new EndGameState();
					break;
				default:
					Debug.LogError($"{GameState} is not supported.");
					break;
			}
		}
		return MachineState;
	}
}
