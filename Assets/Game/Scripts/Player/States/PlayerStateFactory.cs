using UnityEngine;
public class PlayerStateFactory
{
    private Player _player;
    public PlayerBaseState Idle { get; private set; }
    public PlayerBaseState Run { get; private set; }
    public PlayerBaseState Jump { get; private set; }
    public PlayerBaseState Fall { get; private set; }

    public PlayerStateFactory(Player player)
    {
        _player = player;
        Idle = new PlayerIdleState(_player, this);
        Run = new PlayerRunState(_player, this);
        Jump = new PlayerJumpState(_player, this);
        Fall = new PlayerFallState(_player, this);
    }
}