using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; } // Singleton dla ułatwienia

    [Header("Maps")]
    [SerializeField] private Tilemap _mainMap;
    [SerializeField] private Tilemap _backgroundMap;
    [SerializeField] private LayerMask _preventBuildingLayer;

    // Słownik do szybkiego wyszukiwania danych o klocku na podstawie TileBase
    private Dictionary<TileBase, BlockData> _tileToDataMap;
    private Dictionary<Vector3Int, float> _damagedBlocks;

    private void Awake()
    {
        Instance = this;
        InitializeDataBase();
    }
    private void InitializeDataBase()
    {
        _tileToDataMap = new();
        _damagedBlocks = new();
        BlockData[] allBlocks = Resources.LoadAll<BlockData>("Data/Blocks");

        foreach (var block in allBlocks)
        {
            if (block.Tile != null)
            {
                _tileToDataMap.Add(block.Tile, block);
            }
        }
    }

    // Metoda do niszczenia terenu (kopanie)
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
            // Jeśli nie zniszczony, zapisz nowy stan uszkodzeń
            if (_damagedBlocks.ContainsKey(gridPos))
            {
                _damagedBlocks[gridPos] = currentDamage;
            }
            else
            {
                _damagedBlocks.Add(gridPos, currentDamage);
            }
        }

        return true;
    }

    // Metoda do stawiania terenu
    public bool TryPlaceBlock(Vector3 worldPosition, TileBase tileToPlace)
    {
        Vector3Int gridPos = _mainMap.WorldToCell(worldPosition);
        
        // Sprawdzamy czy miejsce jest puste
        if (_mainMap.GetTile(gridPos) != null) return false;

        Vector3 cellCenter = _mainMap.GetCellCenterWorld(gridPos);
    
        // Rozmiar pudełka testowego. Dajemy troszkę mniej niż 1 (np. 0.9f),
        // żeby można było budować "na styk" przy graczu, bez irytowania go.
        Vector2 checkSize = new Vector2(0.9f, 0.9f);

        Collider2D hit = Physics2D.OverlapBox(cellCenter, checkSize, 0f, _preventBuildingLayer);

        if (hit != null) return false;

        _mainMap.SetTile(gridPos, tileToPlace);
        return true;
    }

    private void RemoveBlock(Vector3Int pos)
    {
        _mainMap.SetTile(pos, null);

        if (_damagedBlocks.ContainsKey(pos))
        {
            _damagedBlocks.Remove(pos);
        }
    }
}