using UnityEngine;

public class PlaceableHeldItem : HeldItem
{
    private PlaceableData m_placeableData;

    public override void Initialize(PlayerController owner, InventoryItemData data, ItemInstance itemInstance)
    {
        base.Initialize(owner, data, itemInstance);

        m_placeableData = data as PlaceableData;
    }

    public override void PrimaryAction()
    {
        if (m_isBusy)
            return;

        if (m_placeableData == null)
            return;

        Tile tile = TileManager.Instance.CurrHoverTile;
        if (tile == null || tile.Occupant != null)
            return;

        bool placed = TryPlaceObject(tile.GridPos);
        if (placed)
            m_itemInstance.RemoveFromStack(1);
    }

    private bool TryPlaceObject(Vector2Int gridPos)
    {
        return TileObjectManager.Instance.TrySpawnTileObject(gridPos, m_placeableData);
    }
}