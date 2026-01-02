using Unity.Mathematics;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(Player player, PlayerStateFactory playerStateFactory) : base(player, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if(_player.Inputs.MovementInput.x == 0f)
        {
            _player.States.SwitchState(_factory.Idle);
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
        _player.Controller.CheckForFlip(_player.Inputs.MovementInput.x);
    }
}
