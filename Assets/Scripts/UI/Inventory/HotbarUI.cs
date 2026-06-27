using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private SlotUI[] m_slots;
    [SerializeField] private Hotbar m_hotbar;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private int m_selectedIndex = 0;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        if (m_hotbar == null || m_hotbar.Inventory == null)
        {
            Debug.LogWarning("Hotbar or Inventory not set!");
            return;
        }

        m_hotbar.OnSelectIndexChange += SetSelectedIndex;
        m_hotbar.Inventory.OnInventoryChange += Refresh;

        Refresh();
        SetSelectedIndex(m_selectedIndex);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Hotbar Logic
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void RemoveSlotIndex(int index)
    {
        m_slots[index].ResetUI();
    }

    public void Refresh()
    {
        if(m_hotbar == null)
        {
            Debug.LogWarning("Hotbar is null");
            return;
        }

        for (int i = 0; i < m_slots.Length; i++)
        {
            ItemInstance item = m_hotbar.Inventory.GetItem(i);
            m_slots[i].SetItem(item);
        }
    }

    public void SetSelectedIndex(int index)
    {
        if(m_slots.Length < index)
        {
            Debug.LogWarning("index > hotbar length");
            return;
        }

        m_selectedIndex = index;

        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].SetSelected(i == index);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getter / Setter
    // ----------------------------------------------------------------------------------------------------------------------------------

    public int SelectedItemIndex => m_selectedIndex;
}
