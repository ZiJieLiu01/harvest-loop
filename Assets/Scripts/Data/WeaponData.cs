using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/WeaponItemData/Melee")]
public class WeaponData : InventoryItemData
{
    [Header("Weapon Data")]
    public int baseDamage = 10;
    public float baseAttackSpeed = 1f;
    public float baseKnockback = 1f;
    public float swingAngle = 90f;
}