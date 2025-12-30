using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public TilemapRenderer Renderer { get; private set; }
    public TilemapCollider2D Collider { get; private set; }

    public void Initialize()
    {
        Tilemap = GetComponent<Tilemap>();
        Renderer = GetComponent<TilemapRenderer>();
        Collider = GetComponent<TilemapCollider2D>();
    }

    public void SetTile(int lx, int ly, TileBase tile)
    {
        Tilemap.SetTile(new Vector3Int(lx, ly, 0), tile);
    }

    public void RefreshTile(int lx, int ly)
    {
        Tilemap.RefreshTile(new Vector3Int(lx, ly, 0));
    }
}
