using System;
using UnityEngine;

[Serializable]
public class FloatReference
{
    [SerializeField] private bool _useConstant = true;
    [SerializeField] private float _constantValue;
    [SerializeField] private FloatVariable _variable;

    public float Value => _useConstant ? _constantValue : _variable.Value;

    public FloatVariable Variable => _variable;
    public bool IsVariable => !_useConstant && _variable != null;

    public static implicit operator float(FloatReference reference) => reference.Value;
}