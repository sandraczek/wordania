using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class Inventory : MonoBehaviour, IInventory//, ISaveable
{
    private readonly InventoryData _data;
    public InventoryData Data => _data;
}