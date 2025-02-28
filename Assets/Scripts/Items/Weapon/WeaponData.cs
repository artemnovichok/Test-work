using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    public AmmoType usesAmmoType;
    public int damage;
    private void OnEnable() { maxStack = 1; }
}