using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
public class InventoryData : ScriptableObject
{
    private readonly Dictionary<int, InventoryEntry> _content = new();
    [SerializeField] private ItemDatabase _database;
    public event Action OnInventoryChanged;

    private void OnEnable() => _content.Clear();
    public void AddItem(ItemData data, int amount)
    {
        if (data == null || amount <= 0) return;

        if (_content.TryGetValue(data.Id, out InventoryEntry entry))
        {
            entry.Add(amount);
        }
        else
        {
            _content.Add(data.Id, new InventoryEntry(data, amount));
        }

        OnInventoryChanged?.Invoke();
    }
    public void AddItem(int id, int amount)
    {
        Debug.Log(id + ".1");
        ItemData data = _database.GetItem(id);
        if (data == null || amount <= 0) return;
        Debug.Log(id + ".2");
        if (_content.TryGetValue(data.Id, out InventoryEntry entry))
        {
            entry.Add(amount);
        }
        else
        {
            _content.Add(data.Id, new InventoryEntry(data, amount));
        }

        OnInventoryChanged?.Invoke();
    }
    public bool TryRemoveItem(ItemData data, int amount)
    {
        if (data == null || amount <= 0) return false;

        if (_content.TryGetValue(data.Id, out InventoryEntry entry))
        {

            bool success = entry.TryRemove(amount);
            if(success) OnInventoryChanged?.Invoke();
            return success;
        }
        else return false;
    }

    public int GetQuantity(int itemId)
    {
        return _content.TryGetValue(itemId, out InventoryEntry entry) ? entry.Quantity : 0;
    }

    public IEnumerable<InventoryEntry> GetAllEntries() => _content.Values;
    public void ClearInventory()
    {
        _content.Clear();
    }
}