using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewBlock", menuName = "WorldMap/Block Data")]
public class BlockData : ScriptableObject
{
    [Header("Id")]
    public string ItemID;        // np. "dirt_block" (Ważne do zapisu gry!)
    public string DisplayName;   // np. "Dirt"

    [Header("Visual")]
    public TileBase Tile;        // Tu wrzucimy nasz Rule Tile

    [Header("Stats")]
    [Range(1, 10)]
    public int Hardness = 1;     // Ile uderzeń kilofem potrzeba
    //public ItemData DropItem;    // (Opcjonalnie) Co wypada po zniszczeniu
    
    /*
    [Header("Sounds")]
    public AudioClip PlaceSound;
    public AudioClip BreakSound;
    */
}
