using UnityEngine;

[CreateAssetMenu(fileName = "DebugSettings", menuName = "Game/DebugSettings")]
public class DebugSettings : ScriptableObject
{
    public bool ShowChunks = false;
    public bool GodMode = false;
    public float debugMoveSpeedMultiplier = 2f;
    public Color ChunkColor = Color.cyan;
}
