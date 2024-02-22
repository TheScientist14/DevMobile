using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
	[SerializeField] private float m_DamageDealtOnCollision = 1;
	[SerializeField] private bool m_SelfDestructOnCollision = true;

	[SerializeField] private bool m_DamageOverTime;
    [ShowIf("m_DamageOverTime")]
	[SerializeField] private float m_DamagePerHit;
    [ShowIf("m_DamageOverTime")]
    [SerializeField] private float m_HitRate = 1.0f;

    private List<LivingEntity> m_DamageableEntities = new();
    private HashSet<LivingEntity> m_DamageableEntitiesHash = new();

    private LivingEntity m_LivingEntity;

    public void ChangeDamageParams(float CollisionDamage, bool CollisionSelfDestruct, bool HasContinuousDamage, float ContinuousHitDamage, float ContinuousHitRate)
    {
        m_DamageDealtOnCollision = CollisionDamage;
        m_SelfDestructOnCollision = CollisionSelfDestruct;
        m_DamageOverTime = HasContinuousDamage;
        m_DamagePerHit = ContinuousHitDamage;
        m_HitRate = ContinuousHitRate;
    }

	private void Start()
	{
		TryGetComponent(out m_LivingEntity);
	
        if (m_DamageOverTime)
            StartCoroutine(HandleDamageOverTime());
    }

    private IEnumerator HandleDamageOverTime()
    {
        while (true)
        {
            foreach (LivingEntity entity in m_DamageableEntities)
            {
                entity.TakeDamage(m_DamagePerHit);
            }

            yield return new WaitForSeconds(1.0f / m_HitRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D iOther)
	{
		LivingEntity damageable;
		if(!iOther.TryGetComponent(out damageable))
			return;

		damageable.TakeDamage(m_DamageDealtOnCollision);
		if(m_SelfDestructOnCollision)
		{
			if(m_LivingEntity != null)
				m_LivingEntity.Die();
			else
				Destroy(gameObject);
		}
        else if (!m_DamageableEntitiesHash.Contains(damageable))
        {
            m_DamageableEntities.Add(damageable);
            m_DamageableEntitiesHash.Add(damageable);
        }
    }

    private void OnTriggerExit2D(Collider2D iOther)
    {
        LivingEntity damageable;
        if (!iOther.TryGetComponent(out damageable))
            return;

        if (m_DamageableEntitiesHash.Contains(damageable))
        {
            m_DamageableEntities.Remove(damageable);
            m_DamageableEntitiesHash.Remove(damageable);
        }
    }
}
