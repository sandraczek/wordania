using UnityEngine;
using System;
using System.Collections;
using VContainer;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PlayerHealthView : MonoBehaviour, IDamageable
{
    private PlayerHealthProcessor _healthProcessor;
    
    public event Action OnHurt;
    public event Action OnDeath;

    [Inject]
    public void Construct(PlayerHealthProcessor healthProcessor, IHealthService healthService)
    {
        _healthProcessor = healthProcessor;
        
        healthService.OnHealthChanged += HandleHealthChanged;
    }

    private void Update()
    {
        _healthProcessor.UpdateTick(Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        _healthProcessor.ApplyDamage(amount);
        OnHurt?.Invoke();
    }

    private void HandleHealthChanged(float currentHealth)
    {
        if (currentHealth <= 0) OnDeath?.Invoke();
    }
}