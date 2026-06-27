using UnityEngine;

public class SeedHeldItem : HeldItem
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Seed Implementation
    // ----------------------------------------------------------------------------------------------------------------------------------

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

        if (m_data is CropData cropData)
        {
            bool planted = TileObjectManager.Instance.TrySpawnTileObject(currHoverTile.GridPos, cropData);
            if (planted)
                m_itemInstance.RemoveFromStack(1);
        }
    }
}
