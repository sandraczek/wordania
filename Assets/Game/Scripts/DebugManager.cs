using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }
    [SerializeField] private InputReader _inputReader;

    [field: SerializeField] public DebugSettings Settings {get; private set;}

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
    }

    private void OnEnable()
    {
        _inputReader.InputActions.Debug.Enable();

        _inputReader.InputActions.Debug.ShowChunks.performed += OnToggleChunks;
    }

    private void OnDisable()
    {
        _inputReader.InputActions.Debug.ShowChunks.performed -= OnToggleChunks;
        _inputReader.InputActions.Debug.Disable();
    }

    private void OnToggleChunks(InputAction.CallbackContext context)
    {
        Settings.ShowChunks = !Settings.ShowChunks;
    }
}
