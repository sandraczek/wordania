using UnityEngine;
using Unity.Mathematics;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
    }

    public override void EnterState()
    {
        base.EnterState();
        _ctx.Controller.SetGravity(_ctx.Controller.GravityScale * _ctx.Controller.FallGravityMult);
    }

    public override void ExitState()
    {
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
    }
}
