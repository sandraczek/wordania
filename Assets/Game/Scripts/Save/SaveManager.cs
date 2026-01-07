using UnityEngine;
using System.IO;
public class SaveManager : MonoBehaviour
{
    // public SaveManager Instance;
    private string saveFilePrefix = "SaveSlot_";

    public void Awake()
    {
        // if(Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }
    public void Save(SaveData data, int slotIndex)
    {
        SaveJSON(data, saveFilePrefix + slotIndex.ToString());
    }
    public SaveData Load(int slotIndex)
    {
        return LoadJSON<SaveData>(saveFilePrefix + slotIndex.ToString());
    }
    public void DeleteSaveFile(int slotIndex)
    {
        string fileName = saveFilePrefix + slotIndex.ToString() + ".json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save file {fileName} deleted.");
        }
        else
        {
            Debug.LogWarning("Tried deleting a non-existent save file.");
        }
    }

    private string GetPath(string fileName) 
        => Path.Combine(Application.persistentDataPath, fileName + ".json");

    public void SaveJSON<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(fileName), json);
        Debug.Log($"Saved to: {GetPath(fileName)}");
    }

    public T LoadJSON<T>(string fileName)
    {
        string path = GetPath(fileName);
        if (!File.Exists(path)) return default;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}