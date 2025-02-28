using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorData : ItemData
{
    public ArmorType armorType;
    public int defense;
    private void OnEnable() { maxStack = 1; }
}