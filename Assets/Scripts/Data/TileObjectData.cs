using UnityEngine;

[CreateAssetMenu(menuName = "Tile Object/Tile Object Data")]
public class TileObjectData : BaseData
{
    [Header("World Object Data")]
    public int maxHealth;

    public bool blocksMovement;
    public TileType spawnableTile;  // Could be a array/list later

    public Sprite sprite;
    public Vector2 spriteOffset;

    public ToolType requiredTool;

    public InventoryItemData dropItem;

    public TileObjectInstance tileObjectPrefab;
}