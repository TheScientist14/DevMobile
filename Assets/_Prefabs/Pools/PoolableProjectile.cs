using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableProjectile : Poolable
{
    private ProjectilePool m_projectilePool;

    private void Start()
    {
        m_projectilePool = ProjectilePool.SharedInstance;
    }
}
