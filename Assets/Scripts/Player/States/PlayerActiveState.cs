using UnityEngine;

public class PlayerActiveState : PlayerBaseState
{
    public PlayerActiveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {

    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        if (_ctx.Controller.interactionTriggered)
        {
            _ctx.Controller.interactionTriggered = false;
        }

        if (_ctx.Controller.interactionInput)
        {
            _ctx.Controller.interaction.HandleInteraction(_ctx.Controller.GetWorldAimPosition());
        }
    }
}
