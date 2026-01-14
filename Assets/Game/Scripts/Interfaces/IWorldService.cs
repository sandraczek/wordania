using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public interface IWorldService
{
    public event Action<Vector2Int, WorldLayer> OnBlockChanged;
    public event Action OnWorldGenerated;
    public void StartWorldGeneration();
    public bool TryDamageBlock(Vector3 worldPosition, float damagePower);
    public WorldLayer DamageTile(int x, int y, float damagePower);
    public bool TryDamageCircle(Vector2 worldPos, float radius, float damagePower);

    public bool TryPlaceBlock(Vector3 worldPosition, int blockID);

    public TileBase GetTileBase(int x, int y, WorldLayer layer);
}
