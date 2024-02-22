using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelledValue : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_Label;
	[SerializeField] private TextMeshProUGUI m_Value;

	public void SetLabel(string iLabel)
	{
		m_Label.text = iLabel + ":";
	}

	public void SetValue(string iValue)
	{
		m_Value.text = iValue;
	}
}
