using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [field: Header("References")]
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    [SerializeField] private InputReader _inputs;
    [field: SerializeField] private WorldSettings _settings;
    private PlayerConfig _config;
    
    [HideInInspector] public float LastJumpTime = float.MinValue;
    [HideInInspector] public float LastGroundedTime { get; private set; } = 0f;
    [field: SerializeField] public bool IsGrounded { get; private set; }

    private bool _isFacingRight= true;

    [field: Header("Events")]
    public event Action<Vector3> OnPlayerWarped;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
    }
    public void Start()
    {
        SetGravity(_config.GravityScale);
    }
    public void Initialize(PlayerConfig config)
    {
        _config = config;
    }

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

        transform.position = targetPosition;
        _rb.linearVelocity = Vector2.zero;

        OnPlayerWarped?.Invoke(delta);
    }

    private bool CheckGrounded()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(0f, -(_config.GroundCheckSize.y / 2) + 0.1f);

        return Physics2D.BoxCast(origin, _config.GroundCheckSize, 0f, Vector2.down, _config.GroundCheckDistance + 0.1f, _settings.GroundLayer);
    }

    public void CheckForFlip(float direction)
    {
        if (Mathf.Abs(direction) < 0.01f) return;

        bool inputRight = direction > 0;
        
        if (inputRight != _isFacingRight)
        {
            _isFacingRight = !_isFacingRight;

            if (_isFacingRight)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public Vector2 GetWorldAimPosition()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_inputs.CursorScreenPosition);
        return new Vector2(worldPos.x, worldPos.y);
    }

    public void TryStepUp(float horizontalInput)
    {
        if (Mathf.Abs(horizontalInput) < 0.01f) return;

        float direction = Mathf.Sign(horizontalInput);
        Vector2 rayOrigin = new(
            _col.bounds.center.x + (direction * _col.bounds.extents.x), 
            _col.bounds.min.y + 0.05f
        );

        RaycastHit2D hitLow = Physics2D.Raycast(rayOrigin, Vector2.right * direction, _config.StepLookDistance, _settings.GroundLayer);
        
        if (hitLow.collider != null)
        {
            RaycastHit2D hitHigh = Physics2D.Raycast(rayOrigin + Vector2.up * _settings.TileSize, Vector2.right * direction, _config.StepLookDistance, _settings.GroundLayer);

            if (hitHigh.collider == null)
            {
                Vector2 targetPos = _rb.position + new Vector2(direction * 0.1f, _settings.TileSize + 0.05f);
                Collider2D overlap = Physics2D.OverlapBox(targetPos + _col.offset, _col.size * 0.95f, 0, _settings.GroundLayer);

                if (overlap == null)
                {
                    ExecuteStepUp();
                }
            }
        }
    }

    private void ExecuteStepUp()
    {
        _rb.MovePosition(_rb.position + Vector2.up * (_settings.TileSize + 0.05f));
        if (_rb.linearVelocityY < 0) _rb.linearVelocityY = 0f;
    }

    // setters getters
    public void SetGravity(float scale)
    {
        _rb.gravityScale = scale;
    }
    public void SetVelocityX(float velX)
    {
        _rb.linearVelocityX = velX;
    }
    public void SetVelocityY(float velY)
    {
        _rb.linearVelocityY = velY;
    }
    public float GetVelocityX()
    {
        return _rb.linearVelocityX;
    }
    public float GetVelocityY()
    {
        return _rb.linearVelocityY;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position + new Vector3(0f, -_config.GroundCheckSize.y * 0.5f - _config.GroundCheckDistance * 0.5f, 0f), new Vector3(_config.GroundCheckSize.x, _config.GroundCheckDistance + _config.GroundCheckSize.y, 0f));
    }
}
