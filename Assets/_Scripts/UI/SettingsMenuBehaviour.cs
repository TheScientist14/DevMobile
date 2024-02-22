using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuBehaviour : MonoBehaviour
{
	private GameObject m_PreviousMenu = null;

	public void ShowSettings(GameObject iFrom)
	{
		m_PreviousMenu = iFrom;
		gameObject.SetActive(true);
	}

	public void Back()
	{
		gameObject.SetActive(false);
		m_PreviousMenu?.SetActive(true);
		m_PreviousMenu = null;
	}
}
