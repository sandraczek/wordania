using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Data;

[RequireComponent(typeof(WorldGenerator))]
public class WorldManager : MonoBehaviour
{
    private BlockDatabase _blockDatabase;
    public WorldData Data {get; private set;}
    public WorldSettings Settings;
    private WorldGenerator _generator;
    [SerializeField] private WorldRenderer _renderer;
    [SerializeField] private LootEvent _lootEvent;
    public void Awake()
    {
        _generator = GetComponent<WorldGenerator>();
        _renderer.Initialize(Settings);
    }
    public void Initialize(BlockDatabase database)
    {
        _blockDatabase = database;
    }
    public void StartWorldGeneration()
    {
        Debug.Assert(Data == null);
        Data = _generator.GenerateWorld(Settings);
        _renderer.CreateChunks();
        _renderer.RenderWorld(Data);
    }
    public void LoadWorldData(WorldData data)
    {
        Debug.Assert(Data == null);
        Data = data;
        _renderer.CreateChunks();
        _renderer.RenderWorld(Data);
    }
    public bool TryDamageBlock(Vector3 worldPosition, float damagePower)
    {
        Vector2Int pos = Settings.WorldToGrid(worldPosition);
        if (!IsWithinBounds(pos.x, pos.y)) return false;
        WorldLayer result = DamageTile(pos.x, pos.y, damagePower);
        if (result == WorldLayer.None) return false;

        Vector2Int coord = GetChunkCoord(pos.x, pos.y);
        _renderer.ChunkRefresh(coord, result);
        return true;
    }
    public WorldLayer DamageTile(int x, int y, float damagePower)
    {
        if(!IsWithinBounds(x,y)) return WorldLayer.None;
        BlockData data = _blockDatabase.GetBlock(Data.GetTile(x,y).Main);
        if(data == null) return WorldLayer.None;
        Data.GetTile(x,y).Damage += damagePower / data.Hardness;
        WorldLayer changedLayers;
        if(Data.GetTile(x,y).Damage >= 1f){
            Data.GetTile(x,y).Main = 0;
            Data.GetTile(x,y).Damage = 0f; 

            //DROPPING LOOT
            _lootEvent.Raise(data.loot, data.lootAmount);

            changedLayers = WorldLayer.Main | WorldLayer.Damage;
        }
        else
        {
            changedLayers = WorldLayer.Damage;
        }
        return changedLayers;
    }
    public bool TryDamageCircle(Vector2 worldPos, float radius, float damagePower)
{
    int minX = Mathf.FloorToInt(worldPos.x - radius);
    int maxX = Mathf.CeilToInt(worldPos.x + radius);
    int minY = Mathf.FloorToInt(worldPos.y - radius);
    int maxY = Mathf.CeilToInt(worldPos.y + radius);

    Dictionary<Vector2Int, WorldLayer> chunksToUpdate = new();
    
    bool hitAnything = false;

    for (int x = minX; x <= maxX; x++)
    {
        for (int y = minY; y <= maxY; y++)
        {
            if (!IsWithinBounds(x, y)) continue;

            float closestX = Mathf.Clamp(worldPos.x, x, x + 1f);
            float closestY = Mathf.Clamp(worldPos.y, y, y + 1f);
            float distSq = (worldPos.x - closestX) * (worldPos.x - closestX) + 
                           (worldPos.y - closestY) * (worldPos.y - closestY);

            if (distSq <= radius * radius)
            {
                WorldLayer result = DamageTile(x, y, damagePower);

                if (result != WorldLayer.None)
                {
                    hitAnything = true;
                    Vector2Int coord = GetChunkCoord(x, y);

                    if (!chunksToUpdate.ContainsKey(coord))
                        chunksToUpdate[coord] = result;
                    else
                        chunksToUpdate[coord] |= result;
                }
            }
        }
    }

    if (hitAnything)
    {
        foreach (var entry in chunksToUpdate)
        {
            _renderer.ChunkRefresh(entry.Key, entry.Value);
        }
    }

    return hitAnything;
}

    public bool TryPlaceBlock(Vector3 worldPosition, int blockID)
    {
        Vector2Int pos = Settings.WorldToGrid(worldPosition);
        if (!IsWithinBounds(pos.x, pos.y)) return false;
        if(_blockDatabase.GetBlock(Data.GetTile(pos.x,pos.y).Main) != null) return false;
        Vector3 cellCenter = Settings.GridToWorld(pos.x,pos.y);
        Vector2 checkSize = new(0.9f, 0.9f);

        Collider2D hit = Physics2D.OverlapBox(cellCenter, checkSize, 0f, Settings.PreventBuildingLayer);

        if (hit != null) return false;

        Data.GetTile(pos.x,pos.y).Main = blockID;
        Vector2Int coord = GetChunkCoord(pos.x, pos.y);
        _renderer.ChunkRefresh(coord, WorldLayer.Main);
        return true;
    }

    public TileBase GetTileBase(int x, int y, WorldLayer layer)
    {
        TileData data = Data.GetTile(x,y);
        int id = 0;

        if (layer == WorldLayer.Main) id = data.Main;
        else if (layer == WorldLayer.Background) id = data.Background;
        else if (layer == WorldLayer.Background) id = data.Foreground;
        else if (layer == WorldLayer.Damage) 
        {
            return _blockDatabase.GetCracks(data.Damage);
        }

        if (id == 0) return null;

        return _blockDatabase.GetBlock(id).Tile;
    }
    private bool IsWithinBounds(int x, int y)
    {
        return !(x >= Settings.Width || x < 0 || y >= Settings.Height || y < 0);
    }
    private Vector2Int GetChunkCoord(int x, int y)
    {
        int cx = x / Settings.ChunkSize;
        int cy = y / Settings.ChunkSize;
        return new Vector2Int(cx,cy);
    }
}