using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldRenderer : MonoBehaviour
{
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private WorldSettings _settings;

    private Chunk[,] _chunks;

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
                _chunks[x, y].Initialize(_settings);
            }
        }
    }
    public void RenderWorld(WorldData data)
    {
        int chunkSize = _settings.ChunkSize;
        
        for (int cx = 0; cx < _chunks.GetLength(0); cx++)
        {
            for (int cy = 0; cy < _chunks.GetLength(1); cy++)
            {
                TileBase[] chunkTiles = new TileBase[chunkSize * chunkSize];
                
                for (int lx = 0; lx < chunkSize; lx++)
                {
                    for (int ly = 0; ly < chunkSize; ly++)
                    {
                        int worldX = cx * chunkSize + lx;
                        int worldY = cy * chunkSize + ly;

                        if (worldX < _settings.Width && worldY < _settings.Height)
                        {
                            int id = data.TileArray[worldX, worldY].Main;
                            if (id != 0)
                            {
                                chunkTiles[lx + ly * chunkSize] = _blockDatabase.GetBlock(id).Tile;
                            }
                            else
                            {
                                chunkTiles[lx + ly * chunkSize] = null;
                            }
                        }
                    }
                }

                BoundsInt area = new(0, 0, 0, chunkSize, chunkSize, 1);
                _chunks[cx, cy].SetTilesBatch(chunkTiles, area);
            }
        }
        // ADD RENDERING FOR OTHER MAPS
    }
    public void UpdateTile(int wx, int wy, int id, WorldLayer layer)
    {
        int cx = wx / _settings.ChunkSize;
        int cy = wy / _settings.ChunkSize;
        int lx = wx % _settings.ChunkSize;
        int ly = wy % _settings.ChunkSize;

        if (cx < 0 || cy < 0 || cx >= _chunks.GetLength(0) || cy >= _chunks.GetLength(1)) return;

        BlockData Block = _blockDatabase.GetBlock(id);
        if(Block != null) _chunks[cx, cy].SetTile(lx, ly, Block.Tile, layer);
        else _chunks[cx, cy].SetTile(lx, ly, null, layer);
    }
    public void UpdateTile(int wx, int wy, TileBase tile, WorldLayer layer)
    {
        int cx = wx / _settings.ChunkSize;
        int cy = wy / _settings.ChunkSize;
        int lx = wx % _settings.ChunkSize;
        int ly = wy % _settings.ChunkSize;

        if (cx < 0 || cy < 0 || cx >= _chunks.GetLength(0) || cy >= _chunks.GetLength(1)) return;

        _chunks[cx, cy].SetTile(lx, ly, tile, layer);
    }
}
