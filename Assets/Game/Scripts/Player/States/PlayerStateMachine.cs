using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{
    // Referencje
    [HideInInspector] public PlayerController Controller { get; private set; }

    // Stan
    private PlayerBaseState _currentState;
    [field: SerializeField] private string currentStateName;
    private PlayerStateFactory _factory;

    void Awake()
    {
        Controller = GetComponent<PlayerController>();
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Idle; // Startowy stan
        currentStateName = _factory.Idle.GetType().Name;
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
        currentStateName = newState.GetType().Name;
        _currentState.EnterState();
    }
}