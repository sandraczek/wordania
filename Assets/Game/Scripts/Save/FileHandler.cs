using System.IO;
using UnityEngine;

public static class FileHandler
{
    private static string GetPath(string fileName) 
        => Path.Combine(Application.persistentDataPath, fileName + ".json");

    public static void SaveJSON<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(fileName), json);
        Debug.Log($"Saved to: {GetPath(fileName)}");
    }

    public static T LoadJSON<T>(string fileName)
    {
        string path = GetPath(fileName);
        if (!File.Exists(path)) return default;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}