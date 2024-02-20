using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEntity : MonoBehaviour
{
    // Serialized for debug
    [SerializeField] private bool m_IsShooting = true;

    // [TEMP] some of this data should be overrided by some enemy controller
    [SerializeField] private List<Vector2> m_Directions;
    [SerializeField] private int m_NbBulletsPerShot;
    [SerializeField] private float m_FireRate;
    [SerializeField] private float m_BulletLifetime; // this could be computed and optimized

    // [TEMP] this data should just be taken from the entity config: 
    [SerializeField] private Transform m_BulletPrefab;

    [SerializeField] private Transform m_BulletContainer;
    [SerializeField] private List<Transform> m_Muzzles;

    private void Start()
    {
        StartCoroutine(HandleShooting());
    }

    private IEnumerator HandleShooting()
    {
        while (m_IsShooting && m_FireRate > 0)
        {
            Shoot();
            yield return new WaitForSeconds(1.0f / m_FireRate);
        }
    }

    private void Shoot()
    {
        if (m_Directions.Count != m_NbBulletsPerShot
            || m_Muzzles.Count != m_NbBulletsPerShot)
        {
            Debug.Log("there is not the same amount of bullets and directions for their movement. Some bullets won't be shot !");
        }

        int nbBulletsToShoot = Mathf.Min(m_Muzzles.Count, m_Directions.Count, m_NbBulletsPerShot);

        for (int i = 0; i < nbBulletsToShoot; ++i)
        {
            // [TEMP] We should acquire the bullet from a pool
            Transform newBullet = Instantiate(m_BulletPrefab, m_Muzzles[i].position, m_Muzzles[i].rotation, m_BulletContainer);
            newBullet.gameObject.layer = gameObject.layer;
            if (newBullet.TryGetComponent(out ProjectileMovement projectileMovement))
            {
                projectileMovement.Init(m_Directions[i]);
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
}
