using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{
    // Referencje
    [HideInInspector] public PlayerController Controller { get; private set; }

    // Stan
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    void Awake()
    {
        Controller = GetComponent<PlayerController>();
        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle; // Startowy stan
    }
    void Start()
    {
        _currentState.EnterState();
    }

    void Update()
    {
        _currentState.UpdateState();
        _currentState.CheckSwitchStates();
    }
    
    void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    {
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
}