using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LivingEntity))]
public class DeathAnimator : MonoBehaviour
{
	[SerializeField] private Animator m_VFXAnimator;
	[SerializeField] private bool m_DestroyOnEnd;

	private LivingEntity m_LivingEntity;

	[SerializeField] private UnityEvent m_OnEndEvent;

	private void Awake()
	{
		m_LivingEntity = GetComponent<LivingEntity>();
	}

	private void OnEnable()
	{
		m_LivingEntity.RegisterOnDeathEvent(TriggerDeathAnimation);
	}

	private void OnDisable()
	{
		m_LivingEntity.UnregisterOnDeathEvent(TriggerDeathAnimation);
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
			shootingEntity.Shoot(false);

		m_VFXAnimator.SetTrigger("Explode");
		if(m_DestroyOnEnd)
			Destroy(gameObject, 0.7f);
        m_OnEndEvent.Invoke();

    }
}
