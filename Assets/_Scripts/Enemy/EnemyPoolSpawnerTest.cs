using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolSpawnerTest : MonoBehaviour
{
    EnemiesPoolManager _enemiesPool;

    [SerializeField]
    List<PoolingEnemy> _enemiesToPool;
    
    [SerializeField]
    Transform _spawnPosition;

    [SerializeField]
    float _spawnDelay = 0.5f;
    [SerializeField]
    float _deactivationTimeAfterSpawn = 10.0f;

    void Start()
    {
        _enemiesPool = EnemiesPoolManager.SharedInstance;
        StartCoroutine(SpawnEnemyToPool());
    }

    private IEnumerator SpawnEnemyToPool()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            PoolingEnemy enemyPrefab = _enemiesToPool[Random.Range(0, _enemiesToPool.Count)];
            PoolingEnemy newEnemy = _enemiesPool.GetEnemy(enemyPrefab);
            if (newEnemy != null)
            {
                newEnemy.transform.position = _spawnPosition.transform.position;
                StartCoroutine(DeactivateEnemy(newEnemy));
            }
            else
            {
                Debug.LogWarning("ALED");
            }
        }
    }

    private IEnumerator DeactivateEnemy(PoolingEnemy enemy)
    {
        yield return new WaitForSeconds(_deactivationTimeAfterSpawn);
        _enemiesPool.UnloadEnemy(enemy);
    }
}
