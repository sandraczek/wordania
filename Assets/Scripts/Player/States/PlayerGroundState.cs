using UnityEngine;

public class PlayerGroundState : PlayerActiveState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (_ctx.Controller.jumpTriggered == true) 
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
        if(Mathf.Abs(_ctx.Controller.movementInput.x) > 0.1f){
            float targetSpeed = Mathf.Sign(_ctx.Controller.movementInput.x) * _ctx.Controller.moveSpeed;
            _ctx.Controller.rb.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.rb.linearVelocityX, targetSpeed, _ctx.Controller.accelerationSpeed * _ctx.Controller.moveSpeed);
        }
        else
        {
            _ctx.Controller.rb.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.rb.linearVelocityX, 0f, _ctx.Controller.stoppingSpeed * _ctx.Controller.moveSpeed);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
