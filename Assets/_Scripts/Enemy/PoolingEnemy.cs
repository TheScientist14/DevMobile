using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingEnemy : Poolable
{
    [SerializeField] private string m_EnemyName = "Enemy";
    public string EnemyName => m_EnemyName;

    private EnemiesPoolManager m_poolManager;

    private void Start()
    {
        m_poolManager = EnemiesPoolManager.SharedInstance;
    }

    public void Recycle()
    {
        m_poolManager.UnloadEnemy(this);
    }
}
