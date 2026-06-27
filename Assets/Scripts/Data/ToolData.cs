using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool")]
public class ToolData : InventoryItemData
{
    [Header("Tool Data")]
    public ToolType toolType;
}

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
    Hoe,
}
