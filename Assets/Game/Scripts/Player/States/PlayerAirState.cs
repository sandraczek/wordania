using System;
using UnityEngine;

public class PlayerAirState : PlayerActiveState
{
    public PlayerAirState(Player player, PlayerStateFactory factory) : base(player, factory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time >= _player.Controller.LastJumpTime + _player.Config.MinJumpDuration && _player.Controller.IsGrounded)
        {
            if(Math.Abs(_player.Inputs.MovementInput.x) > 0.1f)
            {
                _player.States.SwitchState(_factory.Run);
                return;
            }
            else{
                _player.States.SwitchState(_factory.Idle);
                return;
            }
        }
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        ApplyStandardMovement(_player.Config.AirAccelerationSpeed, _player.Config.AirStoppingSpeed, _player.Config.MoveSpeedAirMult);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.Controller.CheckForFlip(_player.Inputs.MovementInput.x);
    }
}
