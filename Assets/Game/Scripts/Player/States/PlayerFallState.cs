using UnityEngine;
using Unity.Mathematics;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player player, PlayerStateFactory playerStateFactory) : base(player, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
    }

    public override void EnterState()
    {
        base.EnterState();
        _player.Controller.SetGravity(_player.Config.GravityScale * _player.Config.FallGravityMult);
    }

    public override void ExitState()
    {
        _player.Controller.SetGravity(_player.Config.GravityScale);
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
