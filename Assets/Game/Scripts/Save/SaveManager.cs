using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveService Service { get; private set; }

    [SerializeField] private string _defaultSlot = "GlobalSave_01";

    private void Awake()
    {
        var provider = new JsonPersistenceProvider();
        Service = new SaveService(provider);
    }

    public async void RequestSave()
    {
        await Service.SaveGameAsync(_defaultSlot);
    }

    public async void RequestLoad()
    {
        await Service.LoadGameAsync(_defaultSlot);
    }
}