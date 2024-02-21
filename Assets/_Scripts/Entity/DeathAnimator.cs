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
		entity.OnDeath += TriggerDeathAnimation;
	}

	private void TriggerDeathAnimation()
	{
		PlayerMovement playerMovement;
		if(TryGetComponent(out playerMovement))
			playerMovement.enabled = false;
		MovingEntity movingEntity;
		if(TryGetComponent(out movingEntity))
			movingEntity.enabled = false;
		ShootingEntity shootingEntity;
		if(TryGetComponent(out shootingEntity))
			shootingEntity.enabled = false;

		m_VFXAnimator.SetTrigger("Explode");
		Destroy(gameObject, 0.7f);
	}
}
