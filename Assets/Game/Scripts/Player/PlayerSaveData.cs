using System;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public PlayerSaveData(Vector2 pos)
    {
        Position = pos;
    }
    public Vector2 Position;
}
