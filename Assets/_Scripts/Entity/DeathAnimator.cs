using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class DeathAnimator : MonoBehaviour
{
	[SerializeField] private Animator m_VFXAnimator;

	// Start is called before the first frame update
	void Start()
	{
		LivingEntity entity = GetComponent<LivingEntity>();
		entity.OnDamaged += TriggerExplosion;
	}

	private void TriggerExplosion()
	{
		m_VFXAnimator.SetTrigger("Explode");
	}
}
