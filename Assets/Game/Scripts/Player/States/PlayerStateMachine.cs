using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{
    private Player _player;
    public PlayerBaseState CurrentState {get;private set;}
    public PlayerStateFactory Factory {get;private set;}

    void Awake()
    {
        _player = GetComponent<Player>();
        Factory = new PlayerStateFactory(_player);
        CurrentState = Factory.Idle;
    }
    void Start()
    {
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
    public void ToggleInMenuState(bool isEnteringMenu)
    {
        if (isEnteringMenu)
        {
            SwitchState(Factory.InMenu); 
        }
        else
        {
            SwitchState(Factory.Idle);
        }
    }
}