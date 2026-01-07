using UnityEngine;

public class WorldPassTerrain : IWorldGenerationPass
{
    public void Execute(WorldData data, WorldSettings settings)
    {
        for (int x = 0; x < settings.Width; x++)
        {
            int terrainHeight = CalculateFractalHeight(x, settings);

            for (int y = 0; y < settings.Height; y++)
            {
                if (y < terrainHeight)
                {
                    data.GetTile(x,y).Main = 1;
                }
                else
                {
                    data.GetTile(x,y).Main = 0;
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
}