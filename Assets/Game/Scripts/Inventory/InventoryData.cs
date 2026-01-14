using UnityEngine;
using System;
using System.Collections.Generic;
using VContainer;

public class InventoryData
{
    public readonly Dictionary<int, InventoryEntry> _content = new();
}