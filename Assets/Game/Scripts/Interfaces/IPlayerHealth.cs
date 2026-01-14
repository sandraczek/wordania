using System;

public interface IPlayerHealth
{
    float Current { get; }
    float Max { get; }
    float Ratio => Current / Max;
    bool IsInvincible { get; }

    event Action<float> OnHealthChanged;
    event Action OnDeath;
    event Action OnHurt;

    void TakeDamage(float amount);
    void Heal(float amount);
}