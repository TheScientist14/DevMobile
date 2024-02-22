using System;
using NaughtyAttributes;
using UnityEngine;

public class LivingEntity : EntityGauge, IRestartable
{
    [SerializeField] private VoidEvent m_DeathBroadcast;

    public event Action<float> OnDamageTaken;
    public event Action<float> OnHealGiven;

    public void RegisterOnDeathEvent(Action callback)
    {
        OnMinReached += callback;
    }

    public void UnregisterOnDeathEvent(Action callback)
    {
        OnMinReached -= callback;
    }

    public void TakeDamage(float iValue)
    {
        ChangeHealth(iValue, OnDamageTaken, false);
    }
    
    public void Heal(float iValue)
    {
        ChangeHealth(iValue, OnHealGiven, true);
    }

    private void ChangeHealth(float iDelta, Action<float> trigger, bool iIsPositive)
    {
        if (iDelta < 0) return;
        if (IsDead()) return;
        UpdateValue(iIsPositive ? iDelta : -iDelta);
        trigger?.Invoke(iDelta);
    }

    [Button]
    public void Die()
    {
        if (IsDead()) return;
        SetValue(m_Min);
	}

    public bool IsDead()
    {
        return IsMinimal();
    }

    private void OnEnable()
    {
        RegisterOnDeathEvent(NotifyDeathEvent);
    }

    private void OnDisable()
    {
        UnregisterOnDeathEvent(NotifyDeathEvent);
    }

    private void NotifyDeathEvent()
    {
        m_DeathBroadcast?.RequestRaiseEvent();
    }

    public void Restart()
    {
        SetValue(m_Start);
    }
}
