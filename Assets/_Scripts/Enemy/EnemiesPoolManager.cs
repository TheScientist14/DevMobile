using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.Serialization;

public class EnemiesPoolManager : MonoBehaviour
{
    public static EnemiesPoolManager SharedInstance;

    [SerializeField] private List<PoolingEnemy> m_PoolingEnemyList;

    [Header("DEBUG"), SerializeField, SerializedDictionary]
    SerializedDictionary<string, EnemyPool> m_DictPools = new SerializedDictionary<string, EnemyPool>();

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        } else
        {
            Destroy(gameObject);
        }
        foreach (PoolingEnemy enemyToPool in m_PoolingEnemyList)
        {
            if (!m_DictPools.ContainsKey(enemyToPool.EnemyName)) {
                TryAddPoolForGO(enemyToPool);
            }
        }
    }

    private bool TryAddPoolForGO(PoolingEnemy prefabToSpawn)
    {
        GameObject prefabPool = new GameObject("PoolFor" + prefabToSpawn.EnemyName, typeof(EnemyPool));
        prefabPool.transform.SetParent(transform, false);
        EnemyPool EnemyPool = prefabPool.GetComponent<EnemyPool>();
        EnemyPool.SetObjectToPool(prefabToSpawn);
        EnemyPool.PopulatePool();
        return m_DictPools.TryAdd(prefabToSpawn.EnemyName, EnemyPool);
    }

    public PoolingEnemy GetEnemy(GameObject enemyPrefab)
    {
        PoolingEnemy poolingPrefab = enemyPrefab.GetComponent<PoolingEnemy>();
        if (poolingPrefab is null)
            return null;
        return GetEnemy(poolingPrefab);
    }
    public PoolingEnemy GetEnemy(PoolingEnemy enemyPrefab)
    {
        if (m_DictPools.ContainsKey(enemyPrefab.EnemyName))
        {
            return m_DictPools[enemyPrefab.EnemyName].GetPooledObject();
        }
        else
        {
            if(TryAddPoolForGO(enemyPrefab))
            {
                return m_DictPools[enemyPrefab.EnemyName].GetPooledObject();
            }
            return null;
        }
    }

    public void UnloadObject(GameObject GOOfEnemyToUnload)
    {
        PoolingEnemy EnemyToUnload = null;
        if (GOOfEnemyToUnload.TryGetComponent<PoolingEnemy>(out EnemyToUnload)) {
            UnloadEnemy(EnemyToUnload);
        }
        else
        {
            Destroy(GOOfEnemyToUnload);
        }

    }
    public void UnloadEnemy(PoolingEnemy poolingEnemy)
    {
        if (poolingEnemy == null) return;
        if (m_DictPools.ContainsKey(poolingEnemy.EnemyName))
        {
            m_DictPools[poolingEnemy.EnemyName].UnloadObject(poolingEnemy);
        }
    }

    public void UnloadObjects(List<PoolingEnemy> poolingEnemy)
    {
        foreach(PoolingEnemy enemy in poolingEnemy)
        {
            UnloadEnemy(enemy);
        }
    }

    /// <summary>
    /// DO NOT USE, TO MUCH COASTS
    /// </summary>
    /// <param name="enemiesPrefabToSpawn"></param>
    /// <returns></returns>
    public List<PoolingEnemy> GetEnemies(List<GameObject> enemiesPrefabToSpawn)
    {
        List<PoolingEnemy> enemies = new List<PoolingEnemy>();
        foreach(GameObject go in enemiesPrefabToSpawn)
        {
            enemies.Add(GetEnemy(go));
        }
        return enemies;
    }
}
