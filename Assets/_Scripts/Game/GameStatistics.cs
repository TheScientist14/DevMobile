using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStatistics
{
	[SerializeField] private List<GameStatisticCounter> m_Counters;
	public float m_PlayTime = 0f;

	public void Register(bool iRegister)
	{
		foreach(GameStatisticCounter counter in m_Counters)
		{
			counter.Register(iRegister);
		}
	}

	public List<GameStatisticCounter> GetCounters()
	{
		return m_Counters;
	}
}

[Serializable]
public class GameStatisticCounter
{
	[SerializeField] private VoidEvent m_EventToCount;
	[SerializeField] private string m_Label;

	private int m_Count;

	public void Register(bool iRegister)
	{
		if(iRegister)
			m_EventToCount.OnEventTrigger += OnEventTriggered;
		else
			m_EventToCount.OnEventTrigger -= OnEventTriggered;
	}

	private void OnEventTriggered()
	{
		++m_Count;
	}

	public string GetLabel()
	{
		return m_Label;
	}

	public int GetCount()
	{
		return m_Count;
	}
}
