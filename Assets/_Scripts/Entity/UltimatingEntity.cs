using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class UltimatingEntity : EntityGauge
{
    [SerializeField] private float m_LifeToUltimateConversion = 1.0f;
    [SerializeField] private Transform m_Ultimate;
    private LivingEntity m_LivingEntity;

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
        m_LivingEntity.OnDamageTaken -= OnDamaged;
    }

    private void OnDamaged(float iDamageDealed)
    {
        UpdateValue(iDamageDealed * m_LifeToUltimateConversion);
    }

    private void Activate()
    {
        SetUltimateState(true);
    }

    private void Deactivate()
    {
        SetUltimateState(false);
    }

    // This may become a state machine to handle better animations (Load/Active/Overheat/Inactive)
    // For now, this is just a toggle : iState = true/false => active/inactive
    private void SetUltimateState(bool iState)
    {
        m_Ultimate.gameObject.SetActive(iState);
    }
}
