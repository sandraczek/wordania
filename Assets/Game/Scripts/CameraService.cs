using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraService : MonoBehaviour, ICameraService
{
    private CinemachineCamera _vcam;
    private Transform _target;

    public void Awake()
    {
        _vcam = GetComponent<CinemachineCamera>();
    }
    public void FollowTarget(Transform target)
    {
        _target = target;
        _vcam.Follow = target;
        Debug.Log($"<color=cyan>[Camera]</color> New Target: {target.name}");
    }
    public void Warp(Vector3 delta)
    {
        if (_target == null) return;
        _vcam.OnTargetObjectWarped(_target, delta);
    }
}
