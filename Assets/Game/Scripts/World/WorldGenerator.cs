using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private WorldSettings _settings;

    public WorldData GenerateWorld()
    {
        Random.InitState(_settings.Seed); // setting seed for Randomness

        WorldData worldData = new(_settings.Width, _settings.Height);

        IWorldGenerationPass[] pipeline = {
            new WorldPassTerrain(),
            new WorldPassCave(),
        };

        foreach (var pass in pipeline) {
            pass.Execute(worldData, _settings);
        }
        return worldData;
    }
}
