using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public interface IPersistenceProvider
{
    Task SaveAsync<T>(T data, string identifier);
    Task<T> LoadAsync<T>(string identifier);
    void Delete(string identifier);
}

public class JsonPersistenceProvider : IPersistenceProvider
{
    private readonly string _basePath = Application.persistentDataPath;
    private readonly JsonSerializerSettings _settings;

    public JsonPersistenceProvider()
    {
        _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None // Mniejszy rozmiar pliku w produkcji
        };
    }

    public async Task SaveAsync<T>(T data, string identifier)
    {
        string path = Path.Combine(_basePath, $"{identifier}.json");
        string tempPath = path + ".tmp";

        string json = await Task.Run(() => JsonConvert.SerializeObject(data, _settings));
        byte[] dataBytes = Encoding.UTF8.GetBytes(json);

        // Atomic Write Pattern: Najpierw do pliku .tmp, potem zamiana nazw
        using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
        {
            await fs.WriteAsync(dataBytes, 0, dataBytes.Length);
        }

        if (File.Exists(path)) File.Delete(path);
        File.Move(tempPath, path);
    }

    public async Task<T> LoadAsync<T>(string identifier)
    {
        string path = Path.Combine(_basePath, $"{identifier}.json");
        if (!File.Exists(path)) return default;

        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
        using (var sr = new StreamReader(fs))
        {
            string json = await sr.ReadToEndAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(json, _settings));
        }
    }
    public void Delete(string identifier)
{
    string path = Path.Combine(_basePath, $"{identifier}.json");
    string tempPath = path + ".tmp";

    try
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[Persistence] Successfully deleted save file: {identifier}");
        }

        if (File.Exists(tempPath))
        {
            File.Delete(tempPath);
            Debug.Log($"[Persistence] Cleaned up orphaned temporary file for: {identifier}");
        }
    }
    catch (IOException e)
    {
        Debug.LogError($"[Persistence] Failed to delete file {identifier}. System reported: {e.Message}");
    }
    catch (System.Exception e)
    {
        Debug.LogError($"[Persistence] An unexpected error occurred while deleting {identifier}: {e.GetType().Name}");
    }
}
}