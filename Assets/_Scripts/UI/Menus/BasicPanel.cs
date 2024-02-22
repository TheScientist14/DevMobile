using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPanel<T> : MonoBehaviour where T : IGameState
{
	protected GameStateMachine m_StateMachine;

	protected virtual void Awake()
	{
		m_StateMachine = GameManager.Get().GetStateMachine();
		m_StateMachine.OnStateChange += _OnStateChanged;
	}

	protected virtual void _OnStateChanged(IGameState iPrevState, IGameState iCurState)
	{
		if(iPrevState is T)
			gameObject.SetActive(false);
		if(iCurState is T)
			gameObject.SetActive(true);
	}

	public void SetGameState(BasicGameState iGameState)
	{
		m_StateMachine.SetState(iGameState.GetMachineState());
	}
}
