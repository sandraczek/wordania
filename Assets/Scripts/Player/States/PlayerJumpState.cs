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
        if (_ctx.Controller.RB.linearVelocityY < 0f)
        {
            _ctx.SwitchState(_factory.Fall);
            return;
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        _ctx.Controller.RB.linearVelocityY = _ctx.Controller.JumpForce;
        _ctx.Controller.JumpTriggered = false;
    }

    public override void ExitState()
    {
        _ctx.Controller.JumpInput = false;
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
        if (!_ctx.Controller.JumpInput && _ctx.Controller.RB.linearVelocityY > 0)
        {
            _ctx.Controller.SetGravity(_ctx.Controller.GravityScale * _ctx.Controller.LowJumpGravityMultiplier);
        }
    }
}