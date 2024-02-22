using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuBehaviour : BasicPanel<EndGameState>
{
	[SerializeField] private Transform m_StatsContainer;
	[SerializeField] private LabelledValue m_LabelledValue;

	private GameStatistics m_GameStatistics;

	protected override void Awake()
	{
		base.Awake();
		m_GameStatistics = GameManager.Get().GetStatistics();
	}

	private void OnEnable()
	{
		// reset statistics display
		foreach(Transform child in m_StatsContainer)
			Destroy(child.gameObject);

		// playtime
		LabelledValue timeStat = Instantiate(m_LabelledValue, m_StatsContainer);
		timeStat.SetLabel("Time survived");
		TimeSpan playTimeSpan = TimeSpan.FromSeconds(m_GameStatistics.m_PlayTime);

		string format = "%s's'";
		if(playTimeSpan.TotalHours >= 1)
			format = "%h'h:'%mm'min:'%ss's'";
		else if(playTimeSpan.TotalMinutes >= 1)
			format = "%m'min:'%ss's'";

		timeStat.SetValue(playTimeSpan.ToString(format));

		foreach(GameStatisticCounter stat in m_GameStatistics.GetCounters())
		{
			LabelledValue labelledValue = Instantiate(m_LabelledValue, m_StatsContainer);
			labelledValue.SetLabel(stat.GetLabel());
			labelledValue.SetValue(stat.GetCount().ToString());
		}
	}
}
