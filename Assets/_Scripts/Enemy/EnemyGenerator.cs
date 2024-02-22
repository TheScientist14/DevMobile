using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyGeneratorData m_EnemyGeneratorData;
    [SerializeField] private AnimationCurve m_OpeningCurve;
    [SerializeField] private AnimationCurve m_PeriodicPhaseCurve;

    [SerializeField] private float m_OpeningDuration;
    [SerializeField] private float m_PeriodDuration;
    [SerializeField] private float m_UpdateRate;
    [SerializeField] private float m_MaxDifficulty;
    [SerializeField] private float m_DifficultyGainPerSurvivedPeriod;

    private int m_SurvivedPeriodsCount;
    private bool m_IsOpeningDone;
    private float m_Time;
    private float m_DeltaTime;

    private GameStateMachine m_GameStateMachine;

    private IEnumerator m_CurrentHandle;

    private float m_CurrentDifficulty;

    private float m_MinEnemyDifficulty;
    private float m_MaxEnemyDifficulty;
    private List<float> m_SortedDifficulties;
    private List<int> m_EnemiesIndexesSortedByDifficulty;

    private List<GameObject> m_SpawnQueue;

    private void Awake()
    {
        m_DeltaTime = 1.0f / m_UpdateRate;
        int enemyTypesCount = m_EnemyGeneratorData.Count();
        m_SortedDifficulties = new() { Capacity = enemyTypesCount };
        m_EnemiesIndexesSortedByDifficulty = new() { Capacity = enemyTypesCount };
        for (int i = 0; i < enemyTypesCount; ++i)
        {
            m_EnemiesIndexesSortedByDifficulty[i] = i;
            m_SortedDifficulties[i] = m_EnemyGeneratorData.Get(i).m_Difficulty;
        }
        ListUtility.Sort(ref m_EnemiesIndexesSortedByDifficulty, ref m_SortedDifficulties);

        m_MinEnemyDifficulty = m_SortedDifficulties[0];
        m_MaxEnemyDifficulty = m_SortedDifficulties[^1];

        m_SpawnQueue = new();
    }

    private void Reset()
    {
        m_Time = 0f;
        m_SurvivedPeriodsCount = 0;
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
            StopCoroutine(m_CurrentHandle);
        }
    }

    private IEnumerator HandleGeneratorHeartBeat()
    {
        while (true)
        {
            float targetDifficulty = ComputeTargetDifficulty();
            if (targetDifficulty > m_CurrentDifficulty)
                AdjustDifficulty(targetDifficulty - m_CurrentDifficulty);

            m_Time += m_DeltaTime;
            if (m_IsOpeningDone)
            {
                CheckForOverTime(m_PeriodDuration, out bool newPeriodStarted);
                if (newPeriodStarted) m_SurvivedPeriodsCount++;
            }
            else
            {
                CheckForOverTime(m_OpeningDuration, out m_IsOpeningDone);
            }

            yield return new WaitForSeconds(m_DeltaTime);
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
            if (iNeeded > m_MaxEnemyDifficulty)
            {
                int randomEnemyType = Random.Range(0, m_EnemiesIndexesSortedByDifficulty.Count);
                chosen = m_EnemyGeneratorData.Get(randomEnemyType);
            }
            else
            {
                int index = 0;
                do ++index;
                while (iNeeded > m_SortedDifficulties[index]);

                if (m_SortedDifficulties[index] - iNeeded < iNeeded - m_SortedDifficulties[index - 1])
                    --index;

                chosen = m_EnemyGeneratorData.Get(m_EnemiesIndexesSortedByDifficulty[index]);
            }
            m_SpawnQueue.Add(chosen.m_EnemyPrefab);
            m_CurrentDifficulty += chosen.m_Difficulty;
            iNeeded -= chosen.m_Difficulty;
        }
    }

    public void LowerCurrentDifficulty(int loweringTypeId)
    {
        m_CurrentDifficulty -= m_EnemyGeneratorData.Get(loweringTypeId).m_Difficulty;
    }
}
