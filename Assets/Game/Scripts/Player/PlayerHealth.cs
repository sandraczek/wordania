using UnityEngine;
using System;
using System.Collections;

[SelectionBase]
[DisallowMultipleComponent]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Data Persistence")]
    private Player _player;

    [Header("Settings")]
    [SerializeField] private float _invincibilityDuration = 0.8f;
    
    [SerializeField] public float Current {get; private set;}
    public float Max => _player.Data.MaxHealth;

    public event Action<HealthUpdateArgs> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnHurt;

    private bool _isInvincible;
    private WaitForSeconds _invincibilityWait;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _invincibilityWait = new WaitForSeconds(_invincibilityDuration);
    }

    public void Initialize(float startHP)
    {
        Current = startHP;
    }

    public void TakeDamage(float amount, int sourceID)
    {
        if (_isInvincible || Current <= 0f) return;

        Debug.Assert(amount >= 0f);
        
        Current -= amount;
        Current = Mathf.Clamp(Current, 0f, Max);

        _player.Data.Health = Current;

        OnHurt?.Invoke();
        NotifyHealthChange(sourceID);

        if (Current <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void Heal(float amount)
    {
        if (Current <= 0) return;
        Debug.Assert(amount >= 0f);

        Current = Mathf.Min(Current + Mathf.Abs(amount), Max);
        _player.Data.Health = Current;
        NotifyHealthChange(DamageSource.HEAL);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log("<color=red>Player Perished!</color>");
    }

    private IEnumerator InvincibilityRoutine()
    {
        _isInvincible = true;
        yield return _invincibilityWait;
        _isInvincible = false;
    }

    private void NotifyHealthChange(int sourceID)
    {
        var args = new HealthUpdateArgs 
        { 
            current = Current, 
            max = Max, 
            sourceID = sourceID 
        };
        OnHealthChanged?.Invoke(args);
    }
}

public struct HealthUpdateArgs
{
    public float current;
    public float max;
    public int sourceID;
}