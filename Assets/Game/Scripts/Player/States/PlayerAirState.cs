using System;
using UnityEngine;

public class PlayerAirState : PlayerActiveState
{
    public PlayerAirState(PlayerContext context, IInputReader inputs, PlayerStateFactory playerStateFactory) : base(context, inputs, playerStateFactory){}

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time >= _context.Controller.LastJumpTime + _context.Config.MinJumpDuration && _context.Controller.IsGrounded)
        {
            if(Math.Abs(_inputs.MovementInput.x) > 0.1f)
            {
                _context.States.SwitchState(_factory.Run);
                return;
            }
            else{
                _context.States.SwitchState(_factory.Idle);
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
        ApplyStandardMovement(_context.Config.AirAccelerationSpeed, _context.Config.AirStoppingSpeed, _context.Config.MoveSpeedAirMult);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _context.Controller.CheckForFlip(_inputs.MovementInput.x);
    }
}
