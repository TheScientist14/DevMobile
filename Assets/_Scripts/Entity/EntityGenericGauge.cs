using System;
using NaughtyAttributes;
using UnityEngine;

public class EntityGauge : MonoBehaviour
{
    [BoxGroup("Gauge Config")]
    [SerializeField] protected float m_Max;
    [BoxGroup("Gauge Config")]
    [SerializeField] protected float m_Min;
    [BoxGroup("Gauge Config")]
    [SerializeField] protected float m_Start;

    private float m_Value;

    // void OnValueChanged(float iDelta)
    public event Action<float> OnValueChanged;
    protected event Action OnMinReached;
    protected event Action OnMaxReached;

    protected void Awake()
    {
        _SetValue(m_Start);
    }

    protected void UpdateValue(float iDelta)
    {
        SetValue(m_Value + iDelta);
        OnValueChanged?.Invoke(iDelta);
    }

    protected void SetValue(float iValue)
    {
        _SetValue(iValue);

        if (IsMinimal())
            OnMinReached?.Invoke();
        if (IsMaximal())
            OnMaxReached?.Invoke();
    }

    private void _SetValue(float iValue)
    {
        m_Value = Mathf.Clamp(iValue, m_Min, m_Max);
    }

    protected float GetValue()
    {
        return m_Value;
    }

    protected bool IsMinimal()
    {
        return m_Value <= m_Min + Mathf.Epsilon;
    }

    protected bool IsMaximal()
    {
        return m_Value >= m_Max - Mathf.Epsilon;
    }

    public float GetCompletion()
    {
        return (m_Value - m_Min) / (m_Max - m_Min);
    }
}
