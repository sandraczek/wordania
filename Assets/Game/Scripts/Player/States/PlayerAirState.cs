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
        if (Time.time >= _ctx.Controller.LastJumpTime + _ctx.Controller.MinJumpDuration && _ctx.Controller.IsGrounded)
        {
            if(Math.Abs(_ctx.Controller.movementInput.x) > 0.1f)
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
        if(Math.Abs(_ctx.Controller.movementInput.x) > 0.1f){
            float targetSpeed = Mathf.Sign(_ctx.Controller.movementInput.x) * _ctx.Controller.moveSpeed * _ctx.Controller.moveSpeedAirMult;
            _ctx.Controller.rb.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.rb.linearVelocityX, targetSpeed,_ctx.Controller.airAccelerationSpeed * _ctx.Controller.moveSpeed * _ctx.Controller.moveSpeedAirMult);
        }
        else
        {
            _ctx.Controller.rb.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.rb.linearVelocityX, 0f, _ctx.Controller.airStoppingSpeed * _ctx.Controller.moveSpeed * _ctx.Controller.moveSpeedAirMult); 
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _ctx.Controller.CheckForFlip();
    }
}
