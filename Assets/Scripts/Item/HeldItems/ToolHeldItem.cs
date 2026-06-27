using UnityEngine;

public class ToolHeldItem : HeldItem
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime State
    // ----------------------------------------------------------------------------------------------------------------------------------

    private ToolData m_toolData;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Tool Implementation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public override void Initialize(PlayerController owner, InventoryItemData data, ItemInstance itemInstance)
    {
        base.Initialize(owner, data, itemInstance);

        if(data is ToolData toolData)
        {
            m_toolData = toolData;
        }
    }

    public override void PrimaryAction()
    {
        if (m_isBusy)
            return;

        TileManager tileManager = TileManager.Instance;
        if (tileManager == null)
            return;

        Tile currHoverTile = tileManager.CurrHoverTile;
        if (currHoverTile == null)
            return;

        StartCoroutine(ItemActionRoutine());
        tileManager.ApplyAction(m_owner.Inventory, currHoverTile.GridPos, m_toolData);
    }
}