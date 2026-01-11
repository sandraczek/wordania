using UnityEngine;

public class PlayerActiveState : PlayerBaseState
{
    public override bool CanPerformActions => true;
    public override bool CanSetSlot => true;
    public PlayerActiveState(Player player, PlayerStateFactory factory) : base(player, factory)
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

    protected void ApplyStandardMovement(float acceleration, float deceleration, float speedMultiplier = 1f)
    {
        float xInput = _player.Inputs.MovementInput.x;
        float targetSpeed = xInput * _player.Config.MoveSpeed;
        
        float currentAccel = (Mathf.Abs(xInput) > 0.1f) ? acceleration : deceleration;

        float newVelocityX = Mathf.MoveTowards(
            _player.Controller.VelocityX, 
            targetSpeed, 
            currentAccel * _player.Config.MoveSpeed * speedMultiplier
        );

        _player.Controller.VelocityX = newVelocityX;
    }
}
