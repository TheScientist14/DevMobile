using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuBehaviour : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown m_InputType;
	[SerializeField] private Slider m_MusicVolume;
	[SerializeField] private Slider m_SFXVolume;

	private const string s_InputTypeKey = "InputType";
	private const string s_MusicVolumeKey = "MusicVolume";
	private const string s_SFXVolumeKey = "SFXVolume";

	private GameObject m_PreviousMenu = null;

	private void Awake()
	{
		gameObject.SetActive(false);

		m_InputType.value = PlayerPrefs.GetInt(s_InputTypeKey, 1);
		m_MusicVolume.value = PlayerPrefs.GetFloat(s_MusicVolumeKey, 0.2f);
		m_SFXVolume.value = PlayerPrefs.GetFloat(s_SFXVolumeKey, 0.2f);
	}

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

	private void OnDestroy()
	{
		PlayerPrefs.SetInt(s_InputTypeKey, m_InputType.value);
		PlayerPrefs.SetFloat(s_MusicVolumeKey, m_MusicVolume.value);
		PlayerPrefs.SetFloat(s_SFXVolumeKey, m_SFXVolume.value);
	}
}
