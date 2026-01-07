using UnityEngine;
public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private InputReader _inputReader;

    private bool _isOpen = false;

    private void Awake()
    {
        
    }
    private void Start()
    {
        _isOpen = false;
        _inventoryPanel.SetActive(false);
    }
    private void OnEnable() => _inputReader.OnToggleInventory += Toggle;
    private void OnDisable() => _inputReader.OnToggleInventory -= Toggle;

    private void Update()
    {

    }

    public void Toggle()
    {
        _isOpen = !_isOpen;
        _inventoryPanel.SetActive(_isOpen);

        if (_isOpen)
        {
            OnOpen();
        }
        else
        {
            OnClose();
        }
    }

    private void OnOpen()
    {
        Time.timeScale = 0f;
        _gameManager.UpdatePlayerMenuState(true);
    }

    private void OnClose()
    {
        Time.timeScale = 1f;
        _gameManager.UpdatePlayerMenuState(false);
    }
}
