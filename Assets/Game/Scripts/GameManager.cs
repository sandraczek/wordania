using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private GameObject _playerPrefab;
    private Player _player;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private CameraManager _cameraManager;
    [Range(1,9)]
    [SerializeField] private int _saveSlot = 1;

    void Awake()
    {
        _blockDatabase.Initialize();
        _itemDatabase.Initialize();
        _worldManager.Initialize(_blockDatabase);
    }
    public void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }
    private IEnumerator StartGameCoroutine() 
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        SaveData data = _saveManager.Load(_saveSlot);
        if(data == null)
        {
            RandomizeSeed();
            _worldManager.StartWorldGeneration();

            SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
            // here starting profile for player
        }
        else
        {
            _worldManager.LoadWorldData(data.worldData);

            SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
            _player.LoadData(data.playerData);
            _player.Inventory.LoadInventory(data.Inventory);
        }
        _player.Data.Health = 100f;
        _player.Data.MaxHealth = 100f;
        _player.Initialize();

        yield return new WaitForFixedUpdate();

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    private IEnumerator RestartGameCoroutine() 
    {
        int saveSlot = _saveSlot; 
        Physics2D.simulationMode = SimulationMode2D.Script;
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!op.isDone) yield return null;

        SaveData data = _saveManager.Load(saveSlot);
        if(data == null)
        {
            RandomizeSeed();
            _worldManager.StartWorldGeneration();

            SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
            // here starting profile for player
        }
        else
        {
            _worldManager.LoadWorldData(data.worldData);

            SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
            _player.LoadData(data.playerData);
            _player.Inventory.LoadInventory(data.Inventory);
        }

        yield return new WaitForFixedUpdate();

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }

    private void RandomizeSeed()
    {
        _worldManager.Settings.Seed = System.DateTime.Now.Millisecond + Random.Range(1, 1000000); // randomize seed
    }

    [ContextMenu("SaveGame")]
    public void SaveGame()
    {
        SaveData data = new();
        data.worldData = _worldManager.Data;
        data.Inventory = _player.Inventory.SaveInventory();
        data.playerData = _player.Data;
        _saveManager.Save(data, _saveSlot);
    }
    [ContextMenu("Delete save file")]
    public void DeleteSaveFile()
    {
        _saveManager.DeleteSaveFile(_saveSlot);
    }
    [ContextMenu("Load Game")]
    public void RestartGame()
    {
        StartCoroutine(RestartGameCoroutine());
    }
    private void SpawnPlayer(Vector2 position)
    {
        Debug.Assert(_worldManager != null);
        _player = Instantiate(_playerPrefab, position, Quaternion.identity).GetComponent<Player>();
        _cameraManager.InitializePlayer(_player);
        _player.GetComponent<PlayerInteraction>().InitializeWorldManager(_worldManager);
    }
    public void UpdatePlayerMenuState(bool isInMenu)
    {
        if (_player != null)
        {
            _player.States.ToggleInMenuState(isInMenu);
        }
    }
}

