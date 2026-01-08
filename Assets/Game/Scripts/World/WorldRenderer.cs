using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldRenderer : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private BlockDatabase _blockDatabase;
    private WorldSettings _settings;
    [SerializeField] private WorldManager _manager;

    private Chunk[,] _chunks;
    public void Initialize(WorldSettings settings)
    {
        _settings = settings;
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
                Vector3 pos = new(x * _settings.ChunkSize, y * _settings.ChunkSize, 0);
                GameObject go = Instantiate(_chunkPrefab, pos, Quaternion.identity, transform);
                _chunks[x, y] = go.GetComponent<Chunk>();
                go.name = $"Chunk_{x}_{y}";
                _chunks[x, y].Initialize(_settings, _manager, new Vector2Int(x,y));
            }
        }
    }
    public void RenderWorld()
    {
        foreach (Chunk chunk in _chunks)
        {
            chunk.Refresh(WorldLayer.Main | WorldLayer.Background | WorldLayer.Foreground);
        }
    }
    public void ChunkRefresh(Vector2Int pos, WorldLayer layer)
    {
        Chunk chunk = _chunks[pos.x,pos.y];
        chunk.Refresh(layer);
    }

}
