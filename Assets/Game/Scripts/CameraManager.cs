using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    private CinemachineCamera vcam;

    public void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }
    public void InitializePlayer(Player player)
    {
        _player = player;
        vcam.Follow = player.transform;
        _player.Controller.OnPlayerWarped -= HandleWarp;
        _player.Controller.OnPlayerWarped += HandleWarp;
    }

    private void OnEnable() 
    {
        if(_player == null) return;
        _player.Controller.OnPlayerWarped -= HandleWarp;
        _player.Controller.OnPlayerWarped += HandleWarp;
    }

    private void OnDisable() 
    {
        if (_player != null)
        {
            if (_player.Controller != null)
            {
                _player.Controller.OnPlayerWarped -= HandleWarp;
            }
        }
    }

    private void HandleWarp(Vector3 delta) 
    {
        vcam.OnTargetObjectWarped(_player.transform, delta);
    }
}
