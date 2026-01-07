using UnityEngine;

[CreateAssetMenu(fileName = "Item_New", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int _id = -1;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxStackSize = 99;

    public int Id => _id;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public int MaxStackSize => _maxStackSize;
}
