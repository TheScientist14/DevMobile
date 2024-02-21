using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
	[SerializeField] private float m_DamageDealtOnCollision = 1;
	[SerializeField] private bool m_SelfDestructOnCollision = true;

	private LivingEntity m_LivingEntity;

	private void Start()
	{
		TryGetComponent(out m_LivingEntity);
	}

	private void OnTriggerEnter2D(Collider2D iOther)
	{
		LivingEntity damageable;
		if(!iOther.TryGetComponent(out damageable))
			return;

		damageable.UpdateLife(-m_DamageDealtOnCollision);
		if(m_SelfDestructOnCollision)
		{
			if(m_LivingEntity != null)
				m_LivingEntity.Die();
			else
				Destroy(gameObject);
		}
	}
}
