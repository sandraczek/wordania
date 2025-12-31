using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private WorldGenerator _worldGenerator;
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private WorldRenderer _worldRenderer;
    [SerializeField] private PlayerController _player;
    [SerializeField] private WorldSettings _settings;

    void Start() 
    {

        _blockDatabase.Initialize();
        WorldData data = _worldGenerator.GenerateWorld();
        _worldManager.Initialize(data);
        _worldRenderer.CreateChunks();
        _worldRenderer.RenderWorld(data);
        _player.Warp(_settings.GridToWorld(data.SpawnPoint.x, data.SpawnPoint.y));
    }

    [ContextMenu("RandomizeSeed")]
    public void RandomizeSeed() {
        _settings.Seed = System.DateTime.Now.Millisecond + Random.Range(1, 1000000); // randomize seed
    }
    [ContextMenu("ResetGame")]
    public void ResetGame() {
        WorldData data = _worldGenerator.GenerateWorld();
        _worldManager.Initialize(data);
        _worldRenderer.CreateChunks();
        _worldRenderer.RenderWorld(data);
        _player.Warp(_settings.GridToWorld(data.SpawnPoint.x, data.SpawnPoint.y));
    }
}

