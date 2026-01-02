using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    [field: SerializeField] public DebugSettings Settings {get; private set;}
    
    private GameInput _inputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _inputActions = new GameInput();
    }

    private void OnEnable()
    {
        _inputActions.Debug.Enable();

        _inputActions.Debug.ShowChunks.performed += OnToggleChunks;
    }

    private void OnDisable()
    {
        _inputActions.Debug.ShowChunks.performed -= OnToggleChunks;
        _inputActions.Debug.Disable();
    }

    private void OnToggleChunks(InputAction.CallbackContext context)
    {
        Settings.ShowChunks = !Settings.ShowChunks;
    }
}
