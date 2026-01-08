using UnityEngine;

public class WorldPassVariations : IWorldGenerationPass
{
    public void Execute(WorldData data, WorldSettings settings)
    {
        int stoneId = 3;
        int dirtId = 2;
        int graniteId = 4;
        for (int x = 0; x < settings.Width; x++)
        {
            for (int y = 0; y < settings.Height; y++)
            {
                float graniteNoise = GetNoise(x, y, settings.Seed + 2, settings.GraniteScale);
                float graniteValue = Mathf.Abs(graniteNoise - 0.5f);

                if (graniteValue < settings.GraniteThreshold)
                {
                    if (data.GetTile(x,y).Main == stoneId)
                    {
                        data.GetTile(x,y).Main = graniteId;
                        continue;
                    }
                }
                float stoneNoise = GetNoise(x, y, settings.Seed + 2, settings.DirtStoneScale);
                float stoneValue = Mathf.Abs(stoneNoise - 0.5f);

                if (stoneValue > settings.DirtStoneThreshold)
                {
                    if (data.GetTile(x,y).Main == dirtId)
                    {
                        data.GetTile(x,y).Main = stoneId;
                        continue;
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
