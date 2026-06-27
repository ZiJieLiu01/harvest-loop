using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Data")]
public class InventoryItemData : BaseData
{
    [Header("Inventory Item Data")]
    public Sprite icon;
    public int maxStack = 64;
    public GameObject selectedItemPreviewPrefab;    // Can be null
    public int worthAmount = 1;
    public bool sellable = true;
    public bool consumeOnUse = true;

    // Getter
    public bool IsStackable => maxStack > 1;
}