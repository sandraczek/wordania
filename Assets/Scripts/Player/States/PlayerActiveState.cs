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
        if (_ctx.Controller.InteractionTriggered)
        {
            _ctx.Controller.InteractionTriggered = false;
        }

        if (_ctx.Controller.InteractionInput)
        {
            Debug.Log(true);
            _ctx.Controller.Interaction.HandleInteraction(_ctx.Controller.GetWorldAimPosition());
        }
    }
}
