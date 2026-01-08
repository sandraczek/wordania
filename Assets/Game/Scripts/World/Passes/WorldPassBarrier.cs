using UnityEngine;

public class WorldPassBarrier : IWorldGenerationPass
{
    public void Execute(WorldData data, WorldSettings settings)
    {
        int barrierId = -1;
        for (int x = 0; x < settings.Width; x++)
        {
            data.GetTile(x,0).Main = barrierId;
            data.GetTile(x,settings.Height -1).Main = barrierId;
        }
        for (int y = 0; y < settings.Height; y++)
        {
            data.GetTile(0,y).Main = barrierId;
            data.GetTile(settings.Width -1, y).Main = barrierId;
        }
    }
}
