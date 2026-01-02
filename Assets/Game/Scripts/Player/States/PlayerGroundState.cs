using UnityEngine;

public class PlayerGroundState : PlayerActiveState
{
    public PlayerGroundState(Player player, PlayerStateFactory playerStateFactory) : base(player, playerStateFactory)
    {
    }

    public override void CheckSwitchStates()
    {
        base.CheckSwitchStates();
        if (Time.time < _player.Inputs.JumpPressedTime + _player.Config.JumpBuffor) 
        {
            _player.States.SwitchState(_factory.Jump);
            return;
        }
        if (Time.time > _player.Controller.LastGroundedTime + _player.Config.CoyoteTime)
        {
            _player.States.SwitchState(_factory.Fall);
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
        ApplyStandardMovement(_player.Config.AccelerationSpeed, _player.Config.StoppingSpeed);
        if(Mathf.Abs(_player.Inputs.MovementInput.x) > 0.1f){
            _player.Controller.TryStepUp(_player.Inputs.MovementInput.x);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
