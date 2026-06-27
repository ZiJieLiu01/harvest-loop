using UnityEngine;

public class SellBin : TileObjectInstance
{
    public override void OnSecondaryInteract(IItemReceiver receiver)
    {
        Inventory consumer = receiver as Inventory;     // Should find another solution to solve this instead of just casting
        if (consumer == null)
            return;

        consumer.TrySellItemInHover();
    }
}
