using UnityEngine;

public class PlayerInMenuState : PlayerBaseState
{
    public override bool CanPerformActions => true;
    public override bool CanSetSlot => true;
    public PlayerInMenuState(Player player, PlayerStateFactory factory) : base(player, factory)
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
