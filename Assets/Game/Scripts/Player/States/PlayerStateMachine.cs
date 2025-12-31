using UnityEngine;
public class PlayerStateMachine : MonoBehaviour
{
    // Referencje
    [HideInInspector] public PlayerController Controller { get; private set; }

    // Stan
    public PlayerBaseState CurrentState {get;private set;}
    [field: SerializeField] private string currentStateName;
    private PlayerStateFactory _factory;

    void Awake()
    {
        Controller = GetComponent<PlayerController>();
        _factory = new PlayerStateFactory(this);
        CurrentState = _factory.Idle; // Startowy stan
        currentStateName = _factory.Idle.GetType().Name;
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
        currentStateName = newState.GetType().Name;
        CurrentState.EnterState();
    }
}