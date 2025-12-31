using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    private WorldSettings _settings;
    [Header("Tilemap Layers")]
    private Tilemap BackgroundMap;
    private Tilemap MainMap;
    private Tilemap ForegroundMap;
    private Tilemap DamageMap;

    public void Initialize(WorldSettings settings)
    {
        _settings = settings;
        BackgroundMap = transform.Find(_settings.BackgroundLayerName).GetComponent<Tilemap>();
        MainMap = transform.Find(_settings.MainLayerName).GetComponent<Tilemap>();
        ForegroundMap = transform.Find(_settings.ForegroundLayerName).GetComponent<Tilemap>();
        DamageMap = transform.Find(_settings.DamageLayerName).GetComponent<Tilemap>();
        MainMap.gameObject.layer = 3;
    }

    public void SetTile(int lx, int ly, TileBase tile, WorldLayer layer)
    {
        Tilemap targetMap = GetMapByLayer(layer);
        if (targetMap != null)
        {
            targetMap.SetTile(new Vector3Int(lx, ly, 0), tile);
        }
    }
    public void RefreshTile(int lx, int ly, WorldLayer layer)
    {
        Tilemap targetMap = GetMapByLayer(layer);
        if (targetMap != null)
        {
            targetMap.RefreshTile(new Vector3Int(lx, ly, 0));
        }
    }

    private Tilemap GetMapByLayer(WorldLayer layer)
    {
        return layer switch
        {
            WorldLayer.Background => BackgroundMap,
            WorldLayer.Main => MainMap,
            WorldLayer.Foreground => ForegroundMap,
            WorldLayer.Damage => DamageMap,
            _ => null
        };
    }
    public void SetTilesBatch(TileBase[] tiles, BoundsInt area)
    {
        var collider = MainMap.GetComponent<TilemapCollider2D>();
        if (collider != null) collider.enabled = false;

        MainMap.SetTilesBlock(area, tiles);

        if (collider != null) collider.enabled = true;
    }
}

// Enum dla czytelności kodu
public enum WorldLayer { Background, Main, Foreground, Damage }
