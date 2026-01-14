using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISaveService
{
    public void Register(ISaveable saveable);
    public void Unregister(ISaveable saveable);

    public Task SaveGameAsync(string slotName);
    public Task LoadGameAsync(string slotName);
    public void DeleteSaveFile(string slotName);
}
