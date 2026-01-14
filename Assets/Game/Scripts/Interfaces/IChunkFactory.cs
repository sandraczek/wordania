using UnityEngine;
public interface IChunkFactory
{
    Chunk Create(Vector2Int coord, Transform parent);
}