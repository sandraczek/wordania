using UnityEngine;

public class PlayerHurtState : PlayerBaseState
{
    public override bool CanSetSlot => true;
    private float _hitstunDuration = 0.2f;
    private float _hitTime;

    public PlayerHurtState(Player player, PlayerStateFactory factory) : base(player, factory)
    {
    }
    public override void CheckSwitchStates()
    {
        if (Time.time >= _hitstunDuration + _hitTime)
        {
            DetermineNextState();
        }
    }
    public override void EnterState()
    {
        _hitTime = Time.time;
        //Knockback

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }
    private void DetermineNextState()
{
    if (!_player.Controller.IsGrounded)
    {
        _player.States.SwitchState(_factory.Fall);
        return;
    }

    if (Mathf.Abs(_player.Inputs.MovementInput.x) > 0.1f)
    {
        _player.States.SwitchState(_factory.Run);
        return;
    }

    _player.States.SwitchState(_factory.Idle);
}
}
