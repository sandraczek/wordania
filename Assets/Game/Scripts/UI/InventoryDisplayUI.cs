using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class InventoryDisplayUI : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] private InventoryData _inventory;

    [Header("UI Setup")]
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private Transform _contentParent;

    private ObjectPool<InventorySlotUI> _pool;
    private readonly List<InventorySlotUI> _activeSlots = new();

    private void Awake()
    {
        _pool = new ObjectPool<InventorySlotUI>(
            createFunc: OnCreateSlot,         // Co robić, gdy brakuje obiektów w puli?
            actionOnGet: OnGetSlot,           // Co robić, gdy wyjmujemy obiekt z puli?
            actionOnRelease: OnReleaseSlot,   // Co robić, gdy chowany obiekt do puli?
            actionOnDestroy: OnDestroySlot,   // Co robić, gdy usuwamy obiekt na stałe?
            collectionCheck: false,           // Dla wydajności wyłączamy sprawdzanie czy obiekt już jest w puli
            defaultCapacity: 20,              // Ile slotów przygotować na start
            maxSize: 100                      // Maksymalny limit obiektów w pamięci
        );
    }

    #region Pool Callbacks
    private InventorySlotUI OnCreateSlot() => Instantiate(_slotPrefab, _contentParent);
    
    private void OnGetSlot(InventorySlotUI slot) => slot.gameObject.SetActive(true);

    private void OnReleaseSlot(InventorySlotUI slot) => slot.gameObject.SetActive(false);

    private void OnDestroySlot(InventorySlotUI slot) => Destroy(slot.gameObject);
    #endregion

    private void OnEnable()
    {
        _inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        _inventory.OnInventoryChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        foreach (var slot in _activeSlots)
        {
            _pool.Release(slot);
        }
        _activeSlots.Clear();

        foreach (var entry in _inventory.GetAllEntries())
        {
            InventorySlotUI slot = _pool.Get(); 
            
            slot.transform.SetAsLastSibling(); 
            
            slot.SetData(entry);
            _activeSlots.Add(slot);
        }
    }
}