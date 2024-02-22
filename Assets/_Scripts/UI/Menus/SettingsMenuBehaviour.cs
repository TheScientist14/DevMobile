using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuBehaviour : MonoBehaviour
{
	private GameObject m_PreviousMenu = null;

	public void ShowSettings(GameObject iFrom)
	{
		m_PreviousMenu = iFrom;
		m_PreviousMenu?.SetActive(false);
		gameObject.SetActive(true);
	}

	public void Back()
	{
		gameObject.SetActive(false);
		m_PreviousMenu?.SetActive(true);
		m_PreviousMenu = null;
	}
}
