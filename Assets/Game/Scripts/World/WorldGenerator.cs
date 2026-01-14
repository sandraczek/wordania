using UnityEngine;
using System.Collections.Generic;

public class WorldGenerator : IWorldGenerator
{
    private readonly WorldSettings _settings;
    private readonly IEnumerable<IWorldGenerationPass> _generationPipeline;
    public WorldGenerator(WorldSettings settings, IEnumerable<IWorldGenerationPass> pipeline)
    {
        _settings = settings;
        _generationPipeline = pipeline;
    }
    public WorldData GenerateWorld()
    {
        Random.InitState(_settings.Seed); // setting seed for Randomness

        WorldData worldData = new(_settings.Width, _settings.Height);

        foreach (var pass in _generationPipeline) 
        {
            pass.Execute(worldData);
        }

        return worldData;
    }
}
    
