using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static PopupSpawner Instance;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Popup m_popupPrefab;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Health.OnAnyDamage += SpawnPopup;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Spawn Popup
    // ----------------------------------------------------------------------------------------------------------------------------------

    // value should become DamageInfo later
    private void SpawnPopup(int value, Vector3 position)
    {
        if (m_popupPrefab == null)
        {
            Debug.LogError("Popup Prefab not setup correctly!");
        }

        Popup popupGO = Instantiate(m_popupPrefab, position, Quaternion.identity);
        popupGO.Setup(value, Color.white);
    }
}
