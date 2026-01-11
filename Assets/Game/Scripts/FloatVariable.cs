using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    [SerializeField] private float _value;

    public float Value
    {
        get => _value;
        set
        {
            if (Mathf.Approximately(_value, value)) return;
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public event Action<float> OnValueChanged;

    public void SetValue(float newValue) => Value = newValue;
    public void SetValue(FloatVariable variable) => Value = variable.Value;
}