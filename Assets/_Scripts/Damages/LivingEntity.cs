using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour
{
	[SerializeField] private float m_Life = 10;
	private float m_MaxLife;

	public UnityEvent OnDeath;
	public UnityEvent OnDamaged;

	private void Awake()
	{
		if(OnDeath == null)
			OnDeath = new UnityEvent();
		if(OnDamaged == null)
			OnDamaged = new UnityEvent();
	}

	private void Start()
	{
		m_MaxLife = m_Life;
	}

	public void UpdateLife(float iLifeDelta)
	{
		m_Life = Mathf.Clamp(m_Life + iLifeDelta, 0, m_MaxLife);

		if(iLifeDelta < 0)
			OnDamaged.Invoke();
		if(m_Life <= 0)
			OnDeath.Invoke();
	}

	public float GetLife()
	{
		return m_Life;
	}

	public float GetMaxLife()
	{
		return m_MaxLife;
	}
}
