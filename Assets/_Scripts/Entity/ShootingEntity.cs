using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEntity : MonoBehaviour
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

    // [TEMP] this data should just be taken from the entity config: 
    [SerializeField] private Transform m_BulletPrefab;

    [SerializeField] private Transform m_BulletContainer;

    [SerializeField] private List<Transform> m_Muzzles;
    [SerializeField] private List<ActiveMuzzlesIndexList> m_ActiveMuzzleIndexesPerShot;

    private int m_ShootIndexInShootingPattern = 0;

    private void Start()
    {
        if (CheckIndexesInMuzzles())
            StartCoroutine(HandleShootingPattern());
    }

    private IEnumerator HandleShootingPattern()
    {
        while (m_IsShooting && m_FireRate > 0)
        {
            Shoot();

            ++m_ShootIndexInShootingPattern;
            if (m_ShootIndexInShootingPattern >= m_ActiveMuzzleIndexesPerShot.Count)
                m_ShootIndexInShootingPattern = 0;

            yield return new WaitForSeconds(1.0f / m_FireRate);
        }
    }

    private void Shoot()
    {
        List<int> currentActiveMuzzles = m_ActiveMuzzleIndexesPerShot[m_ShootIndexInShootingPattern].m_MuzzlesIndexes;
        int nbBulletsToShoot = currentActiveMuzzles.Count;

        for (int i = 0; i < nbBulletsToShoot; ++i)
        {
            Transform currentMuzzle = m_Muzzles[currentActiveMuzzles[i]];

            // [TEMP] We should acquire the bullet from a pool
            Transform newBullet = Instantiate(m_BulletPrefab, currentMuzzle.position, currentMuzzle.rotation, m_BulletContainer);
            newBullet.gameObject.layer = gameObject.layer;

            if (newBullet.TryGetComponent(out ProjectileMovement projectileMovement))
            {
                projectileMovement.Init(currentMuzzle.up);
                // [WARNING] We should implement a lifetime feature when Instantiate/Destroy get replaced by object pooling
                Destroy(newBullet.gameObject, m_BulletLifetime);
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
        foreach (ActiveMuzzlesIndexList el in m_ActiveMuzzleIndexesPerShot)
        {
            foreach (int index in el.m_MuzzlesIndexes)
            {
                if (index < 0 || index >= m_Muzzles.Count)
                {
                    Debug.LogWarning("Muzzle index out of Muzzle list range !");
                    return false;
                }
            }
        }
        return true;
    }
}
