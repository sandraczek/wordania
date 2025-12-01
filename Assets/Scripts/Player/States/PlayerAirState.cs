using System;
using UnityEngine;

public class PlayerAirState : PlayerActiveState
{
    float jumpTrigerTimer = 0f;
    public PlayerAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (_ctx.Controller.IsGrounded())
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
            float targetSpeed = Mathf.Sign(_ctx.Controller.MovementInput.x) * _ctx.Controller.MoveSpeed * _ctx.Controller.MoveSpeedAirMult;
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, targetSpeed,_ctx.Controller.AirAccelerationSpeed * _ctx.Controller.MoveSpeed * _ctx.Controller.MoveSpeedAirMult);
        }
        else
        {
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, 0f, _ctx.Controller.AirStoppingSpeed * _ctx.Controller.MoveSpeed * _ctx.Controller.MoveSpeedAirMult); 
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _ctx.Controller.CheckForFlip();
        if (_ctx.Controller.JumpTriggered)
        {
            jumpTrigerTimer+=Time.deltaTime;
            if(jumpTrigerTimer > _ctx.Controller.JumpBuffor)
            {
                jumpTrigerTimer = 0f;
                _ctx.Controller.JumpTriggered = false;
            }
        }
    }
}
