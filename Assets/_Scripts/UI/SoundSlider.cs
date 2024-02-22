using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SoundSlider : MonoBehaviour
{
	[SerializeField] private AudioMixerGroup m_MixerGroup;

	// Start is called before the first frame update
	void Start()
	{
		Slider slider = GetComponent<Slider>();
		slider.onValueChanged.AddListener(_UpdateVolume);
	}

	// Update is called once per frame
	private void _UpdateVolume(float iValue)
	{
		m_MixerGroup.audioMixer.SetFloat(m_MixerGroup.name + "Volume", iValue);
	}
}
