using UnityEngine;
using System;

[Serializable]
public class InventoryEntry
{
    private readonly ItemData _data;
    private int _quantity;

    public ItemData Data => _data;
    public int Quantity => _quantity;

    public InventoryEntry(ItemData data, int quantity)
    {
        _data = data;
        _quantity = quantity;
    }

    public void Add(int amount) 
{
    _quantity = Mathf.Min(_quantity + amount, _data.MaxStackSize);
}

public bool TryRemove(int amount) 
{
    if (_quantity < amount) return false;
    
    _quantity -= amount;
    return true;
}
    
    public bool IsFull => _quantity >= _data.MaxStackSize;
}
