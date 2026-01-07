using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private Player _player;
    [SerializeField] private SaveManager _saveManager;
    [Range(1,9)]
    [SerializeField] private int saveSlot = 1;

    void Awake()
    {
        _blockDatabase.Initialize();
        _itemDatabase.Initialize();
        _worldManager.Initialize(_blockDatabase);
    }
    void Start() 
    {
        if(!TryLoadGame()){
            RandomizeSeed();
            _worldManager.StartWorldGeneration();
            _player.Controller.Warp(_worldManager.Settings.GridToWorld(_worldManager.Data.SpawnPoint.x, _worldManager.Data.SpawnPoint.y));
        }
        
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
        _saveManager.Save(data, saveSlot);
    }
    private bool TryLoadGame()
    {
        SaveData data = _saveManager.Load(saveSlot);
        if(data == null) return false;
        _worldManager.LoadWorldData(data.worldData);
        _player.LoadData(data.playerData);
        _player.Inventory.LoadInventory(data.Inventory);
        return true;
    }
    [ContextMenu("Delete save file")]
    public void DeleteSaveFile()
    {
        _saveManager.DeleteSaveFile(saveSlot);
    }
    [ContextMenu("Load Game")]
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

