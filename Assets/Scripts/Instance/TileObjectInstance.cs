using UnityEngine;

public class TileObjectInstance : MonoBehaviour, IInteractable
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Seralize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] protected SpriteRenderer m_spriteRenderer;
    [SerializeField] protected Collider2D m_collider;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    protected TileObjectData m_data;
    protected Vector2Int m_gridPos;

    protected int m_currentHealth;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Setup
    // ----------------------------------------------------------------------------------------------------------------------------------

    public virtual void Init(Vector2Int gridPos, TileObjectData tileObjectData)
    {
        m_data = tileObjectData;
        m_gridPos = gridPos;

        m_currentHealth = m_data.maxHealth;

        if(m_spriteRenderer != null)
        {
            if(tileObjectData.sprite != null)
                m_spriteRenderer.sprite = tileObjectData.sprite;

            m_spriteRenderer.transform.position += (Vector3)tileObjectData.spriteOffset;
        }

        if(m_collider != null)
        {
            if(tileObjectData.blocksMovement)
                m_collider.enabled = true;
            else
                m_collider.enabled = false;
        }
    }

    public virtual void Init(Vector2Int gridPos, PlaceableData placeableData)
    {
        Init(gridPos, placeableData.tileObjectData);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    protected virtual void OnDestroy()
    {
        TileObjectManager.Instance.RemoveTileObject(m_gridPos);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // IInteractable Implemenation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public virtual void OnPrimaryInteract(IItemReceiver receiver, ToolData toolData)
    {
        if (toolData == null)
            return;     // Does nothing

        if (m_data.requiredTool == toolData.toolType)
        {
            TakeDamage(receiver, 1);
        }
    }

    public virtual void OnSecondaryInteract(IItemReceiver receiver)
    {
        // Does nothing for now
    }

    public virtual string GetDebugInfo()
    {
        return $"=== Tile Object ===\n" +
            $"Type: {m_data.displayName}\n" +
            $"HP: {m_currentHealth}/{m_data.maxHealth}\n";
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Take Damage
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void TakeDamage(IItemReceiver receiver, int healthTaken)
    {
        m_currentHealth -= healthTaken;
        if (m_currentHealth <= 0)
        {
            GiveItemTo(receiver);
            Destroy(gameObject);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Give item / Drop Item
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void GiveItemTo(IItemReceiver receiver)
    {
        receiver.ReceiveItem(m_data.dropItem);
    }
}
