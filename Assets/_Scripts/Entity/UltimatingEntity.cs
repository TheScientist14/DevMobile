using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class UltimatingEntity : EntityGauge, IRestartable
{
    [SerializeField] private VoidEvent m_UltimateBroadcast;

    [SerializeField] private float m_ActiveUltimateConsumption = 1.0f;
    [SerializeField] private float m_ConsumptionRate = 1.0f;

    [SerializeField] private float m_LifeToUltimateConversion = 1.0f;
    [SerializeField] private Transform m_Ultimate;
    private LivingEntity m_LivingEntity;

    private bool m_IsActive;

    private new void Awake()
    {
        base.Awake();
        m_Ultimate.gameObject.SetActive(false);
        m_LivingEntity = GetComponent<LivingEntity>();
    }

    private void OnEnable()
    {
        m_LivingEntity.OnDamageTaken += OnDamaged;
        OnMinReached += Deactivate;
        OnMaxReached += Activate;
    }

    private void OnDisable()
    {
        Deactivate();
        m_LivingEntity.OnDamageTaken -= OnDamaged;
    }

    private void OnDamaged(float iDamageDealed)
    {
        UpdateValue(iDamageDealed * m_LifeToUltimateConversion);
    }

    private void Activate()
    {
        if (!m_IsActive)
        {
            m_IsActive = true;
            StartCoroutine(HandleUltimateConsumption());
            m_Ultimate.gameObject.SetActive(true);
            m_UltimateBroadcast?.RequestRaiseEvent();
        }
    }

    private void Deactivate()
    {
        if (m_IsActive)
        {
            m_IsActive = false;
            m_Ultimate.gameObject.SetActive(false);
        }
    }

    private IEnumerator HandleUltimateConsumption()
    {
        while (!IsMinimal())
        {
            UpdateValue(-m_ActiveUltimateConsumption);
            yield return new WaitForSeconds(1.0f / m_ConsumptionRate);
        }
        Deactivate();
    }

    public void Restart()
    {
        SetValue(m_Start);
        Deactivate();
    }
}
