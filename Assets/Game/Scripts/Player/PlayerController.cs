using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: Header("References")]
    [HideInInspector] public Rigidbody2D rb { get; private set; }
    [HideInInspector] public BoxCollider2D col { get; private set; }
    [HideInInspector] public PlayerInteraction interaction { get; private set; }
    [field: SerializeField] private WorldSettings _settings;

    // === USTAWIENIA (TWEAKOWANIE FELLINGU) ===
    [field: Header("Movement Stats")]
    [field: SerializeField] public float moveSpeed { get; private set; } = 10f;
    [field: SerializeField] public float moveSpeedAirMult { get; private set; } = 0.8f;
    [field: SerializeField] public float stoppingSpeed { get; private set; } = 0.1f;
    [field: SerializeField] public float airAccelerationSpeed { get; private set; } = 0.1f;
    [field: SerializeField] public float airStoppingSpeed { get; private set; } = 0.01f;
    [field: SerializeField] public float accelerationSpeed { get; private set; } = 0.2f;
    [field: SerializeField] public float jumpForce { get; private set; } = 24f;
    [field: SerializeField] private float _stepLookDistance = 0.2f;

    [field: Header("Runtime Info")]
    public float LastJumpTime = float.MinValue;
    public float LastGroundedTime { get; private set; } = 0f;
    public float JumpPressedTime = float.MinValue;
    [field: SerializeField] public float JumpBuffor { get; private set; } = 0.1f;   // jump when pressed before hitting ground
    [field: SerializeField] public float CoyoteTime { get; private set; } = 0.1f;   // time to jump after walking off a block
    [field: SerializeField] public float MinJumpDuration { get; private set; } = 0.1f;  // minimal jump duration for dealing with glitches

    [field: Header("Physics Tweaks")]
    [field: SerializeField] public float GravityScale { get; private set; } = 5f; // Domyślna grawitacja
    [field: SerializeField] public float FallGravityMult { get; private set; } = 1.5f; // Szybsze spadanie
    [field: SerializeField] public float LowJumpGravityMultiplier { get; private set; } = 3f; // Jak szybko spadać po puszczeniu spacji

    [field: Header("Ground Check")]
    [field: SerializeField] public bool IsGrounded { get; private set; }
    private Vector2 groundCheckSize = new(2.4f, 0.1f);
    private float groundCheckDistance = 0.1f;

    [field: Header("Visuals")]
    private bool isFacingRight= true;

    [field: Header("Inputs")]
    [field: SerializeField] public Vector2 movementInput { get; private set; }
    [field: SerializeField] public Vector2 cursorScreenPosition { get; private set; }
    public bool interactionInput { get; set; }
    public bool interactionTriggered {get; set; }
    public bool jumpInput { get; set; } 

    [field: Header("Events")]
    public event Action<Vector3> OnPlayerWarped;

    public GameInput inputActions { get; private set; }
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        interaction = GetComponent<PlayerInteraction>();
        stateMachine = GetComponent<PlayerStateMachine>(); 
        
        inputActions = new GameInput();
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        inputActions.Player.Interact.performed+= ctx => {interactionTriggered = true; interactionInput = true;};
        inputActions.Player.Interact.canceled += ctx => interactionInput = false;
        inputActions.Player.Point.performed+= ctx => cursorScreenPosition = ctx.ReadValue<Vector2>();
        inputActions.Player.CycleInteraction.started+= ctx => interaction.CycleInteractionMode();
        
        inputActions.Player.Jump.performed += ctx => { jumpInput = true; JumpPressedTime = Time.time; };
        inputActions.Player.Jump.canceled += ctx => jumpInput = false;

        jumpInput = false;
        interactionTriggered = false;
        interactionInput = false;

        SetGravity(GravityScale);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

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

        rb.linearVelocity = Vector2.zero;
        transform.position = targetPosition;

        OnPlayerWarped?.Invoke(delta);
    }

    private bool CheckGrounded()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(0f, -(groundCheckSize.y / 2) + 0.1f);

        return Physics2D.BoxCast(origin, groundCheckSize, 0f, Vector2.down, groundCheckDistance + 0.1f, _settings.GroundLayer);
    }

    public void SetGravity(float scale)
    {
        rb.gravityScale = scale;
    }

    public void CheckForFlip()
    {
        if (Mathf.Abs(movementInput.x) < 0.01f) return;

        // 2. Warunek zmiany: Czy kierunek inputu jest inny niż kierunek patrzenia?
        bool inputRight = movementInput.x > 0;
        
        if (inputRight != isFacingRight)
        {
            isFacingRight = !isFacingRight;

            // "Industry Standard" obrót:
            // Ustawiamy sztywno 0 lub 180. To jest bezpieczniejsze niż mnożenie przez -1.
            if (isFacingRight)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public Vector2 GetWorldAimPosition()
    {
        // 1. Jeśli gramy na Myszce:
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
        return new Vector2(worldPos.x, worldPos.y);

        // 2. (Przyszłość) Jeśli gramy na Padzie:
        // return (Vector2)transform.position + (RawLookInput * zasięg);
    }

    public void TryStepUp(float horizontalInput)
    {
        if (Mathf.Abs(horizontalInput) < 0.01f) return;

        float direction = Mathf.Sign(horizontalInput);
        Vector2 rayOrigin = new(
            col.bounds.center.x + (direction * col.bounds.extents.x), 
            col.bounds.min.y + 0.05f
        );

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, Vector2.right * direction, _stepLookDistance, _settings.GroundLayer);
        
        if (hitLow.collider != null)
        {
            RaycastHit2D hitHigh = Physics2D.Raycast(rayOrigin + Vector2.up * _settings.TileSize, Vector2.right * direction, _stepLookDistance, _settings.GroundLayer);

            if (hitHigh.collider == null)
            {
                Vector2 targetPos = rb.position + new Vector2(direction * 0.1f, _settings.TileSize + 0.05f);
                Collider2D overlap = Physics2D.OverlapBox(targetPos + col.offset, col.size * 0.95f, 0, _settings.GroundLayer);

                if (overlap == null)
                {
                    ExecuteStepUp();
                }
            }
        }
    }

    private void ExecuteStepUp()
    {
        rb.MovePosition(rb.position + Vector2.up * (_settings.TileSize + 0.05f));
        if (rb.linearVelocityY < 0) rb.linearVelocityY = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -groundCheckSize.y * 0.5f - groundCheckDistance * 0.5f, 0f), new Vector3(groundCheckSize.x, groundCheckDistance + groundCheckSize.y, 0f));
    }
}
