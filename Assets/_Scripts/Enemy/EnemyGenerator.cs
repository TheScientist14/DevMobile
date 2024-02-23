using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGenerator : MonoBehaviour, IRestartable
{
    [SerializeField] private EnemyGeneratorData m_EnemyGeneratorData;
    [SerializeField] private AnimationCurve m_OpeningCurve;
    [SerializeField] private AnimationCurve m_PeriodicPhaseCurve;

    [SerializeField] private float m_OpeningDuration;
    [SerializeField] private float m_PeriodDuration;
    [SerializeField] private float m_RefreshTime;
    [SerializeField] private float m_MaxDifficulty;
    [SerializeField] private float m_DifficultyGainPerSurvivedPeriod;

    [Header("Debug")]
    [SerializeField] private bool m_DoLogGeneratorHeartBeatReports;

    private float m_CurrentDifficulty;
    private int m_SurvivedPeriodsCount;
    private bool m_IsOpeningDone;
    private float m_Time;

    private GameStateMachine m_GameStateMachine;

    private IEnumerator m_CurrentHandle;


    private float m_MinEnemyDifficulty;
    private float m_MaxEnemyDifficulty;
    private List<float> m_SortedDifficulties;
    private List<int> m_EnemiesIndexesSortedByDifficulty;

    private List<GameObject> m_SpawnQueue;
    private List<int> m_SpawnQueueIdentifiers;

    public event Action OnHeartBeat;

    private void Awake()
    {
        int enemyTypesCount = m_EnemyGeneratorData.Count();
        m_SortedDifficulties = new() { Capacity = enemyTypesCount };
        m_EnemiesIndexesSortedByDifficulty = new() { Capacity = enemyTypesCount };
        for (int i = 0; i < enemyTypesCount; ++i)
        {
            m_EnemiesIndexesSortedByDifficulty.Add(i);
            m_SortedDifficulties.Add(m_EnemyGeneratorData.Get(i).m_Difficulty);
        }
        ListUtility.Sort(ref m_EnemiesIndexesSortedByDifficulty, ref m_SortedDifficulties);

        m_MinEnemyDifficulty = m_SortedDifficulties[0];
        m_MaxEnemyDifficulty = m_SortedDifficulties[^1];

        m_SpawnQueue = new();
        m_SpawnQueueIdentifiers = new();
    }

    public void Restart()
    {
        m_SpawnQueue.Clear();
        m_SpawnQueueIdentifiers.Clear();
        m_CurrentDifficulty = 0f;
        m_SurvivedPeriodsCount = 0;
        m_IsOpeningDone = false;
        m_Time = 0f;
    }

    private void OnEnable()
    {
        m_GameStateMachine = GameManager.Get().GetStateMachine();
        m_GameStateMachine.OnStateChange += OnStateChanged;
    }

    private void OnDisable()
    {
        m_GameStateMachine.OnStateChange -= OnStateChanged;
    }

    private void OnStateChanged(IGameState iPrevious, IGameState iCurrent)
    {
        if (iCurrent is PlayGameState)
        {
            m_CurrentHandle ??= HandleGeneratorHeartBeat();
            StartCoroutine(m_CurrentHandle);
        }
        else
        {
            if (m_CurrentHandle != null)
                StopCoroutine(m_CurrentHandle);
        }
    }

    private IEnumerator HandleGeneratorHeartBeat()
    {
        while (true)
        {
            string debugLog = string.Empty;
            if (m_DoLogGeneratorHeartBeatReports)
            {
                debugLog += $"GeneratorHeartBeat Report :\n";
                debugLog += $"Time : {m_Time}\n";
                debugLog += $"Is in Opening Phase : {!m_IsOpeningDone}\n";
                debugLog += $"Is in Periodic Phase : {m_IsOpeningDone}\n";
                debugLog += $"---\n";
                debugLog += $"Current Difficulty : {m_CurrentDifficulty}\n";
            }

            m_SpawnQueue.Clear();
            m_SpawnQueueIdentifiers.Clear();

            float targetDifficulty = ComputeTargetDifficulty();
            if (targetDifficulty > m_CurrentDifficulty)
                AdjustDifficulty(targetDifficulty - m_CurrentDifficulty);

            if (m_DoLogGeneratorHeartBeatReports)
            {
                debugLog += $"Target Difficulty : {targetDifficulty}\n";
                debugLog += $"Adjusted Difficulty : {m_CurrentDifficulty}\n";
                debugLog += $"Enemies to spawn : \n";
                foreach (GameObject go in m_SpawnQueue)
                {
                    debugLog += go.name + "\n";
                }
                Debug.Log(debugLog);
            }

            m_Time += m_RefreshTime;
            if (m_IsOpeningDone)
            {
                CheckForOverTime(m_PeriodDuration, out bool newPeriodStarted);
                if (newPeriodStarted) m_SurvivedPeriodsCount++;
            }
            else
            {
                CheckForOverTime(m_OpeningDuration, out m_IsOpeningDone);
            }

            OnHeartBeat?.Invoke();
            yield return new WaitForSeconds(m_RefreshTime);
        }
    }

    private void CheckForOverTime(float iLimit, out bool iNewPeriodStarted)
    {
        iNewPeriodStarted = false;
        float overtime = m_Time - iLimit;
        if (overtime > 0)
        {
            m_Time = overtime;
            iNewPeriodStarted = true;
        }
    }

    private float ComputeTargetDifficulty()
    {
        float targetDifficulty;
        if (m_IsOpeningDone)
        {
            float coef = m_MaxDifficulty + m_SurvivedPeriodsCount * m_DifficultyGainPerSurvivedPeriod;
            targetDifficulty = coef * m_PeriodicPhaseCurve.Evaluate(m_Time / m_PeriodDuration);
        }
        else
        {
            targetDifficulty = m_MaxDifficulty * m_OpeningCurve.Evaluate(m_Time / m_OpeningDuration);
        }

        return targetDifficulty;
    }

    private void AdjustDifficulty(float iNeeded)
    {
        while (iNeeded > m_MinEnemyDifficulty)
        {
            EnemyGeneratorData.Entry chosen;
            int enemyIdentifier;
            if (iNeeded > m_MaxEnemyDifficulty)
            {
                enemyIdentifier = Random.Range(0, m_EnemiesIndexesSortedByDifficulty.Count);
                chosen = m_EnemyGeneratorData.Get(enemyIdentifier);
            }
            else
            {
                int index = 0;
                do ++index;
                while (iNeeded > m_SortedDifficulties[index]);

                if (m_SortedDifficulties[index] - iNeeded < iNeeded - m_SortedDifficulties[index - 1])
                    --index;

                enemyIdentifier = m_EnemiesIndexesSortedByDifficulty[index];
                chosen = m_EnemyGeneratorData.Get(enemyIdentifier);
            }
            m_SpawnQueue.Add(chosen.m_EnemyPrefab);
            m_SpawnQueueIdentifiers.Add(enemyIdentifier);
            m_CurrentDifficulty += chosen.m_Difficulty;
            iNeeded -= chosen.m_Difficulty;
        }
    }

    public void LowerCurrentDifficulty(int loweringTypeId)
    {
        m_CurrentDifficulty -= m_EnemyGeneratorData.Get(loweringTypeId).m_Difficulty;
    }

    public List<GameObject> GetSpawnQueue()
    {
        return m_SpawnQueue;
    }

    public List<int> GetSpawnQueueIdentifiers()
    {
        return m_SpawnQueueIdentifiers;
    }
}
