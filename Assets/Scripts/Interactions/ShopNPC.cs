using UnityEngine;

public class ShopNPC : TileObjectInstance
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serailize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [Header("Shop Setting")]
    [SerializeField] private InventoryItemData[] m_itemForSell;
    [SerializeField] private float m_shopMultiplier = 2;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // TileObjectInstance Implementation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public override void OnSecondaryInteract(IItemReceiver receiver)
    {
        Inventory consumer = receiver as Inventory;     // Should find another solution to solve this instead of just casting
        if (consumer == null)
            return;

        ShopUI.Instance.Show(m_itemForSell, consumer, m_shopMultiplier);
    }
}
