using UnityEngine;

public sealed class PlayerContext
{
    public readonly PlayerStateMachine States;
    public readonly PlayerController Controller;
    public readonly PlayerHealthView Health;
    public readonly PlayerConfig Config;
    public readonly Transform Transform;

    public PlayerContext(PlayerStateMachine states, PlayerController controller, PlayerHealthView health, PlayerConfig config, Transform transform)
    {
        States = states;
        Controller = controller;
        Health = health;
        Config = config;
        Transform = transform;
    }
}