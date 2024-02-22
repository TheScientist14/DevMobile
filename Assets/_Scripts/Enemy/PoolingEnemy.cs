using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingEnemy : Poolable
{
    [SerializeField] private string m_EnemyName = "Enemy";
    public string EnemyName => m_EnemyName;
}
