using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldRenderer : MonoBehaviour
{
    [SerializeField] private BlockDatabase blockDatabase;
    [Header("Maps")]
    [SerializeField] private Tilemap _backgroundMap;
    [SerializeField] private Tilemap _foregroundMap;
    [SerializeField] private Tilemap _mainMap;
    [SerializeField] private Tilemap _damageMap;

    public void RenderFullWorld(WorldData data) 
    {
        _mainMap.ClearAllTiles();
        _backgroundMap.ClearAllTiles();
        _foregroundMap.ClearAllTiles();

        for (int x = 0; x < data.Width; x++) 
        {
            for (int y = 0; y < data.Height; y++) 
            {
                TileData currentTile = data.TileArray[x, y];
                UpdateTile(x,y,currentTile, false);

                // TO ADD - LIGHT LEVEL
            }
        }

        _mainMap.GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
    public void UpdateTile(int x, int y, TileData newData, bool updateComposite) 
    {
        Vector3Int pos = new(x, y, 0);
        BlockData fg = blockDatabase.GetBlock(newData.Foreground);
        if (fg != null) _foregroundMap.SetTile(pos, fg.Tile);
        else _foregroundMap.SetTile(pos, null);

        BlockData bg = blockDatabase.GetBlock(newData.Background);
        if (bg != null) _backgroundMap.SetTile(pos, bg.Tile);
        else _backgroundMap.SetTile(pos, null);
        
        BlockData main = blockDatabase.GetBlock(newData.Main);
        if (main != null) _mainMap.SetTile(pos, main.Tile);
        else _mainMap.SetTile(pos, null);

        if(updateComposite) _mainMap.GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
}
