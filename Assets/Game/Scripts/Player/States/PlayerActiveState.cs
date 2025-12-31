using UnityEngine;

public class PlayerActiveState : PlayerBaseState
{
    public override bool CanPerformActions => true;
    public override bool CanSetSlot => true;
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

    }
}
