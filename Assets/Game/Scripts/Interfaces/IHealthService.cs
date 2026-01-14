using System;
using UnityEngine;

public interface IHealthService
{
    float Current { get; }
    float Max { get; }
    public void ModifyHealth(float amount);
    public void SetHealth(float health);
    public void Initialize(float current, float max);
    event Action<float> OnHealthChanged;
}