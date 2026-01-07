using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public WorldData worldData;
    public PlayerData playerData;

    public List<ItemSaveData> Inventory = new();
    
    public string SaveDate;
    public int SaveVersion = 1;
}

[Serializable]
public struct ItemSaveData
{
    public int id;
    public int amount;
}