using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public InputReader Inputs;

    private PlayerController _controller;
    public PlayerController Controller => _controller = _controller != null ? _controller : GetComponent<PlayerController>();

    private PlayerStateMachine _states;
    public PlayerStateMachine States => _states = _states != null ? _states : GetComponent<PlayerStateMachine>();
    [field:SerializeField] public PlayerConfig Config {get; private set;}
    [field:SerializeField] public InventoryData Inventory {get; private set;}
    private PlayerData _data;
    public PlayerData Data { get 
    {
        _data.SetPosition(transform.position); 
        return _data;
    } }

    public void Awake()
    {
        Controller.Initialize(Config);
        _data = new();
    }
    public void LoadData(PlayerData data)
    {
        _data = data;
        _controller.Warp(_data.Position);
    }
}
