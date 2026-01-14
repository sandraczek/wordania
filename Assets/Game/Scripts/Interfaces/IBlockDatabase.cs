using UnityEngine;
using UnityEngine.Tilemaps;

public interface IBlockDatabase
{
    public BlockData GetBlock(int id);
    public TileBase GetCracks(float damage);
}
