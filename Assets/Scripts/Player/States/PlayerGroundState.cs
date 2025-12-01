using UnityEngine;

public class PlayerGroundState : PlayerActiveState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (_ctx.Controller.JumpTriggered == true) 
        {
            _ctx.SwitchState(_factory.Jump);
            return;
        }
        if (!_ctx.Controller.IsGrounded())
        {
            _ctx.SwitchState(_factory.Fall);
            return;
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
        if(Mathf.Abs(_ctx.Controller.MovementInput.x) > 0.1f){
            float targetSpeed = Mathf.Sign(_ctx.Controller.MovementInput.x) * _ctx.Controller.MoveSpeed;
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, targetSpeed, _ctx.Controller.AccelerationSpeed * _ctx.Controller.MoveSpeed);
        }
        else
        {
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, 0f, _ctx.Controller.StoppingSpeed * _ctx.Controller.MoveSpeed);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
