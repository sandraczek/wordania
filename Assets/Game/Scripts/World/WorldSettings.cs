using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings", menuName = "WorldSettings")]
public class WorldSettings : ScriptableObject
{
    public float TileSize = 1f;
    public int Width;
    public int Height;

    public int Seed;
    public int ChunkSize;

    [Header("Terrain")]
    public float HeightMultiplier; // = 50f;
    public int SurfaceOffset; // = half of height
    public float SurfaceFrequency; //  = 0.01f
    [Range(1, 8)]
    public int Octaves; // = 4
    public float Persistence; // = 0.5f
    public float Lacunarity; // = 2.0f

    [Header("Caves")]
    public float GlobalCaveDensity; // = 0.5f;
    public float MacroScale; // = 0.02f;
    public float MacroWeight; // = 0.6f;
    public float MicroScale; // = 0.12f;
    public float MicroWeight; // = 0.4f;
    public float TunnelScale; // = 0.04f;
    public float TunnelThreshold; // = 0.05f;
    public float CaveStartDepth; // = 0.8f;
    public float CaveFullDensityDepth; // = 0.6f;

    [Header("Other")]
    public LayerMask PreventBuildingLayer;
    public LayerMask GroundLayer;
    public string MainLayerName = "Main";
    public string BackgroundLayerName = "Background";
    public string DamageLayerName = "Damage";
    public string ForegroundLayerName = "Foreground";

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
