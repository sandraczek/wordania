using UnityEngine;

public class WorldPassCave : IWorldGenerationPass
{
    public void Execute(WorldData data, WorldSettings settings)
    {
        for (int x = 0; x < data.Width; x++)
        {
            for (int y = 0; y < data.Height; y++)
            {
                float currentDepth = (float)y / data.Height;
                float depthMask = Mathf.InverseLerp(settings.CaveStartDepth, settings.CaveFullDensityDepth, currentDepth);

                float macroNoise = GetNoise(x, y, settings.Seed, settings.MacroScale);
                float microNoise = GetNoise(x, y, settings.Seed + 1, settings.MicroScale);
                
                float combinedNoise = (macroNoise * settings.MacroWeight) + (microNoise * settings.MicroWeight);
                combinedNoise *= depthMask;

                float tunnelNoise = GetNoise(x, y, settings.Seed + 2, settings.TunnelScale);
                float tunnelValue = Mathf.Abs(tunnelNoise - 0.5f);

                if (combinedNoise > settings.GlobalCaveDensity || tunnelValue < settings.TunnelThreshold * depthMask)
                {
                    if (data.TileArray[x, y].Main != 0)
                    {
                        // data.TileArray[x, y].Background = 2;
                        data.TileArray[x, y].Main = 0;
                    }
                }
            }
        }
    }

    private float GetNoise(int x, int y, int seed, float scale)
    {
        return Mathf.PerlinNoise((x + seed) * scale, (y + seed) * scale);
    }
}
