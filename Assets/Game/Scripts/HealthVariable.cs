using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewHealthVariable", menuName = "Variables/Health")]
public class HealthVariable : ScriptableObject
{
    [SerializeField] private float _value;
    
    // Event, który przenosi pełny kontekst zmiany
    public event Action<HealthChangeContext> OnHealthChangedWithContext;

    public float Value => _value;

    public void SetValue(float newValue, int sourceID)
    {
        float delta = newValue - _value;
        if (Mathf.Approximately(delta, 0f)) return;

        _value = newValue;
        
        // Rozgłaszamy zmianę z sourceID
        OnHealthChangedWithContext?.Invoke(new HealthChangeContext(_value, delta, sourceID));
    }
    public static implicit operator float(HealthVariable variable) => variable.Value;
}

public readonly struct HealthChangeContext
{
    public readonly float NewValue;
    public readonly float Delta;
    public readonly int SourceID;

    public HealthChangeContext(float newValue, float delta, int sourceID)
    {
        NewValue = newValue;
        Delta = delta;
        SourceID = sourceID;
    }
}