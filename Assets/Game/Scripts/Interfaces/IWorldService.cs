using UnityEngine;
using UnityEngine.Tilemaps;

public interface IWorldService
{
    public void StartWorldGeneration();
    public bool TryDamageBlock(Vector3 worldPosition, float damagePower);
    public WorldLayer DamageTile(int x, int y, float damagePower);
    public bool TryDamageCircle(Vector2 worldPos, float radius, float damagePower);

    public bool TryPlaceBlock(Vector3 worldPosition, int blockID);

    public TileBase GetTileBase(int x, int y, WorldLayer layer);
}
