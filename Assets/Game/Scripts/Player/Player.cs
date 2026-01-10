using UnityEngine;
using System;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    public static Player Local {get;private set;}
    public InputReader Inputs;

    private PlayerController _controller;
    public PlayerController Controller => _controller;

    private PlayerStateMachine _states;
    public PlayerStateMachine States => _states;
    [field:SerializeField] public PlayerConfig Config {get; private set;}
    [field:SerializeField] public InventoryData Inventory {get; private set;}
    [SerializeField] private PlayerVisuals visuals;
    private PlayerData _data;
    public PlayerData Data { get 
    {
        _data.SetPosition(transform.position); 
        return _data;
    } }
    private PlayerHealth _health;
    public PlayerHealth Health => _health;

    public static event Action<Player> OnLocalPlayerReady;

    public void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _states = GetComponent<PlayerStateMachine>();
        _health = GetComponent<PlayerHealth>();
        _controller.Setup(Config);
        _data = new();
    }
    public void Initialize()
    {
        _health.Initialize(_data.Health);

        Local = this;

        OnLocalPlayerReady?.Invoke(Local);
    }
    private void OnEnable()
    {
        _health.OnHurt += HandleHurt;
        _health.OnHurt += HandleHurtVisuals;
        _health.OnDeath += HandleDeath;
        _controller.OnLanded += HandleLanding;
    }

    private void OnDisable()
    {
        _health.OnHurt -= HandleHurt;
        _health.OnHurt -= HandleHurtVisuals;
        _health.OnDeath -= HandleDeath;
        _controller.OnLanded -= HandleLanding;
    }
    public void LoadData(PlayerData data)
    {
        _data = data;
        _controller.Warp(_data.Position);
    }
    private void HandleHurt()
    {
        _states.SwitchState(_states.Factory.Hurt);
    }

    private void HandleDeath()
    {
        //_states.SwitchState(_states.Factory.Dead);
    }
    private void HandleLanding(float velocity)
    {
        if (velocity > Config.fallDamageThreshold)
        {
            float damage = CalculateFallDamage(velocity);
            if(damage > 0f) Health.TakeDamage(damage, DamageSource.FALL_DAMAGE);
        }
    }
    private float CalculateFallDamage(float velocity)
    {
        return (velocity - Config.fallDamageThreshold) * Config.fallDamageMultiplier;
    }
    private void HandleHurtVisuals()
    {
        visuals.PlayHurtEffect();
    }
}
