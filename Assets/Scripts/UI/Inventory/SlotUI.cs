using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_itemNameText;
    [SerializeField] private TextMeshProUGUI m_itemCountText;
    [SerializeField] private GameObject m_selectionBorder;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private ItemInstance m_item;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if (m_selectionBorder == null)
        {
            Debug.Log("SelectionBorder is null");
            return;
        }

        m_selectionBorder.SetActive(false);
        ResetUI();
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Setter
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void SetItem(ItemInstance item)
    {
        if(m_itemNameText == null)
        {
            Debug.Log("TMP_UGUI is null for item name text");
            return;
        }

        if(m_itemCountText == null)
        {
            Debug.Log("TEP_UGUI is null for item count text");
            return;
        }

        if (item == null)
            return;

        item.OnStackChange += UpdateItemCount;
        m_item = item;

        if(item.Data.icon != null)
        {
            m_icon.enabled = true;
            m_icon.sprite = item.Data.icon;
        }
        else
        {
            m_itemNameText.text = item.Data.displayName;
        }

        m_itemCountText.text = item.ItemCount.ToString();
    }

    public void SetSelected(bool selected)
    {
        if (m_selectionBorder == null)
        {
            Debug.Log("SelectionBorder is null");
            return;
        }

        m_selectionBorder.SetActive(selected);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void UpdateItemCount(int count)
    {
        m_itemCountText.text = count.ToString();
    }

    public void ResetUI()
    {
        m_item = null;
        m_icon.enabled = false;
        m_itemNameText.text = string.Empty;
        m_itemCountText.text = string.Empty;
    }
}
