using UnityEngine;

public class WorldPassCave : IWorldGenerationPass
{
    int airId = 0;
    public void Execute(WorldData data, WorldSettings settings)
    {
        for (int x = 0; x < settings.Width; x++)
        {
            for (int y = 0; y < settings.Height; y++)
            {
                float currentDepth = (float)y / settings.Height;
                float depthMask = Mathf.InverseLerp(settings.CaveStartDepth, settings.CaveFullDensityDepth, currentDepth);

                float macroNoise = GetNoise(x, y, settings.Seed, settings.MacroScale);
                float microNoise = GetNoise(x, y, settings.Seed + 1, settings.MicroScale);
                
                float combinedNoise = (macroNoise * settings.MacroWeight) + (microNoise * settings.MicroWeight);
                combinedNoise *= depthMask;

                if (combinedNoise > settings.GlobalCaveDensity)
                {
                    if (data.GetTile(x,y).Main != airId)
                    {
                        data.GetTile(x,y).Main = airId;
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
