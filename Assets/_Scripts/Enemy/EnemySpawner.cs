using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	public void Spawn()
	{
		// TODO: replace gameObject with proper parameter value
		GameObject instance = Instantiate(gameObject, GetRandomPosInside(m_SpawnZone), Quaternion.identity);

		MovingEntity movingEntity;
		if(!instance.TryGetComponent(out movingEntity))
		{
			Debug.LogWarning("Spawned an object with no moving entity component.");
			return;
		}

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
