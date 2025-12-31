using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Data;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }
    [SerializeField] private BlockDatabase _blockDatabase;
    private WorldData _worldData;
    [SerializeField] private WorldSettings _settings;
    [SerializeField] private WorldRenderer _renderer;
    private void Awake()
    {
        Instance = this;
    }
    public void Initialize(WorldData worldData)
    {
        _worldData = worldData;
    }

    public bool TryDamageBlock(Vector3 worldPosition, float damagePower)
    {
        Vector2Int pos = _settings.WorldToGrid(worldPosition);

        BlockData data = _blockDatabase.GetBlock(_worldData.TileArray[pos.x,pos.y].Main);
        if(data == null) return false;
        _worldData.TileArray[pos.x,pos.y].Damage += damagePower / data.Hardness;

        if(_worldData.TileArray[pos.x,pos.y].Damage >= 1){
            _worldData.TileArray[pos.x,pos.y].Main = 0;
            _worldData.TileArray[pos.x,pos.y].Damage = 0; 

            //DROPPING LOOT
        }
        _renderer.UpdateTile(pos.x,pos.y,_worldData.TileArray[pos.x,pos.y].Main, WorldLayer.Main);
        TileBase cracks = _blockDatabase.GetCracks(_worldData.TileArray[pos.x,pos.y].Damage);
        _renderer.UpdateTile(pos.x,pos.y,cracks, WorldLayer.Damage);
        return true;
    }


    public bool TryPlaceBlock(Vector3 worldPosition, BlockData block)
    {
        Vector2Int pos = _settings.WorldToGrid(worldPosition);
        if (_worldData.TileArray[pos.x,pos.y].Main != 0 || pos.x >= _settings.Width || pos.x < 0 || pos.y >= _settings.Height || pos.y < 0) return false;
        Vector3 cellCenter = _settings.GridToWorld(pos.x,pos.y);
        Vector2 checkSize = new(0.9f, 0.9f);

        Collider2D hit = Physics2D.OverlapBox(cellCenter, checkSize, 0f, _settings.PreventBuildingLayer);

        if (hit != null) return false;

        _worldData.TileArray[pos.x,pos.y].Main = block.ID;
        _renderer.UpdateTile(pos.x,pos.y,_worldData.TileArray[pos.x,pos.y].Main, WorldLayer.Main);
        return true;
    }
}