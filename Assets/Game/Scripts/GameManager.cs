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
    [SerializeField] private int _saveSlotID = 1;
    [SerializeField] private bool NewGame = true;

    void Awake()
    {
        _blockDatabase.Initialize();
        _itemDatabase.Initialize();
        _worldManager.Setup(_blockDatabase);
    }
    public void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        if(NewGame){
            RandomizeSeed();
            _worldManager.StartWorldGeneration();

            SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
            // here starting profile for player
        }
        else
        {
            SpawnPlayer(new Vector2(0,0));
            LoadGame();

        }

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    // private IEnumerator RestartGameCoroutine() 
    // {
    //     int saveSlot = _saveSlot; 
    //     Physics2D.simulationMode = SimulationMode2D.Script;
    //     AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    //     while (!op.isDone) yield return null;

    //     SaveData data = _saveManager.Load(saveSlot);
    //     if(data == null)
    //     {
    //         RandomizeSeed();
    //         _worldManager.StartWorldGeneration();

    //         SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
    //         // here starting profile for player
    //     }
    //     else
    //     {
    //         _worldManager.LoadWorldData(data.worldData);

    //         SpawnPlayer(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
    //         _player.LoadData(data.playerData);
    //         _player.Inventory.LoadInventory(data.Inventory);
    //     }

    //     yield return new WaitForFixedUpdate();

    //     Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    // }

    private void RandomizeSeed()
    {
        _worldManager.Settings.Seed = System.DateTime.Now.Millisecond + Random.Range(1, 1000000); // randomize seed
    }

    [ContextMenu("SaveGame")]
    public void SaveGame()
    {
        _saveManager.RequestSave();
    }
    [ContextMenu("Delete save file")]
    public void DeleteSaveFile()
    {
        SaveManager.Service.DeleteSaveFile(GetSaveSlot(_saveSlotID));
    }
    public void LoadGame()
    {
        _saveManager.RequestLoad();
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
    private string GetSaveSlot(int id)
    {
        return "SaveSlot_" + id.ToString();
    }
}

