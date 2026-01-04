using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(InputReader))]
public class Player : MonoBehaviour
{
    private InputReader _inputs;
    public InputReader Inputs => _inputs = _inputs != null ? _inputs : GetComponent<InputReader>();

    private PlayerController _controller;
    public PlayerController Controller => _controller = _controller != null ? _controller : GetComponent<PlayerController>();

    private PlayerStateMachine _states;
    public PlayerStateMachine States => _states = _states != null ? _states : GetComponent<PlayerStateMachine>();
    [field:SerializeField] public PlayerConfig Config {get; private set;}
    [field:SerializeField] public InventoryData Inventory {get; private set;}
    public void Awake()
    {
        Controller.Initialize(Config);
    }
}
