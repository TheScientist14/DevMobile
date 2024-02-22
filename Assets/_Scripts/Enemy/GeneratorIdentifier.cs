using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class GeneratorIdentifier : MonoBehaviour
{
    private int m_TypeId;

    private LivingEntity m_LivingEntity;
    private EnemyGenerator m_Generator;

    private void Awake()
    {
        m_LivingEntity = GetComponent<LivingEntity>();
    }

    private void OnEnable()
    {
        m_LivingEntity.RegisterOnDeathEvent(OnDeath);
    }

    private void OnDisable()
    {
        m_LivingEntity.UnregisterOnDeathEvent(OnDeath);
    }

    private void OnDeath()
    {
        m_Generator.LowerCurrentDifficulty(m_TypeId);
    }

    public void Setup(EnemyGenerator iGenerator, int iType)
    {
        m_Generator = iGenerator;
        m_TypeId = iType;
    }
}
