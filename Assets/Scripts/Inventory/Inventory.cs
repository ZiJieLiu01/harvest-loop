using NUnit.Framework.Interfaces;
using System;
using UnityEngine;

public class Inventory : MonoBehaviour, IItemReceiver
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serializ Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private int m_startingMoney = 50;
    [SerializeField] private PlayerController m_owner;
    [SerializeField] private int m_inventorySize = 30;
    [SerializeField] private HotbarUI m_hotbarUI;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Action Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    public event Action OnInventoryChange;
    public event Action<int> OnCurrencyChange;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private ItemInstance[] m_slots;
    private int m_currentMoney;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        m_slots = new ItemInstance[m_inventorySize];
        m_currentMoney = m_startingMoney;
    }

    private void Start()
    {
        GameDatabase database = GameManager.Instance.GameDatabase;

        InventoryItemData axeData = database.GetItem("axe");
        InventoryItemData hoeData = database.GetItem("hoe");
        InventoryItemData pickaxeData = database.GetItem("pickaxe");
        InventoryItemData bedData = GameManager.Instance.GameDatabase.GetPlaceable("bed");
        CropData wheatData = database.GetCrop("wheat_seed");

        ReceiveItem(hoeData);
        ReceiveItem(axeData);
        ReceiveItem(pickaxeData);
        ReceiveItem(wheatData, 10);
        ReceiveItem(bedData);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Inventory Logics
    // ----------------------------------------------------------------------------------------------------------------------------------

    public ItemInstance GetItem(int index)
    {
        if (index < 0 || index >= m_slots.Length)
            return null;

        return m_slots[index];
    }

    public int ReceiveItem(InventoryItemData itemData, int amount = 1)
    {
        int remaining = amount;

        remaining = TryStack(itemData, remaining);
        remaining = TryCreateNewStacks(itemData, remaining);

        OnInventoryChange?.Invoke();
        return remaining;
    }

    public void RemoveItemSlot(int slot)
    {
        m_slots[slot] = null;
        m_hotbarUI.RemoveSlotIndex(slot);
        OnInventoryChange?.Invoke();
    }

    public bool TryBuy(InventoryItemData item, int cost)
    {
        if (m_currentMoney < cost)
            return false;

        m_currentMoney -= cost;
        OnCurrencyChange?.Invoke(m_currentMoney);

        ReceiveItem(item);

        return true;
    }

    public void TrySellItemInHover()
    {
        ItemInstance itemInstance = m_slots[m_hotbarUI.SelectedItemIndex];
        if (itemInstance == null)
            return;

        if (itemInstance.Data.sellable == false)
            return;     // Cannot sell item

        itemInstance.RemoveFromStack(1);
        m_currentMoney += itemInstance.Data.worthAmount;
        OnCurrencyChange?.Invoke(m_currentMoney);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private int TryStack(InventoryItemData itemData, int amount)
    {
        if (!itemData.IsStackable)
            return amount;  // Not stackable

        for (int i = 0; i < m_slots.Length; i++)
        {
            ItemInstance slot = m_slots[i];

            if (slot != null && slot.Data == itemData) // Comparing ScriptableObject
            {
                int leftOver = slot.AddToStack(amount);
                if (leftOver <= 0)
                    return 0;
            }
        }

        return amount;
    }

    private int TryCreateNewStacks(InventoryItemData itemData, int amount)
    {
        if (amount <= 0)
            return 0;

        for(int i = 0; i  < m_slots.Length; i++)
        {
            if (m_slots[i] == null)
            {
                int createAmount = Mathf.Min(itemData.maxStack, amount);
                m_slots[i] = new ItemInstance(this, itemData, createAmount, i);

                amount -= createAmount;

                if (amount <= 0)
                    return 0;
            }
        }

        return amount;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters / Setters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public PlayerController Owner => m_owner;
    public int Money => m_currentMoney;
}
