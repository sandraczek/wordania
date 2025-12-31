using UnityEngine;

public class PlayerGroundState : PlayerActiveState
{
    public PlayerGroundState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time < _ctx.Controller.JumpPressedTime + _ctx.Controller.Config.JumpBuffor) 
        {
            _ctx.SwitchState(_factory.Jump);
            return;
        }
        if (Time.time > _ctx.Controller.LastGroundedTime + _ctx.Controller.Config.CoyoteTime)
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
            float targetSpeed = Mathf.Sign(_ctx.Controller.MovementInput.x) * _ctx.Controller.Config.MoveSpeed;
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, targetSpeed, _ctx.Controller.Config.AccelerationSpeed * _ctx.Controller.Config.MoveSpeed);
            _ctx.Controller.TryStepUp(_ctx.Controller.MovementInput.x);
        }
        else
        {
            _ctx.Controller.RB.linearVelocityX = Mathf.MoveTowards(_ctx.Controller.RB.linearVelocityX, 0f, _ctx.Controller.Config.StoppingSpeed * _ctx.Controller.Config.MoveSpeed);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
