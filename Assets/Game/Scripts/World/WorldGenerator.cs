using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldData GenerateWorld(WorldSettings settings)
    {
        Random.InitState(settings.Seed); // setting seed for Randomness

        WorldData worldData = new(settings.Width, settings.Height);

        IWorldGenerationPass[] pipeline = {
            new WorldPassTerrain(),
            new WorldPassCave(),
        };

        foreach (var pass in pipeline) {
            pass.Execute(worldData, settings);
        }
        return worldData;
    }
}
