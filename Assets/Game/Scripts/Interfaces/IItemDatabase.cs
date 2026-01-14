using System;
using UnityEngine;
public interface IItemDatabase
{
    ItemData GetItem(int id);
}