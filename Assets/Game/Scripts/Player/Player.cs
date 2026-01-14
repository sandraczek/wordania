using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealthView))]
public class Player : MonoBehaviour, ISaveable
{
    [Header("Components")]
    private PlayerController _controller;
    private PlayerStateMachine _states;
    private PlayerHealthView _health;
    [SerializeField] private PlayerVisuals visuals;

    [Header("Dependencies")]
    private InputReader _inputs;
    private PlayerStateFactory _factory;
    private PlayerConfig _config;
    private ISaveService _save;
    
    [Header("Save Data")]
    public string PersistenceId => "Player";  

    public void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _states = GetComponent<PlayerStateMachine>();
        _health = GetComponent<PlayerHealthView>();
    }
    [Inject]
    public void Configure(InputReader inputs, PlayerStateFactory factory, PlayerConfig config, ISaveService save)
    {
        _inputs = inputs;
        _factory = factory;
        _config = config;
        _save = save;

        _save.Register(this);
    }
    public void OnDestroy()
    {
        _save.Unregister(this);
    }
    private void OnEnable()
    {
        _health.OnHurt += HandleHurt;     // to do - make player not a god object
        _health.OnHurt += HandleHurtVisuals;
        _health.OnDeath += HandleDeath;
        _controller.OnLanded += HandleLanding;
        _inputs.OnToggleInventory += HandleInventoryToggle;
    }

    private void OnDisable()
    {
        _health.OnHurt -= HandleHurt;
        _health.OnHurt -= HandleHurtVisuals;
        _health.OnDeath -= HandleDeath;
        _controller.OnLanded -= HandleLanding;
        _inputs.OnToggleInventory -= HandleInventoryToggle;
    }
    private void HandleHurt()
    {
        _states.SwitchState(_factory.Hurt);
    }

    private void HandleDeath()
    {
        //_states.SwitchState(_states.Factory.Dead);
    }
    private void HandleInventoryToggle()
    {
        if (_states.CurrentState == _factory.InMenu)
        {
            _states.SwitchState(_factory.Idle);
        }
        else
        {
            _states.SwitchState(_factory.InMenu);
        }
    }
    private void HandleLanding(float velocity)  // to move
    {
        if (velocity > _config.fallDamageThreshold)
        {
            float damage = CalculateFallDamage(velocity);
            if(damage > 0f) _health.TakeDamage(damage);
        }
    }
    private float CalculateFallDamage(float velocity) // to move
    {
        return (velocity - _config.fallDamageThreshold) * _config.fallDamageMultiplier;
    }
    private void HandleHurtVisuals()
    {
        visuals.PlayHurtEffect();
    }

    // ----- Save -----

    public object CaptureState()
    {
        return new PlayerSaveData(
            _controller.Position
        );
    }

    public void RestoreState(object state)
    {
        if (state is Newtonsoft.Json.Linq.JObject jObject)
        {
            var dataJ = jObject.ToObject<PlayerSaveData>();
            ApplyData(dataJ);
        }
        else if (state is PlayerSaveData data)
        {
            ApplyData(data);
        }
    }
    private void ApplyData(PlayerSaveData data)
    {
        if (data == null) return;
        _controller.Position = data.Position;
    }
}
