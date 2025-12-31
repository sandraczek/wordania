using Unity.Mathematics;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if(_ctx.Controller.MovementInput.x == 0f)
        {
            _ctx.SwitchState(_factory.Idle);
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
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _ctx.Controller.CheckForFlip();
    }
}
