using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameState : IGameState
{
	private GameStatistics m_Statistics;

	public void Enter()
	{
		m_Statistics = GameManager.Get().GetStatistics();
	}

	public void Exit()
	{
	}

	public void Update(float iDeltaTime)
	{
		m_Statistics.m_PlayTime += iDeltaTime;
	}

	public float GetPlayTime()
	{
		return m_Statistics.m_PlayTime;
	}
}

