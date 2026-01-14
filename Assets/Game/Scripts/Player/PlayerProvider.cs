using UnityEngine;
using System;
public class PlayerProvider : IPlayerProvider
{
    public PlayerContext Current { get; private set; }
    public event Action<PlayerContext> OnPlayerSpawned;
    public event Action OnPlayerDespawned;

    public void Register(PlayerContext context)
    {
        Current = context;
        OnPlayerSpawned?.Invoke(context);
    }

    public void Unregister()
    {
        Current = null;
        OnPlayerDespawned?.Invoke();
    }
}
