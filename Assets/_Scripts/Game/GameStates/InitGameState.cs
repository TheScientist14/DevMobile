using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGameState : IGameState
{
	public void Enter()
	{
		GameManager.Get().GetStatistics().m_PlayTime = 0f;
	}

	public void Exit()
	{

	}

	public void Update(float iDeltaTime)
	{

	}
}
