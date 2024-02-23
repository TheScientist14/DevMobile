using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	[Serializable]
	public struct Path
	{
		public string Name;
		public bool DoLoop;
	}

	[SerializeField] private List<Path> m_Pathes = new List<Path>();

	[SerializeField] private Rect m_SpawnZone;
	[SerializeField] private Rect m_InitZone;

    private EnemiesPoolManager m_EnemiesPool;
    [SerializeField] private List<PoolingEnemy> m_EnemiesToPool;
    [SerializeField] private float m_SpawnDelay = 0.5f;

	[SerializeField] private EnemyGenerator m_Generator;

    private void OnEnable()
    {
		m_Generator.OnHeartBeat += OnGeneratorHeartBeat;
    }

    private void OnDisable()
    {
        m_Generator.OnHeartBeat -= OnGeneratorHeartBeat;
    }

    void Start()
	{
		m_EnemiesPool = EnemiesPoolManager.SharedInstance;
	}

	public void OnGeneratorHeartBeat()
	{
		StartCoroutine(HandleMultiSpawn());
	}

    public IEnumerator HandleMultiSpawn()
	{
		List<GameObject> queueCopy = new();
        queueCopy.AddRange(m_Generator.GetSpawnQueue());
        List<int> idsCopy = new();
        idsCopy.AddRange(m_Generator.GetSpawnQueueIdentifiers());
		Assert.AreEqual(queueCopy.Count, idsCopy.Count);

        int i = 0;
		while (i < queueCopy.Count)
		{
			Spawn(queueCopy[i], idsCopy[i]);
			++i;
			yield return new WaitForSeconds(m_SpawnDelay);
		}
	}

	public void Spawn(GameObject iToSpawn, int iId)
	{
        PoolingEnemy newEnemy = m_EnemiesPool.GetEnemy(iToSpawn);
		if (newEnemy != null)
		{
			newEnemy.transform.position = GetRandomPosInside(m_SpawnZone);
		}
		else
        {
            Debug.LogWarning("Trying to pool a null enemy on Spawn !");
			return;
        }

		if (newEnemy.TryGetComponent(out GeneratorIdentifier identifier))
		{
			identifier.Setup(m_Generator, iId);
		}

        if (newEnemy.TryGetComponent(out LivingEntity life))
        {
			life.Restart();
        }

        MovingEntity movingEntity;
		if(!newEnemy.TryGetComponent(out movingEntity))
		{
			Debug.LogWarning("Spawned an object with no moving entity component.");
			return;
		}

        movingEntity.enabled = true;
        movingEntity.SetStartPos(GetRandomPosInside(m_InitZone));
		Path path = m_Pathes[Random.Range(0, m_Pathes.Count)];
		movingEntity.SetPathName(path.Name);
		movingEntity.SetDoLoop(path.DoLoop);
	}

	private Vector2 GetRandomPosInside(Rect iZone)
	{
		return new Vector2(Random.Range(iZone.min.x, iZone.max.x), Random.Range(iZone.min.y, iZone.max.y));
	}
}
