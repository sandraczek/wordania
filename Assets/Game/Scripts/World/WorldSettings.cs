using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings", menuName = "WorldSettings")]
public class WorldSettings : ScriptableObject
{
    public float TileSize = 1f;
    public int Width;
    public int Height;

    public int Seed;

    [Header("Terrain Base")]
    public float HeightMultiplier; // = 50f;
    public int SurfaceOffset; // = half of height
    public float SurfaceFrequency; //  = 0.01f

    [Header("Fractal Details")]
    [Range(1, 8)]
    public int Octaves; // = 4
    public float Persistence; // = 0.5f
    public float Lacunarity; // = 2.0f

    [Header("Other")]
    public LayerMask PreventBuildingLayer;

    public Vector2Int WorldToGrid(Vector3 worldPos) 
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / TileSize),
            Mathf.FloorToInt(worldPos.y / TileSize)
        );
    }

    public Vector3 GridToWorld(int x, int y) 
    {
        return new Vector3(
            (x * TileSize) + (TileSize * 0.5f),
            (y * TileSize) + (TileSize * 0.5f),
            0
        );
    }
}
