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
        if (_ctx.Controller.rb.linearVelocityY < 0f)
        {
            _ctx.SwitchState(_factory.Fall);
            return;
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        _ctx.Controller.rb.linearVelocityY = _ctx.Controller.jumpForce;
        _ctx.Controller.jumpTriggered = false;
    }

    public override void ExitState()
    {
        _ctx.Controller.jumpInput = false;
        _ctx.Controller.SetGravity(_ctx.Controller.gravityScale);
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
            _ctx.Controller.SetGravity(_ctx.Controller.gravityScale * _ctx.Controller.lowJumpGravityMultiplier);
        }
    }
}