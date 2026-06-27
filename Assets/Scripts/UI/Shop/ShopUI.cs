using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static ShopUI Instance;      // Prob should not be instance but

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Field
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private ShopButton m_shopButtonPrefab;
    [SerializeField] private Transform m_shopListTransform;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime State
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Inventory m_consumer;                   // Who is shopping at the moment
    private List<ShopButton> m_buttonSpawned = new();

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Hide();
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Shop Imeplementation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void ConsumerTryBuy(InventoryItemData item, int price)
    {
        if (m_consumer == null)
            return;     // No consumer

        m_consumer.TryBuy(item, price);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // UI Implemenation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void Show(InventoryItemData[] items, Inventory consumer, float valueableMultiplier)
    {
        m_consumer = consumer;

        foreach (var item in items)
        {
            ShopButton newButton = Instantiate(m_shopButtonPrefab, m_shopListTransform);
            m_buttonSpawned.Add(newButton);

            newButton.Setup(this, item, (int)(item.worthAmount * valueableMultiplier));
        }

        gameObject.SetActive(true);
        InputManager.Instance.PlayerInputAction.Disable();
    }

    // Should be called when "Back" button is pressed
    public void Hide()
    {
        m_consumer = null;
        gameObject.SetActive(false);

        foreach(var button in m_buttonSpawned)
        {
            Destroy(button.gameObject);
        }

        m_buttonSpawned.Clear();
        InputManager.Instance.PlayerInputAction.Enable();
    }
}
