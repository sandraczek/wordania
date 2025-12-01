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
    [HideInInspector] public LayerMask GroundLayer { get; private set; }

    // === USTAWIENIA (TWEAKOWANIE FELLINGU) ===
    [field: Header("Movement Stats")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 8f;
    [field: SerializeField] public float MoveSpeedAirMult { get; private set; } = 0.8f;
    [field: SerializeField] public float StoppingSpeed { get; private set; } = 1f;
    [field: SerializeField] public float AirAccelerationSpeed { get; private set; } = 1f;
    [field: SerializeField] public float AirStoppingSpeed { get; private set; } = 0.1f;
    [field: SerializeField] public float AccelerationSpeed { get; private set; } = 2f;
    [field: SerializeField] public float JumpForce { get; private set; } = 26f;
    [field: SerializeField] public float JumpBuffor { get; private set; } = 0.2f;

    [field: Header("Physics Tweaks")]
    [field: SerializeField] public float GravityScale { get; private set; } = 4f; // Domyślna grawitacja
    [field: SerializeField] public float FallGravityMult { get; private set; } = 1.5f; // Szybsze spadanie
    [field: SerializeField] public float LowJumpGravityMultiplier { get; private set; } = 2f; // Jak szybko spadać po puszczeniu spacji

    [field: Header("Ground Check")]
    private Vector2 groundCheckSize = new(2.6f, 0.1f);
    private float groundCheckDistance = 0.1f;

    [field: Header("Visuals")]
    private bool IsFacingRight= true; // Domyślnie w prawo


    [field: Header("Inputs")]
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    [field: SerializeField] public Vector2 MousePosition { get; private set; }
    public bool InteractionTriggered { get; set; }
    public bool InteractionInput { get; set; }
    public bool JumpInput { get; set; }     // Czy przycisk jest wciśnięty?
    public bool JumpTriggered { get; set; } // Czy w tej klatce naciśnięto skok? (Buffer)

    public GameInput InputActions { get; private set; }
    private PlayerStateMachine _stateMachine;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Col = GetComponent<BoxCollider2D>();
        Interaction = GetComponent<PlayerInteraction>();
        _stateMachine = GetComponent<PlayerStateMachine>(); 

        GroundLayer = 3;
        
        InputActions = new GameInput();
        InputActions.Player.Move.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        InputActions.Player.Move.canceled += ctx => MovementInput = Vector2.zero;
        InputActions.UI.Click.performed+= ctx => {InteractionTriggered = true; InteractionInput = true; };
        InputActions.UI.Click.canceled += ctx => InteractionInput = false;
        InputActions.UI.Point.performed+= ctx => MousePosition = ctx.ReadValue<Vector2>();
        
        InputActions.Player.Jump.performed += ctx => { JumpInput = true; JumpTriggered = true; };
        InputActions.Player.Jump.canceled += ctx => JumpInput = false;

        JumpInput = false;
        JumpTriggered = false;
    }

    private void OnEnable() => InputActions.Enable();
    private void OnDisable() => InputActions.Disable();

    private void Update()
    {

    }

    public bool IsGrounded()
    {
        return RB.linearVelocityY <= 0f && Physics2D.BoxCast(transform.position + new Vector3(0f, -groundCheckSize.y, 0f), groundCheckSize, 0f, Vector2.down, groundCheckDistance, GroundLayer);
    }

    public void SetGravity(float scale)
    {
        RB.gravityScale = scale;
    }

    public void CheckForFlip()
    {
        // 1. Warunek konieczny: Input musi być wyraźny (nie 0)
        if (Mathf.Abs(MovementInput.x) < 0.01f) return;

        // 2. Warunek zmiany: Czy kierunek inputu jest inny niż kierunek patrzenia?
        bool inputRight = MovementInput.x > 0;
        
        if (inputRight != IsFacingRight)
        {
            IsFacingRight = !IsFacingRight;

            // "Industry Standard" obrót:
            // Ustawiamy sztywno 0 lub 180. To jest bezpieczniejsze niż mnożenie przez -1.
            if (IsFacingRight)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public Vector2 GetWorldAimPosition()
    {
        // 1. Jeśli gramy na Myszce:
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(MousePosition);
        return new Vector2(worldPos.x, worldPos.y);

        // 2. (Przyszłość) Jeśli gramy na Padzie:
        // return (Vector2)transform.position + (RawLookInput * zasięg);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -groundCheckSize.y * 0.5f - groundCheckDistance * 0.5f, 0f), new Vector3(groundCheckSize.x, groundCheckDistance + groundCheckSize.y, 0f));
    }
}
