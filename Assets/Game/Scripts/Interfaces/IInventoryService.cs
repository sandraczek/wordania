using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IInventoryService
{
    bool IsOpen { get; }
    event Action OnInventoryChanged;
    event Action<bool> OnStateChanged;

    void AddItem(int itemId, int amount);
    bool RemoveItem(int slotIndex, int amount);
    int GetQuantity(int itemId);
    IEnumerable<InventoryEntry> GetAllEntries();
    void SetVisibility(bool isOpen);
}