using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStateMachine : MonoBehaviour, IPlayerStateMachine
{
    public PlayerBaseState CurrentState {get;private set;}
    // private PlayerBaseState CurrentState;

    void Initialize(PlayerBaseState initialState)
    {
        CurrentState = initialState;
        CurrentState.EnterState();
    }

    void Update()
    {
        CurrentState.UpdateState();
        CurrentState.CheckSwitchStates();
    }
    
    void FixedUpdate()
    {
        CurrentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}