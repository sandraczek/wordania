using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISaveable
{
    string PersistenceId { get; }
    object CaptureState();
    void RestoreState(object state);
}

[System.Serializable]
public class GameSaveSnapshot
{
    public Dictionary<string, object> StateData { get; set; } = new();
    public System.DateTime Timestamp { get; set; }
    public int Version { get; set; }

    public GameSaveSnapshot()
    {
        Timestamp = System.DateTime.UtcNow;
        Version = 1;
    }
}