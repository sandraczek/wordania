using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInventory : MonoBehaviour
{
    private Player _player;
    [SerializeField] private LootEvent _lootChannel;

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
}
