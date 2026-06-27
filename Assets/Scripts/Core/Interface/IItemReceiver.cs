using UnityEngine;

public interface IItemReceiver
{
    /// <summary>
    /// Receive an item.
    /// </summary>
    /// <param name="itemData">The type of item</param>
    /// <param name="amount">How many to receive</param>
    /// <returns>0 if no left over, else the number is the left over</returns>
    public int ReceiveItem(InventoryItemData itemData, int amount = 1);
}
