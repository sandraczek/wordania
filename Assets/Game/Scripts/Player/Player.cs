using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine States {get; private set;}
    public PlayerController Controller {get; private set;}
    public InputReader Inputs {get; private set;}
    [field: SerializeField] public PlayerConfig Config;
    public void Awake()
    {
        States = GetComponent<PlayerStateMachine>();
        Controller = GetComponent<PlayerController>();
        Inputs = GetComponent<InputReader>();

        Controller.Initialize(Config);
        // ZROBIC TO SAMO W WORLD MANAGERZE (tylko jeden world config)
    }
}
