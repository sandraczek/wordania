using System;
using UnityEngine;

public class PlayerAirState : PlayerActiveState
{
    public PlayerAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time >= _ctx.Controller.LastJumpTime + _ctx.Controller.Config.MinJumpDuration && _ctx.Controller.IsGrounded)
        {
            if(Math.Abs(_ctx.Controller.MovementInput.x) > 0.1f)
            {
                _ctx.SwitchState(_factory.Run);
                return;
            }
            else{
                _ctx.SwitchState(_factory.Idle);
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
        if(Math.Abs(_ctx.Controller.MovementInput.x) > 0.1f){
            float targetSpeed = Mathf.Sign(_ctx.Controller.MovementInput.x) * _ctx.Controller.Config.MoveSpeed * _ctx.Controller.Config.MoveSpeedAirMult;
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, targetSpeed,_ctx.Controller.Config.AirAccelerationSpeed * _ctx.Controller.Config.MoveSpeed * _ctx.Controller.Config.MoveSpeedAirMult);
        }
        else
        {
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, 0f, _ctx.Controller.Config.AirStoppingSpeed * _ctx.Controller.Config.MoveSpeed * _ctx.Controller.Config.MoveSpeedAirMult); 
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _ctx.Controller.CheckForFlip();
    }
}
