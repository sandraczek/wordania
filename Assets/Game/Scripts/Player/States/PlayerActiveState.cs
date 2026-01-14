using UnityEngine;

public class PlayerActiveState : PlayerBaseState
{
    public override bool CanPerformActions => true;
    public override bool CanSetSlot => true;
    public PlayerActiveState(PlayerContext context, IInputReader inputs, PlayerStateFactory playerStateFactory) : base(context, inputs, playerStateFactory){}

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
        float xInput = _inputs.MovementInput.x;
        float targetSpeed = xInput * _context.Config.MoveSpeed;
        
        float currentAccel = (Mathf.Abs(xInput) > 0.1f) ? acceleration : deceleration;

        float newVelocityX = Mathf.MoveTowards(
            _context.Controller.VelocityX, 
            targetSpeed, 
            currentAccel * _context.Config.MoveSpeed * speedMultiplier
        );

        _context.Controller.VelocityX = newVelocityX;
    }
}
