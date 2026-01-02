using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ChunkDebugger : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private Vector2 chunkSize = new Vector2(16, 16);

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 4;
        _lineRenderer.useWorldSpace = false;

        _lineRenderer.SetPositions(new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(chunkSize.x, 0, 0),
            new Vector3(chunkSize.x, chunkSize.y, 0),
            new Vector3(0, chunkSize.y, 0)
        });

        // Ukrywamy na starcie
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        bool shouldShow = DebugManager.Instance.Settings.ShowChunks;
        
        if (_lineRenderer.enabled != shouldShow)
        {
            _lineRenderer.enabled = shouldShow;
            _lineRenderer.startColor = DebugManager.Instance.Settings.ChunkColor;
            _lineRenderer.endColor = DebugManager.Instance.Settings.ChunkColor;
        }
    }
}