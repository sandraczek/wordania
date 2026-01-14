using Unity.VisualScripting;
using UnityEngine;
using System;

public interface IInputReader
{
    Vector2 MovementInput { get; }
    Vector2 CursorScreenPosition { get; }
    bool JumpInput { get; }
    float JumpPressedTime { get; }

    // --- Events ---
    event Action<int> OnHotbarSlotPressed;
    event Action<bool> OnPrimaryActionHeld;
    event Action OnCycleActionSettings;
    event Action OnToggleInventory;
    event Action OnToggleChunks;

    public void EnablePlayerInput();
    public void DisableAllInput();
    public void ConsumeJump();
}
