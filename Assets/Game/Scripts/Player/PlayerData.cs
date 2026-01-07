using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector2 Position;

    public void SetPosition(Vector2 newPos)
    {
        Position = newPos;
    }
}
