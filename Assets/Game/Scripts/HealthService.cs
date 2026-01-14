using System;
using UnityEngine;

public class HealthService : IHealthService
{
    public float Current { get; private set; } = 100f;
    public float Max { get; private set; } = 100f;
    public event Action OnHealthChanged;

    public void ModifyHealth(float amount)
    {
        if (amount == 0) return;
        
        SetHealth(Current + amount);
    }
    public void SetHealth(float health)
    {
        float newHealth = Mathf.Clamp(health, 0, Max);
        
        if (Mathf.Approximately(Current, newHealth)) return;

        Current = newHealth;
        OnHealthChanged?.Invoke();
    }
    public void Initialize(float current, float max)
    {
        Max = max;
        Current = Mathf.Clamp(current, 0, Max);
        OnHealthChanged?.Invoke();
    }
}
