using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Data/Placeable")]
public class PlaceableData : InventoryItemData
{
    [Header("Placeable Data")]
    public bool blocksMovement;

    public InventoryItemData dropItem;

    public TileObjectData tileObjectData;
}