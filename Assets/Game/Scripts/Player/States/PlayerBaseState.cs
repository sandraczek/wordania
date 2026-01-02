using UnityEngine;
public abstract class PlayerBaseState
{
    protected Player _player;
    protected PlayerStateFactory _factory;

    [Header("Booleans")]
    public virtual bool CanPerformActions => false;
    public virtual bool CanSetSlot => false;

    public PlayerBaseState(Player player, PlayerStateFactory factory)
    {
        _player = player;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
}