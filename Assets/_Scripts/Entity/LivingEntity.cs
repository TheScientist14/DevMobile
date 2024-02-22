using System;

public class LivingEntity : EntityGauge
{
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

    public void Die()
    {
        if (IsDead()) return;
        SetValue(m_Min);
	}

    public bool IsDead()
    {
        return IsMinimal();
    }
}
