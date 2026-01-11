using UnityEngine;
using System;
using System.Collections;

[SelectionBase]
[DisallowMultipleComponent]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthVariable _current;
    [SerializeField] private FloatVariable _max;

    [Header("Settings")]
    [SerializeField] private float _invincibilityDuration = 0.8f;
    
    public float Current => _current.Value;
    public float Max => _max.Value;

    public event Action OnDeath;
    public event Action OnHurt;

    private bool _isInvincible;
    private WaitForSeconds _invincibilityWait;

    private void Awake()
    {
        _invincibilityWait = new WaitForSeconds(_invincibilityDuration);
    }
    public void SetInitial(float current, float max)
    {
        _current.SetValue(current, DamageSource.INITIALIZE);
        _max.SetValue(max);
    }

    public void TakeDamage(float amount, int sourceID)
    {
        if (_isInvincible || _current.Value <= 0f) return;

        Debug.Assert(amount >= 0f);
        
        _current.SetValue(_current.Value - amount, sourceID);

        OnHurt?.Invoke();

        if (_current.Value <= 0)
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
        if (_current.Value <= 0) return;
        Debug.Assert(amount >= 0f);

        _current.SetValue(_current.Value + amount, DamageSource.HEAL);
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
}