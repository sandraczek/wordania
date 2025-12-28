using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float actionRange = 6f;
    private float actionRate = 0.05f;
    private float minePower = 1f; 
    private float nextActionTime;
    [SerializeField] private TileBase buildingBlock;
    private enum InteractionMode
    {
        mine,
        build
    }
    private InteractionMode interactionMode = InteractionMode.build;
    public bool HandleInteraction(Vector2 targetWorldPos)
    {
        if (Time.time < nextActionTime) return false;

        // checking range
        float deltaRoundX = Mathf.Abs(Mathf.Round(targetWorldPos.x - 0.5f) - Mathf.Round(transform.position.x));
        float deltaRoundY = Mathf.Abs(Mathf.Round(targetWorldPos.y - 0.5f) - 2f - Mathf.Round(transform.position.y)); // distance from arms so -2f
        if (deltaRoundX > actionRange || deltaRoundY > actionRange) return false;
        
        switch (interactionMode){
            case InteractionMode.build:
                if(!WorldManager.Instance.TryPlaceBlock(targetWorldPos, buildingBlock)) return false;
                break;
            case InteractionMode.mine:
                if(!WorldManager.Instance.TryDamageBlock(targetWorldPos, minePower)) return false;
                break;
        }
        nextActionTime = Time.time + actionRate;
        return true;
    }
    public void CycleInteractionMode()
    {
        if(interactionMode == InteractionMode.build) {
            interactionMode = InteractionMode.mine;
            return;
        }
        if(interactionMode == InteractionMode.mine){
            interactionMode = InteractionMode.build;
            return;
        }
    }
}
