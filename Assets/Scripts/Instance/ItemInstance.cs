using System;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Event Action
    // ----------------------------------------------------------------------------------------------------------------------------------

    public event Action<int> OnStackChange;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Inventory m_inventory;
    private InventoryItemData m_data;
    private int m_slotIndex;
    private int m_currStack;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Ctor
    // ----------------------------------------------------------------------------------------------------------------------------------

    public ItemInstance(Inventory inventory, InventoryItemData data, int startAmount, int slotIndex)
    {
        m_data = data;
        m_slotIndex = slotIndex;
        m_inventory = inventory;
        AddToStack(startAmount);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Add / Remove
    // ----------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds amount to the current stacks and returns the left over
    /// amount. If no left over, returns 0.
    /// </summary>
    public int AddToStack(int amount)
    {
        int space = m_data.maxStack - m_currStack;

        if (space <= 0)
            return amount;  // no space left

        int addAmount = Mathf.Min(space, amount);
        m_currStack += addAmount;

        OnStackChange?.Invoke(m_currStack);

        return amount - addAmount;
    }

    public void RemoveFromStack(int amount)
    {
        m_currStack -= amount;
        OnStackChange?.Invoke(m_currStack);

        if (m_currStack <= 0)
            m_inventory.RemoveItemSlot(m_slotIndex);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters / Setters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public InventoryItemData Data => m_data;
    public int ItemCount => m_currStack;
    public int SlotIndex => m_slotIndex;
}
