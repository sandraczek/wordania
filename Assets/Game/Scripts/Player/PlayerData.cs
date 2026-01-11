using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public PlayerData(Vector2 pos, float health, float maxHealth)
    {
        Position = pos;
        Health = health;
        MaxHealth = maxHealth;
    }
    public Vector2 Position;
    public float Health;
    public float MaxHealth;
}
