using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    private WorldSettings _settings;
    private WorldManager _manager;
    [Header("Tilemap Layers")]
    private Tilemap BackgroundMap;
    private Tilemap MainMap;
    private Tilemap ForegroundMap;
    private Tilemap DamageMap;
    private Vector2Int _chunkCoord;

    private Vector3Int[] _positionsCache;
    private TileBase[] _tilesCache;

    public void Initialize(WorldSettings settings, WorldManager manager, Vector2Int coord)
    {
        _settings = settings;
        _manager = manager;
        BackgroundMap = transform.Find(_settings.BackgroundLayerName).GetComponent<Tilemap>();
        MainMap = transform.Find(_settings.MainLayerName).GetComponent<Tilemap>();
        ForegroundMap = transform.Find(_settings.ForegroundLayerName).GetComponent<Tilemap>();
        DamageMap = transform.Find(_settings.DamageLayerName).GetComponent<Tilemap>();
        MainMap.gameObject.layer = 3;
        _chunkCoord = coord;
        int totalTiles = _settings.ChunkSize * _settings.ChunkSize;
        _positionsCache = new Vector3Int[totalTiles];
        _tilesCache = new TileBase[totalTiles];

        int index = 0;
        for (int x = 0; x < _settings.ChunkSize; x++)
        {
            for (int y = 0; y < _settings.ChunkSize; y++)
            {
                _positionsCache[index] = new Vector3Int(x, y, 0);
                index++;
            }
        }
    }
    private void UpdateLayer(Tilemap targetMap, WorldLayer layerType)
    {
        int index = 0;

        for (int x = 0; x < _settings.ChunkSize; x++)
        {
            for (int y = 0; y < _settings.ChunkSize; y++)
            {
                int worldX = _chunkCoord.x * _settings.ChunkSize + x;
                int worldY = _chunkCoord.y * _settings.ChunkSize + y;

                _tilesCache[index] = _manager.GetTileBase(worldX, worldY, layerType);
                index++;
            }
        }

        targetMap.SetTiles(_positionsCache, _tilesCache);
    }
    public void Refresh(WorldLayer layer)
    {
        if ((layer & WorldLayer.Main) != 0)
            UpdateLayer(MainMap, WorldLayer.Main);

        if ((layer & WorldLayer.Background) != 0)
            UpdateLayer(BackgroundMap, WorldLayer.Background);

        if ((layer & WorldLayer.Foreground) != 0)
            UpdateLayer(ForegroundMap, WorldLayer.Foreground);

        if ((layer & WorldLayer.Damage) != 0)
            UpdateLayer(DamageMap, WorldLayer.Damage);
    }
    public void Clear()
    {
        MainMap.ClearAllTiles();
        BackgroundMap.ClearAllTiles();
        ForegroundMap.ClearAllTiles();
        DamageMap.ClearAllTiles();
    }
    public int GetChunkSize()
    {
        return _settings.ChunkSize;
    }
}

[System.Flags]
public enum WorldLayer {
    None = 0,
    Main = 1 << 0,
    Background = 1 << 1,
    Damage = 1 << 2,     
    Foreground = 1 << 3,     
    All = ~0             
}
