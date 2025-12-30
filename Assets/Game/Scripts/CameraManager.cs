using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private CinemachineCamera vcam;

    public void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    private void OnEnable() 
    {
        // "Zapisujemy się" na powiadomienia od gracza
        player.OnPlayerWarped += ctx => {vcam.OnTargetObjectWarped(player.transform, ctx);};
    }

    private void OnDisable() 
    {
        // "Wypisujemy się" (ważne, żeby nie było błędów w pamięci!)
        player.OnPlayerWarped -= HandleWarp;
    }

    private void HandleWarp(Vector3 delta) 
    {
        // Tylko tutaj kamera dowiaduje się, że musi przeskoczyć
        
    }
}
