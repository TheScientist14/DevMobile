using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyGeneratorData", fileName = "EnemyGeneratorData")]
public class EnemyGeneratorData : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public float m_Difficulty;
        public GameObject m_EnemyPrefab;
    }

    [SerializeField] private List<Entry> m_Entries;

    public Entry Get(int index)
    {
        return m_Entries[index];
    }

    public int Count()
    {
        return m_Entries.Count;
    }
}