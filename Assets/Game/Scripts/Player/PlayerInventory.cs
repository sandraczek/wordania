using UnityEngine;
using System;

[RequireComponent(typeof(Player))]
public class PlayerInventory : MonoBehaviour//, ISaveable
{
    private Player _player;
    [SerializeField] private LootEvent _lootChannel;

    public string PersistenceId => "Player_Inventory";

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void OnEnable() {
        _lootChannel.Subscribe(HandleLoot);
    }
    private void OnDisable() {
        _lootChannel.Unsubscribe(HandleLoot);
    }
    private void HandleLoot(ItemData item, int amount) {
        _player.Inventory.AddItem(item, amount);
    }

    // public object CaptureState()
    // {
        
    // }

    public void RestoreState(object state)
    {

    }
}

[Serializable]
public struct ItemSaveData
{
    public int id;
    public int amount;
}