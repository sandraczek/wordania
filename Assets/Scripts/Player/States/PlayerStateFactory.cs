using UnityEngine;
public class PlayerStateFactory
{
    PlayerStateMachine _context;
    
    // Słownik lub zmienne dla każdego stanu
    // W prostszej wersji po prostu propertisy:
    public PlayerBaseState Idle { get; private set; }
    public PlayerBaseState Run { get; private set; }
    public PlayerBaseState Jump { get; private set; }
    public PlayerBaseState Fall { get; private set; }

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        Idle = new PlayerIdleState(_context, this);
        Run = new PlayerRunState(_context, this);
        Jump = new PlayerJumpState(_context, this);
        Fall = new PlayerFallState(_context, this);
    }
}