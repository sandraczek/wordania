using System;
using UnityEngine;
public interface IPlayerProvider
{
    IReadOnlyReactiveProperty<IPlayerFacade> CurrentPlayer { get; }
    void RegisterPlayer(IPlayerFacade player);
    void UnregisterPlayer();
}