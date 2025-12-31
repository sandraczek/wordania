using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: Header("References")]
    [HideInInspector] public Rigidbody2D RB { get; private set; }
    [HideInInspector] public BoxCollider2D Col { get; private set; }
    [HideInInspector] public PlayerInteraction Interaction { get; private set; }
    [field: SerializeField] private WorldSettings _settings;
    [field: SerializeField] public PlayerConfig Config { get; private set;}
    
    [HideInInspector] public float LastJumpTime = float.MinValue;
    [HideInInspector] public float LastGroundedTime { get; private set; } = 0f;
    [HideInInspector] public float JumpPressedTime = float.MinValue;
    [field: SerializeField] public bool IsGrounded { get; private set; }

    private bool _isFacingRight= true;

    [field: Header("Inputs")]
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    [field: SerializeField] public Vector2 CursorScreenPosition { get; private set; }
    public bool JumpInput { get; set; } 

    [field: Header("Events")]
    public event Action<Vector3> OnPlayerWarped;
    public event Action<int> OnHotbarSlotPressed;
    public event Action OnToolSettingChanged;
    public event Action<bool> OnPrimaryActionHeld;

    public GameInput InputActions { get; private set; }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Col = GetComponent<BoxCollider2D>();
        Interaction = GetComponent<PlayerInteraction>(); 
        
        InputActions = new GameInput();
        InputActions.Player.Move.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        InputActions.Player.Move.canceled += ctx => MovementInput = Vector2.zero;
        InputActions.Player.PrimaryAction.performed+= ctx => OnPrimaryActionHeld.Invoke(true);
        InputActions.Player.PrimaryAction.canceled += ctx => OnPrimaryActionHeld.Invoke(false);
        InputActions.Player.Point.performed+= ctx => CursorScreenPosition = ctx.ReadValue<Vector2>();
        InputActions.Player.CycleInteraction.started+= ctx => OnToolSettingChanged?.Invoke();
        InputActions.Player.Slot1.performed+= ctx => OnHotbarSlotPressed?.Invoke(1);
        InputActions.Player.Slot2.performed+= ctx => OnHotbarSlotPressed?.Invoke(2);
        
        
        InputActions.Player.Jump.performed += ctx => { JumpInput = true; JumpPressedTime = Time.time; };
        InputActions.Player.Jump.canceled += ctx => JumpInput = false;

        JumpInput = false;

        SetGravity(Config.GravityScale);
    }

    private void OnEnable() => InputActions.Enable();
    private void OnDisable() => InputActions.Disable();

    private void Update()
    {


    }
    private void FixedUpdate()
    {
        IsGrounded = CheckGrounded();

        if (IsGrounded)
        {
            LastGroundedTime = Time.time;
        }
    }
    public void Warp(Vector3 targetPosition) 
    {
        Vector3 delta = targetPosition - transform.position;

        RB.linearVelocity = Vector2.zero;
        transform.position = targetPosition;

        OnPlayerWarped?.Invoke(delta);
    }

    private bool CheckGrounded()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(0f, -(Config.GroundCheckSize.y / 2) + 0.1f);

        return Physics2D.BoxCast(origin, Config.GroundCheckSize, 0f, Vector2.down, Config.GroundCheckDistance + 0.1f, _settings.GroundLayer);
    }

    public void SetGravity(float scale)
    {
        RB.gravityScale = scale;
    }

    public void CheckForFlip()
    {
        if (Mathf.Abs(MovementInput.x) < 0.01f) return;

        // 2. Warunek zmiany: Czy kierunek inputu jest inny niż kierunek patrzenia?
        bool inputRight = MovementInput.x > 0;
        
        if (inputRight != _isFacingRight)
        {
            _isFacingRight = !_isFacingRight;

            // "Industry Standard" obrót:
            // Ustawiamy sztywno 0 lub 180. To jest bezpieczniejsze niż mnożenie przez -1.
            if (_isFacingRight)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public Vector2 GetWorldAimPosition()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(CursorScreenPosition);
        return new Vector2(worldPos.x, worldPos.y);
    }

    public void TryStepUp(float horizontalInput)
    {
        if (Mathf.Abs(horizontalInput) < 0.01f) return;

        float direction = Mathf.Sign(horizontalInput);
        Vector2 rayOrigin = new(
            Col.bounds.center.x + (direction * Col.bounds.extents.x), 
            Col.bounds.min.y + 0.05f
        );

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, Vector2.right * direction, Config.StepLookDistance, _settings.GroundLayer);
        
        if (hitLow.collider != null)
        {
            RaycastHit2D hitHigh = Physics2D.Raycast(rayOrigin + Vector2.up * _settings.TileSize, Vector2.right * direction, Config.StepLookDistance, _settings.GroundLayer);

            if (hitHigh.collider == null)
            {
                Vector2 targetPos = RB.position + new Vector2(direction * 0.1f, _settings.TileSize + 0.05f);
                Collider2D overlap = Physics2D.OverlapBox(targetPos + Col.offset, Col.size * 0.95f, 0, _settings.GroundLayer);

                if (overlap == null)
                {
                    ExecuteStepUp();
                }
            }
        }
    }

    private void ExecuteStepUp()
    {
        RB.MovePosition(RB.position + Vector2.up * (_settings.TileSize + 0.05f));
        if (RB.linearVelocityY < 0) RB.linearVelocityY = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -Config.GroundCheckSize.y * 0.5f - Config.GroundCheckDistance * 0.5f, 0f), new Vector3(Config.GroundCheckSize.x, Config.GroundCheckDistance + Config.GroundCheckSize.y, 0f));
    }
}
