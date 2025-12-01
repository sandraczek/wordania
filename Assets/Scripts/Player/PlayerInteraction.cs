using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _range = 4f;
    private float actionRate = 0.2f;
    private float nextActionTime;

    [field: Header("TEMPORARY")]
    [SerializeField] private TileBase temporaryTileBase;

    public bool HandleInteraction(Vector2 targetWorldPos)
    {
        if (Time.time < nextActionTime) return false;
        if (Vector2.Distance(transform.position, targetWorldPos) > _range) return false;

        if(!WorldManager.Instance.TryPlaceBlock(targetWorldPos, temporaryTileBase)) return false;
        nextActionTime = Time.time + actionRate;
        return true;
    }
}
