using UnityEngine;
using System;
public class InputReader : MonoBehaviour
{
   private GameInput _inputActions;

   [field: SerializeField] public Vector2 MovementInput { get; private set; }
   [field: SerializeField] public Vector2 CursorScreenPosition { get; private set; }

   public bool JumpInput { get; private set; } 
   [HideInInspector] public float JumpPressedTime { get; private set; }  = float.MinValue;

   public event Action<int> OnHotbarSlotPressed;
   public event Action<bool> OnPrimaryActionHeld;
   public event Action OnCycleActionSetting;

   private void OnEnable() => _inputActions.Enable();
   private void OnDisable() => _inputActions.Disable();

   public void Awake()
   {
      _inputActions = new GameInput();
      _inputActions.Player.Move.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
      _inputActions.Player.Move.canceled += ctx => MovementInput = Vector2.zero;
      _inputActions.Player.PrimaryAction.performed+= ctx => OnPrimaryActionHeld.Invoke(true);
      _inputActions.Player.PrimaryAction.canceled += ctx => OnPrimaryActionHeld.Invoke(false);
      _inputActions.Player.Point.performed+= ctx => CursorScreenPosition = ctx.ReadValue<Vector2>();
      _inputActions.Player.CycleActionSetting.started+= ctx => OnCycleActionSetting?.Invoke();
      _inputActions.Player.Slot1.performed+= ctx => OnHotbarSlotPressed?.Invoke(1);
      _inputActions.Player.Slot2.performed+= ctx => OnHotbarSlotPressed?.Invoke(2);
      
      
      _inputActions.Player.Jump.performed += ctx => { JumpInput = true; JumpPressedTime = Time.time; };
      _inputActions.Player.Jump.canceled += ctx => JumpInput = false;

      JumpInput = false;
   }
   public void ConsumeJump()
    {
        JumpPressedTime = float.MinValue;
    } 
}
