using UnityEngine;

public interface IWorldRenderer
{
    public void CreateChunks();
    public void RenderWorld();
    public void ChunkRefresh(Vector2Int pos, WorldLayer layer);
}
