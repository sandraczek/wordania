using UnityEngine;
using VContainer;
using System;
using VContainer.Unity;

using UnityEngine;
using VContainer.Unity;

public sealed class PlayerHealthProcessor : IPlayerHealth, ITickable
{
    private readonly PlayerConfig _config;
    private float _currentHealth;
    private float _invincibilityTimer;

    public float Current => _currentHealth;
    public float Max => _config.MaxHealth;
    public bool IsInvincible => _invincibilityTimer > 0;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnHurt;

    public PlayerHealthProcessor(PlayerConfig config)
    {
        _config = config;
        _currentHealth = 100f;//config.MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsInvincible || _currentHealth <= 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, Max);
        _invincibilityTimer = 0.8f;//_config.InvincibilityDuration;

        OnHealthChanged?.Invoke(_currentHealth);
        OnHurt?.Invoke();

        if (_currentHealth <= 0) OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        if (_currentHealth <= 0) return;
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, Max);
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void Tick()
    {
        if (_invincibilityTimer > 0)
        {
            _invincibilityTimer -= Time.deltaTime;
        }
    }
}