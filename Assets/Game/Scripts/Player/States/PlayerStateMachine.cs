using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{
    private Player _player;
    public PlayerBaseState CurrentState {get;private set;}
    private PlayerStateFactory _factory;

    void Awake()
    {
        _player = GetComponent<Player>();
        _factory = new PlayerStateFactory(_player);
        CurrentState = _factory.Idle;
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
            SwitchState(_factory.InMenu); 
        }
        else
        {
            SwitchState(_factory.Idle);
        }
    }
}