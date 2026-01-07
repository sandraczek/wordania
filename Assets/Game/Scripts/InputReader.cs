using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    private GameInput _inputActions;
    public GameInput InputActions
    {
        get
        {
            if(_inputActions == null) Initialize();
            return _inputActions;
        }
    }

    // --- Properties ---
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    [field: SerializeField] public Vector2 CursorScreenPosition { get; private set; }
    public bool JumpInput { get; private set; }
    public float JumpPressedTime { get; private set; } = float.MinValue;

    // --- Events ---
    public event Action<int> OnHotbarSlotPressed;
    public event Action<bool> OnPrimaryActionHeld;
    public event Action OnCycleActionSettings;
    public event Action OnToggleInventory;

    private void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        if (_inputActions != null) return;
        _inputActions = new GameInput();
        
        _inputActions.Player.SetCallbacks(this);
        EnablePlayerInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void EnablePlayerInput() => _inputActions.Player.Enable();
    public void DisableAllInput() => _inputActions.Disable();

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnPrimaryActionHeld?.Invoke(true);
        if (context.canceled) OnPrimaryActionHeld?.Invoke(false);
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        CursorScreenPosition = context.ReadValue<Vector2>();
    }

    public void OnCycleActionSetting(InputAction.CallbackContext context)
    {
        if (context.started) OnCycleActionSettings?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
            JumpPressedTime = (float)context.startTime;
        }
        if (context.canceled)
        {
            JumpInput = false;
        }
    }

    public void OnSlot1(InputAction.CallbackContext context) { if (context.performed) OnHotbarSlotPressed?.Invoke(1); }
    public void OnSlot2(InputAction.CallbackContext context) { if (context.performed) OnHotbarSlotPressed?.Invoke(2); }

    public void OnShowInventory(InputAction.CallbackContext context)
    {
        if (context.performed) OnToggleInventory?.Invoke();
    }

    public void ConsumeJump()
    {
        JumpPressedTime = float.MinValue;
    }
}