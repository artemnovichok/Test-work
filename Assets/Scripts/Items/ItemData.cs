using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public float weight;
    public int maxStack = 1;
}

public enum AmmoType { Ammo1, Ammo2 }
public enum ArmorType { Head, Torso }
