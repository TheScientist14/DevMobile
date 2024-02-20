using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
	[SerializeField] private float m_Life = 10;
	private float m_MaxLife;

	public event Action OnDeath;
	public event Action OnDamaged;

	private void Start()
	{
		m_MaxLife = m_Life;
	}

    public void UpdateLife(float iLifeDelta)
	{
		m_Life = Mathf.Clamp(m_Life + iLifeDelta, 0, m_MaxLife);

		if(iLifeDelta < 0)
			OnDamaged?.Invoke();
		if(m_Life <= 0)
			OnDeath?.Invoke();
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
