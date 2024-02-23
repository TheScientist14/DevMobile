using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEntity : MonoBehaviour, IRestartable
{
	[Serializable]
	public struct ActiveMuzzlesIndexList
	{
		public List<int> m_MuzzlesIndexes;
	}

	// Serialized for debug
	[SerializeField] private bool m_IsShooting = true;

	// [TEMP] some of this data should be overrided by some enemy controller
	[SerializeField] private float m_FireRate;
	[SerializeField] private float m_BulletLifetime; // this could be computed and optimized

    [SerializeField] private Transform m_BulletPrefab;
    [SerializeField] private ProjectileData m_ProjectileData;
    private ProjectilePool m_ProjectilePool;

	[SerializeField] private Transform m_BulletContainer;

	[SerializeField] private List<Transform> m_Muzzles;
	[SerializeField] private List<ActiveMuzzlesIndexList> m_ActiveMuzzleIndexesPerShot;

	private int m_ShootIndexInShootingPattern = 0;

    private void OnEnable()
    {
        m_ProjectilePool = ProjectilePool.SharedInstance;
        m_IsShooting = true;
        if (CheckIndexesInMuzzles() && GameManager.Get().GetStateMachine().GetState() is not MenuGameState)
            StartCoroutine(HandleShootingPattern());
    }

    public void Restart()
    {
        m_IsShooting = true;
        StartCoroutine(HandleShootingPattern());
    }

    private void OnDisable()
    {
		m_IsShooting = false;
    }

    private IEnumerator HandleShootingPattern()
	{
		while(m_IsShooting && m_FireRate > 0)
		{
			Shoot();

			++m_ShootIndexInShootingPattern;
			if(m_ShootIndexInShootingPattern >= m_ActiveMuzzleIndexesPerShot.Count)
				m_ShootIndexInShootingPattern = 0;

			yield return new WaitForSeconds(1.0f / m_FireRate);
		}
	}

	private void Shoot()
	{
		List<int> currentActiveMuzzles = m_ActiveMuzzleIndexesPerShot[m_ShootIndexInShootingPattern].m_MuzzlesIndexes;
		int nbBulletsToShoot = currentActiveMuzzles.Count;

		for(int i = 0; i < nbBulletsToShoot; ++i)
		{
			Transform currentMuzzle = m_Muzzles[currentActiveMuzzles[i]];

            PoolableProjectile newBullet = m_ProjectilePool.GetPooledObject();
            newBullet.transform.position = currentMuzzle.position;
            newBullet.transform.rotation = currentMuzzle.rotation;
            newBullet.gameObject.layer = gameObject.layer;

            if (newBullet.TryGetComponent(out ProjectileSyncer projectileSyncer))
            {
                projectileSyncer.SynchronizeWithProjectileData(m_ProjectileData);
                projectileSyncer.ProjectileMovement.Init(currentMuzzle.up);
                // [WARNING] We should implement a lifetime feature when Instantiate/Destroy get replaced by object pooling
                // Destroy(newBullet.gameObject, m_BulletLifetime);
            }
            else
            {
                Debug.LogWarning("Trying to spawn a bullet without ProjectileMovement component !");
                Destroy(newBullet.gameObject);
            }
        }
    }

	private bool CheckIndexesInMuzzles()
	{
		foreach(ActiveMuzzlesIndexList el in m_ActiveMuzzleIndexesPerShot)
		{
			foreach(int index in el.m_MuzzlesIndexes)
			{
				if(index < 0 || index >= m_Muzzles.Count)
				{
					Debug.LogWarning("Muzzle index out of Muzzle list range !");
					return false;
				}
			}
		}
		return true;
	}

	public void Shoot(bool iShoot)
	{
		m_IsShooting = iShoot;
	}
}
