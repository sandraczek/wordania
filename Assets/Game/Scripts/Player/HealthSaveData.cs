using System;
using UnityEngine;

[Serializable]
public class HealthSaveData
{
    public HealthSaveData(float health, float maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }
    public float Health;
    public float MaxHealth;
}
