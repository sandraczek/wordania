using UnityEngine;
using VContainer;

[RequireComponent(typeof(Chunk))]
[RequireComponent(typeof(LineRenderer))]
public sealed class ChunkDebugger : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private IDebugService _debugService;
    private DebugSettings _settings;
    private int _chunkSize;

    [Inject]
    public void Construct(IDebugService debugService, DebugSettings settings)
    {
        _debugService = debugService;
        _settings = settings;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _chunkSize = GetComponent<Chunk>().GetChunkSize();
        
        SetupLineRenderer();
    }

    private void Start()
    {
        UpdateVisibility(_settings.ShowChunks);
        
        _debugService.OnShowChunksChanged += UpdateVisibility;
    }

    private void OnDestroy()
    {
        if (_debugService != null)
            _debugService.OnShowChunksChanged -= UpdateVisibility;
    }

    private void UpdateVisibility(bool visible)
    {
        _lineRenderer.enabled = visible;
        if (visible)
        {
            _lineRenderer.startColor = _settings.ChunkColor;
            _lineRenderer.endColor = _settings.ChunkColor;
        }
    }

    private void SetupLineRenderer()
    {
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = 4;
        _lineRenderer.useWorldSpace = false;
        
        _lineRenderer.SetPositions(new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(_chunkSize, 0, 0),
            new Vector3(_chunkSize, _chunkSize, 0),
            new Vector3(0, _chunkSize, 0)
        });
    }
}