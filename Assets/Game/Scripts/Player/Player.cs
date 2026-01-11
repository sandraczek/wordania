using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour, ISaveable
{
    public InputReader Inputs;

    private PlayerController _controller;
    public PlayerController Controller => _controller;

    private PlayerStateMachine _states;
    public PlayerStateMachine States => _states;
    [field:SerializeField] public PlayerConfig Config {get; private set;}
    [field:SerializeField] public InventoryData Inventory {get; private set;}
    [SerializeField] private PlayerVisuals visuals;
    private PlayerHealth _health;
    public PlayerHealth Health => _health;

    public string PersistenceId => "Player_Main";

    public void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _states = GetComponent<PlayerStateMachine>();
        _health = GetComponent<PlayerHealth>();
        _controller.Setup(Config);
    }
    private void OnEnable()
    {
        SaveManager.Service.Register(this);
        _health.OnHurt += HandleHurt;
        _health.OnHurt += HandleHurtVisuals;
        _health.OnDeath += HandleDeath;
        _controller.OnLanded += HandleLanding;
    }

    private void OnDisable()
    {
        SaveManager.Service.Unregister(this);
        _health.OnHurt -= HandleHurt;
        _health.OnHurt -= HandleHurtVisuals;
        _health.OnDeath -= HandleDeath;
        _controller.OnLanded -= HandleLanding;
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

    public object CaptureState()
    {
        return new PlayerData(
            Controller.Position,
            _health.Current,
            _health.Max
        );
    }

    public void RestoreState(object state)
    {
        if (state is Newtonsoft.Json.Linq.JObject jObject)
        {
            var dataJ = jObject.ToObject<PlayerData>();
            ApplyData(dataJ);
        }
        else if (state is PlayerData data)
        {
            ApplyData(data);
        }
    }
    private void ApplyData(PlayerData data)
    {
        if (data == null) return;
        Controller.Position = data.Position;
        _health.SetInitial(data.Health, data.MaxHealth);
    }
}
