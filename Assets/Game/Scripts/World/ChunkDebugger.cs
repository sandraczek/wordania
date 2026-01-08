using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Chunk))]
public class ChunkDebugger : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private int _chunkSize;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 4;
        _lineRenderer.useWorldSpace = false;
    }
    private void Start()
    {
        _chunkSize = GetComponent<Chunk>().GetChunkSize();
        _lineRenderer.SetPositions(new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(_chunkSize, 0, 0),
            new Vector3(_chunkSize, _chunkSize, 0),
            new Vector3(0, _chunkSize, 0)
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