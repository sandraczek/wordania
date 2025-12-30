using UnityEngine;

public interface IWorldGenerationPass 
{
    void Execute(WorldData data, WorldSettings settings);
}
