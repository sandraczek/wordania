using UnityEngine;
using System;

[Serializable]
public struct Ingredient
{
    public ItemData item;
    public int amount;
}

[CreateAssetMenu(menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private Ingredient[] _requirements;

    public Ingredient[] Requirements => _requirements;
}