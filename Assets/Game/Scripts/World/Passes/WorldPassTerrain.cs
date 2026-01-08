using System.Data;
using System.Data.Common;
using UnityEngine;

public class WorldPassTerrain : IWorldGenerationPass
{
    public void Execute(WorldData data, WorldSettings settings)
    {
        int airId = 0;
        int grassId = 1;
        int grassWallId = 1001;
        int dirtId = 2;
        int dirtWallId = 1002;
        int stoneId = 3;
        int stoneWallId = 1003;
        for (int x = 0; x < settings.Width; x++)
        {
            int terrainHeight = CalculateFractalHeight(x, settings);

            int stoneHeight = CalculateStoneHeight(x, terrainHeight, settings);

            float wallNoiseValue = Mathf.PerlinNoise((x + settings.Seed + 6767) * settings.dirt_wall_Terrain_Scale, 0);
            int wallOffset = (int)((wallNoiseValue - 0.5f) * settings.dirt_wall_Terrain_Amplitude);
            int wallTerrainHeight = terrainHeight + wallOffset;


            for (int y = 0; y < settings.Height; y++)
            {
                if (y > terrainHeight) {
                    data.GetTile(x, y).Main = airId;
                } else if (y >= terrainHeight -1) {
                    data.GetTile(x, y).Main = grassId;
                } else if (y < stoneHeight - settings.dirt_stoneTransitionMargin) {
                    data.GetTile(x, y).Main = stoneId;
                } else if (y > stoneHeight + settings.dirt_stoneTransitionMargin) {
                    data.GetTile(x, y).Main = dirtId;
                } else {
                    float stoneChance = Mathf.InverseLerp(stoneHeight + settings.dirt_stoneTransitionMargin, stoneHeight - settings.dirt_stoneTransitionMargin, y);
                    data.GetTile(x, y).Main = (Random.value < stoneChance) ? stoneId : dirtId;
                }

                if (y > wallTerrainHeight) {
                    data.GetTile(x, y).Background = airId;
                } else if (y >= wallTerrainHeight -1 || y > terrainHeight) {
                    data.GetTile(x,y).Background = grassWallId;
                } else if (y < stoneHeight - settings.dirt_stoneTransitionMargin) {
                    data.GetTile(x,y).Background = stoneWallId;
                } else if (y > stoneHeight + settings.dirt_stoneTransitionMargin) {
                    data.GetTile(x,y).Background = dirtWallId;
                } else {
                    float stoneChance = Mathf.InverseLerp(stoneHeight + settings.dirt_stoneTransitionMargin, stoneHeight - settings.dirt_stoneTransitionMargin, y);
                    data.GetTile(x, y).Background = (Random.value < stoneChance) ? stoneWallId : dirtWallId;
                }
            }
        }

        int centerX = settings.Width / 2;
        int groundY = CalculateFractalHeight(centerX, settings); 
        data.SpawnPoint = new Vector2Int(centerX, groundY + 2);
    }

    private int CalculateFractalHeight(int x, WorldSettings settings)
    {
        float total = 0;
        float frequency = settings.SurfaceFrequency;
        float amplitude = 1f;
        float maxValue = 0;

        for (int i = 0; i < settings.Octaves; i++)
        {
            float sampleX = (x + settings.Seed) * frequency + (i * 1000);
            float noiseValue = Mathf.PerlinNoise(sampleX, 0);

            total += noiseValue * amplitude;
            maxValue += amplitude;

            amplitude *= settings.Persistence;
            frequency *= settings.Lacunarity;
        }

        float normalizedHeight = total / maxValue;
        int finalHeight = Mathf.FloorToInt(normalizedHeight * settings.HeightMultiplier) + settings.SurfaceOffset;

        return Mathf.Clamp(finalHeight, 0, settings.Height - 1);
    }

    private int CalculateStoneHeight(int x, int terrainHeight, WorldSettings settings)
{
    float baseStoneLevel = terrainHeight - settings.MinDirtLayerDepth;

    float variation = Mathf.PerlinNoise((x + settings.Seed + 123) * settings.StoneNoiseScale, 0);
    
    float offset = (variation - 0.5f) * settings.StoneNoiseAmplitude;

    return Mathf.RoundToInt(baseStoneLevel + offset);
}
}