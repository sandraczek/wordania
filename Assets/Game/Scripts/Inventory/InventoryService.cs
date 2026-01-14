using UnityEngine;
using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

public class InventoryService : IInventoryService, IDisposable
{
    private readonly IInventory _inventory;
    private readonly ItemDatabase _database;
    public bool IsOpen { get; private set; }
    private readonly LootEvent _lootChannel; // temporary
    public event Action<bool> OnStateChanged;
    public event Action OnInventoryChanged;

    public InventoryService(IInventory inventory, ItemDatabase database, LootEvent lootEvent)
    {
        _inventory = inventory;
        _database = database;
        _lootChannel = lootEvent;

        _lootChannel.Subscribe(HandleLoot);
    }
    public void Dispose()
    {
        _lootChannel.Unsubscribe(HandleLoot);
    }
                                            // TO DO - SWITCH TO List<>
    public void AddItem(int id, int amount) // to do - convert all to bool
    {       
        var data = _database.GetItem(id);   //to do - and also structural refactor HandleLoot
        if (data == null) return;
        if (data == null || amount <= 0) return;

        if (_inventory.Data._content.TryGetValue(data.Id, out InventoryEntry entry))
        {
            entry.Add(amount);
        }
        else
        {
            _inventory.Data._content.Add(data.Id, new InventoryEntry(data, amount));
        }

        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(int id, int amount)
    {
        var data = _database.GetItem(id);
        if (data == null || amount <= 0) return false;

        if (_inventory.Data._content.TryGetValue(data.Id, out InventoryEntry entry))
        {
            bool success = entry.TryRemove(amount);
            if(success) OnInventoryChanged?.Invoke();
            return success;
        }

        return false;
    }
    public int GetQuantity(int itemId)
    {
        return _inventory.Data._content.TryGetValue(itemId, out InventoryEntry entry) ? entry.Quantity : 0;
    }
    public IEnumerable<InventoryEntry> GetAllEntries() => _inventory.Data._content.Values;
    public void ClearInventory()
    {
        _inventory.Data._content.Clear();
    }

    private void HandleLoot(ItemData item, int amount) {
        AddItem(item.Id, amount);
    }

    public void SetVisibility(bool isOpen)
    {
        if (IsOpen == isOpen) return;
        
        IsOpen = isOpen;
        
        // maybe later - move to ITimeService
        Time.timeScale = isOpen ? 0f : 1f;
        
        OnStateChanged?.Invoke(IsOpen);
    }
}
