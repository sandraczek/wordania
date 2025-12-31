using UnityEngine;
using Unity.Mathematics;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time >= _ctx.Controller.LastJumpTime + _ctx.Controller.MinJumpDuration && _ctx.Controller.rb.linearVelocityY < 0f)
        {
            _ctx.SwitchState(_factory.Fall);
            return;
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        _ctx.Controller.rb.linearVelocityY = _ctx.Controller.jumpForce;

        _ctx.Controller.JumpPressedTime = float.MinValue;  // jump consume
        _ctx.Controller.LastJumpTime = Time.time;
    }

    public override void ExitState()
    {
        _ctx.Controller.jumpInput = false;
        _ctx.Controller.SetGravity(_ctx.Controller.GravityScale);
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!_ctx.Controller.jumpInput && _ctx.Controller.rb.linearVelocityY > 0)
        {
            _ctx.Controller.SetGravity(_ctx.Controller.GravityScale * _ctx.Controller.LowJumpGravityMultiplier);
        }
    }
}