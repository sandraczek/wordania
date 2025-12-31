using NUnit.Framework.Constraints;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileData {
    public int Background;
    public int Main;
    public int Foreground;
    public float Damage;        // [0f - 1f]
    public byte LightLevel;
}
public class WorldData
{
    public Vector2Int SpawnPoint;
    public TileData[,] TileArray;
    public WorldData(int width, int height)
    {
        TileArray = new TileData[width,height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                TileArray[x, y].Foreground = 0; // 0 = Empty
                TileArray[x, y].Main = 0; // 0 = Empty
                TileArray[x, y].Background = 0; // 0 = Empty
                TileArray[x, y].Damage = 0;
            }
        }
    }
}
