using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/Starting Profile")]
public class StartingProfile : ScriptableObject
{
    [Header("Player Start State")]
    public Vector2 startingPosition;

    [Header("Starting Inventory")]
    public List<ItemSaveData> startingItems;
}