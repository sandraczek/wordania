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

    public void TakeDamage(float amount)
    {
        _healthProcessor.TakeDamage(amount);
        OnHurt?.Invoke();
    }

    private void HandleHealthChanged()
    {
        if (_healthProcessor.Current <= 0) OnDeath?.Invoke();
    }
}