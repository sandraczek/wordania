using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private PlayerController _player;

    void Awake()
    {
        _blockDatabase.Initialize();
        _itemDatabase.Initialize();
        _worldManager.Initialize(_blockDatabase);
    }
    void Start() 
    {
        _worldManager.StartWorldGeneration();
        WorldData data = _worldManager.WorldData;
        _player.Warp(_worldManager.Settings.GridToWorld(data.SpawnPoint.x, data.SpawnPoint.y));
    }

    [ContextMenu("RandomizeSeed")]
    public void RandomizeSeed() {
        _worldManager.Settings.Seed = System.DateTime.Now.Millisecond + Random.Range(1, 1000000); // randomize seed
    }
    [ContextMenu("ResetGame")]
    public void ResetGame() {
       _worldManager.StartWorldGeneration();
        WorldData data = _worldManager.WorldData;
        _player.Warp(_worldManager.Settings.GridToWorld(data.SpawnPoint.x, data.SpawnPoint.y));
    }
}

