using UnityEngine;
public abstract class PlayerBaseState
{
    protected PlayerContext _context;
    protected IInputReader _inputs;
    protected PlayerStateFactory _factory;

    [Header("Booleans")]
    public virtual bool CanPerformActions => false;
    public virtual bool CanSetSlot => false;

    public PlayerBaseState(PlayerContext context, IInputReader inputs, PlayerStateFactory factory)
    {
        _context = context;
        _factory = factory;
        _inputs = inputs;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
}