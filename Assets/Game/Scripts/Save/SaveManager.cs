using UnityEngine;
public class SaveManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    [ContextMenu("Save")]
    public void SaveGame()
    {
        SaveData data = new();

        data.PlayerPosition = _player.transform.position;
        data.SaveDate = System.DateTime.Now.ToString();

        foreach (var item in _player.Inventory.GetAllEntries())
        {
            if (item.Quantity > 0)
            {
                data.Inventory.Add(new ItemSaveData { id = item.Data.Id, amount = item.Quantity });
            }
        }

        FileHandler.SaveJSON(data, "SaveSlot_1");
    }
    [ContextMenu("Load")]
    public void LoadGame()
    {
        SaveData data = FileHandler.LoadJSON<SaveData>("SaveSlot_1");
        if (data == null) return;

        _player.transform.position = data.PlayerPosition;

        _player.Inventory.ClearInventory();
        foreach (var item in data.Inventory)
        {
            _player.Inventory.AddItem(item.id, item.amount);
        }
        
        Debug.Log("Game loaded successfully.");
    }
}