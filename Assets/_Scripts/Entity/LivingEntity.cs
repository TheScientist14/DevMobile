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
        if (iValue < 0) return;
        ChangeHealth(-iValue, OnDamageTaken);
    }
    
    public void Heal(float iValue)
    {
        if (iValue < 0) return;
        ChangeHealth(iValue, OnHealGiven);
    }

    private void ChangeHealth(float iDelta, Action<float> trigger)
    {
        if (IsDead()) return;
        UpdateValue(iDelta);
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
