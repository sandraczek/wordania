using UnityEngine;
using Unity.Mathematics;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, PlayerStateFactory playerStateFactory) : base(player, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time >= _player.Controller.LastJumpTime + _player.Config.MinJumpDuration && _player.Controller.VelocityY < 0f)
        {
            _player.States.SwitchState(_factory.Fall);
            return;
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        _player.Controller.VelocityY = _player.Config.JumpForce;

        _player.Inputs.ConsumeJump();
        _player.Controller.LastJumpTime = Time.time;
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
        if (!_player.Inputs.JumpInput && _player.Controller.VelocityY > 0f)
        {
            _player.Controller.SetGravity(_player.Config.GravityScale * _player.Config.LowJumpGravityMultiplier);
        }
    }
}