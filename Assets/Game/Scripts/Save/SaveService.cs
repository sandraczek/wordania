using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SaveService
{
    private readonly IPersistenceProvider _provider;
    private readonly HashSet<ISaveable> _registeredObjects = new();

    public SaveService(IPersistenceProvider provider)
    {
        _provider = provider;
    }

    public void Register(ISaveable saveable) => _registeredObjects.Add(saveable);
    public void Unregister(ISaveable saveable) => _registeredObjects.Remove(saveable);

    public async Task SaveGameAsync(string slotName)
    {
        var snapshot = new GameSaveSnapshot();

        foreach (var saveable in _registeredObjects)
        {
            snapshot.StateData[saveable.PersistenceId] = saveable.CaptureState();
        }

        await _provider.SaveAsync(snapshot, slotName);
        Debug.Log($"[SaveService] Operation successful: {slotName}");
    }

    public async Task LoadGameAsync(string slotName)
    {
        var snapshot = await _provider.LoadAsync<GameSaveSnapshot>(slotName);
        if (snapshot == null) return;

        foreach (var saveable in _registeredObjects)
        {
            if (snapshot.StateData.TryGetValue(saveable.PersistenceId, out var state))
            {
                saveable.RestoreState(state);
            }
        }
    }
    public void DeleteSaveFile(string slotName)
    {
        _provider.Delete(slotName);
    }
}