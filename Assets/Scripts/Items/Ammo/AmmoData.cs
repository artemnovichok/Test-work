using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ammo")]
public class AmmoData : ItemData
{
    public AmmoType ammoType;
    private void OnEnable() { maxStack = 100; }
}