using System;
using NUnit.Framework.Constraints;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileData {
    public int Background;
    public int Main;
    public int Foreground;
    [System.NonSerialized] public float Damage;        // [0f - 1f]
    //public byte LightLevel;
}
[System.Serializable]
public class WorldData
{
    public Vector2Int SpawnPoint;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] public TileData[] Tiles;
    public WorldData() {}
    public WorldData(int width, int height)
    {
        Tiles = new TileData[width * height];
        _width = width;
        _height = height;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Tiles[x + _width * y].Foreground = 0; // 0 = Empty
                Tiles[x + _width * y].Main = 0; // 0 = Empty
                Tiles[x + _width * y].Background = 0; // 0 = Empty
                Tiles[x + _width * y].Damage = 0;
            }
        }
    }

    public ref TileData GetTile(int x, int y)
    {
        Debug.Assert(_width != 0);
        return ref Tiles[x + y * _width];
    }
}
