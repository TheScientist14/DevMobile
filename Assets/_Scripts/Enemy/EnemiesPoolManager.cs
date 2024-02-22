using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class EnemiesPoolManager : MonoBehaviour
{
    public static EnemiesPoolManager SharedInstance;

    [SerializeField] private List<PoolingEnemy> m_PoolingEnemyList;
    
    Dictionary<string, PoolBase<PoolingEnemy>> m_DictPools = new Dictionary<string, PoolBase<PoolingEnemy>>();

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
            if (!m_DictPools.ContainsKey(enemyToPool.gameObject.name)) {
                TryAddPoolForGO(enemyToPool);
            }
        }
    }

    private bool TryAddPoolForGO(PoolingEnemy prefabToSpawn)
    {
        GameObject prefabPool = new GameObject("PoolFor" + prefabToSpawn.name, typeof(PoolBase<PoolingEnemy>));
        prefabPool.transform.SetParent(transform, false);
        PoolBase<PoolingEnemy> EnemyPool = prefabPool.GetComponent<PoolBase<PoolingEnemy>>();
        EnemyPool.SetObjectToPool(prefabToSpawn);
        EnemyPool.PopulatePool();
        return m_DictPools.TryAdd(prefabToSpawn.name, EnemyPool);

    }

    public PoolingEnemy GetEnemy(GameObject enemyPrefab)
    {
        if (m_DictPools.ContainsKey(enemyPrefab.name))
        {
            return m_DictPools[enemyPrefab.name].GetPooledObject();
        } else
        {
            PoolingEnemy poolingEnemy = null;
            if (enemyPrefab.TryGetComponent(out poolingEnemy) && TryAddPoolForGO(poolingEnemy))
            {
                return m_DictPools[enemyPrefab.name].GetPooledObject();
            }
            return null;
        }
    }

    public void UnloadObject(GameObject enemyPrefab)
    {

    }

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
