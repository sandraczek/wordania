using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewBlock", menuName = "World/Block")]
public class BlockData : ScriptableObject
{
    [Header("Id")]
    public int ID;
    public string DisplayName;

    [Header("Visual")]
    public TileBase Tile;

    [Header("Stats")]
    [Range(1, 10)]
    public float Hardness = 1;
    
    [Header("Item Info")]
    public Recipe recipe;
    public ItemData loot;
    public int lootAmount;
}
