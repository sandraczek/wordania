using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }
    
    [Header("Maps")]
    [SerializeField] private Tilemap _backgroundMap;
    [SerializeField] private Tilemap _mainMap;
    [SerializeField] private Tilemap _damageMap;
    [SerializeField] private LayerMask _preventBuildingLayer;

    [SerializeField] private BlockDatabase blockDatabase;
    private Dictionary<TileBase, BlockData> _tileToDataMap;
    private Dictionary<Vector3Int, float> _damagedBlocks;

    [SerializeField] private TileBase cracks;

    private void Awake()
    {
        Instance = this;
        InitializeDataBase();
    }
    private void InitializeDataBase()
    {
        blockDatabase.Initialize();
        _tileToDataMap = new();
        _damagedBlocks = new();
        foreach (var block in blockDatabase.allBlocks)
        {
            if (block.Tile != null)
            {
                _tileToDataMap.Add(block.Tile, block);
            }
        }
    }

    public bool TryDamageBlock(Vector3 worldPosition, float damagePower)
    {
        Vector3Int gridPos = _mainMap.WorldToCell(worldPosition);
        TileBase tile = _mainMap.GetTile(gridPos);

        if (tile == null || !_tileToDataMap.ContainsKey(tile)) return false; // Uderzamy w powietrze

        BlockData data = _tileToDataMap[tile];
        Debug.Log(data.Hardness);
        float currentDamage = 0f;
        if (_damagedBlocks.ContainsKey(gridPos))
        {
            currentDamage = _damagedBlocks[gridPos];
        }
        currentDamage += damagePower;

        if(currentDamage >= data.Hardness){
            RemoveBlock(gridPos);   
        }
        else
        {

            if (_damagedBlocks.ContainsKey(gridPos))
            {
                _damagedBlocks[gridPos] = currentDamage;
            }
            else
            {
                _damagedBlocks.Add(gridPos, currentDamage);
            }

            _damageMap.SetTile(gridPos, cracks);   // adding cracking to tiles
            _damageMap.SetTileFlags(gridPos, TileFlags.None);
            _damageMap.SetColor(gridPos, new Color(1f, 1f, 1f, 0.5f));
        }



        return true;
    }


    public bool TryPlaceBlock(Vector3 worldPosition, TileBase tileToPlace)
    {
        Vector3Int gridPos = _mainMap.WorldToCell(worldPosition);
        if (_mainMap.GetTile(gridPos) != null) return false;
        Vector3 cellCenter = _mainMap.GetCellCenterWorld(gridPos);
        Vector2 checkSize = new Vector2(0.9f, 0.9f);

        Collider2D hit = Physics2D.OverlapBox(cellCenter, checkSize, 0f, _preventBuildingLayer);

        if (hit != null) return false;

        _mainMap.SetTile(gridPos, tileToPlace);
        return true;
    }

    private void RemoveBlock(Vector3Int pos)
    {
        _mainMap.SetTile(pos, null);
        _damageMap.SetTile(pos, null);

        if (_damagedBlocks.ContainsKey(pos))
        {
            _damagedBlocks.Remove(pos);
        }
    }
}