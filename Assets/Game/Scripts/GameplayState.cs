using System;
using UnityEngine;
using VContainer.Unity;
public sealed class GameplayState : IInitializable, IDisposable
{
    private readonly ICameraService _cameraService;
    private readonly Player _player;

    public GameplayState(
        ICameraService cameraService,
        Player player
        )
    {
        _cameraService = cameraService;
        _player = player;
    }

    public void Initialize()
    {
        _cameraService.FollowTarget(_player.transform);

        _player.Controller.OnPlayerWarped += HandlePlayerWarp;
    }

    public void Dispose()
    {
        if (_player != null && _player.Controller != null)
            _player.Controller.OnPlayerWarped -= HandlePlayerWarp;
    }

    private void HandlePlayerWarp(Vector3 delta) => _cameraService.Warp(delta);
}