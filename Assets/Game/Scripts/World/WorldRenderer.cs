using UnityEditor;
using UnityEngine;

public class WorldRenderer : IWorldRenderer
{
    [Header("Dependencies")]
    private readonly IBlockDatabase _blockDatabase;
    private readonly WorldSettings _settings;
    private readonly IWorldService _world;
    private readonly Transform _chunksParent;
    private readonly IChunkFactory _factory;

    [Header("Data")]        
    private Chunk[,] _chunks;
    public WorldRenderer(
        IBlockDatabase blockDatabase,
        WorldSettings settings,
        IWorldService worldService,
        WorldChunksRoot chunksParent,
        IChunkFactory factory
        )
    {
        _blockDatabase = blockDatabase;
        _settings = settings;
        _world = worldService;
        _chunksParent = chunksParent.transform;
        _factory = factory;

        _world.OnBlockChanged += ChunkRefresh;
        _world.OnWorldGenerated += HandleWorldGenerated;
    }

    public void CreateChunks()
    {
        int chunksX = Mathf.CeilToInt((float)_settings.Width / _settings.ChunkSize);
        int chunksY = Mathf.CeilToInt((float)_settings.Height / _settings.ChunkSize);
        _chunks = new Chunk[chunksX, chunksY];

        for (int x = 0; x < chunksX; x++)
        {
            for (int y = 0; y < chunksY; y++)
            {
                _chunks[x, y] = _factory.Create(new Vector2Int(x,y), _chunksParent);
            }
        }
    }
    private void RenderWorld()
    {
        foreach (Chunk chunk in _chunks)
        {
            chunk.Refresh(WorldLayer.Main | WorldLayer.Background | WorldLayer.Foreground);
        }
    }
    private void ChunkRefresh(Vector2Int pos, WorldLayer layer)
    {
        Chunk chunk = _chunks[pos.x,pos.y];
        chunk.Refresh(layer);
    }
    
    private void HandleWorldGenerated()
    {
        CreateChunks();
        RenderWorld();
    }
}
