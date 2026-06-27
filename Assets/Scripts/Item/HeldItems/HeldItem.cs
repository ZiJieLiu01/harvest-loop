using System.Collections;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private float m_itemActionRotationEnd;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    protected PlayerController m_owner;
    protected InventoryItemData m_data;
    protected Transform m_transform;
    protected bool m_isBusy = false;
    protected ItemInstance m_itemInstance;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Init
    // ----------------------------------------------------------------------------------------------------------------------------------

    public virtual void Initialize(PlayerController owner, InventoryItemData data, ItemInstance itemInstance)
    {
        m_owner = owner;
        m_data = data;
        m_itemInstance = itemInstance;

        m_transform = transform;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Use Item Implementation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public virtual void PrimaryAction() 
    {

    }

    public virtual void SecondaryAction() 
    {
        TileManager tileManager = TileManager.Instance;
        if (tileManager == null)
            return;

        Tile currHoverTile = tileManager.CurrHoverTile;
        if (currHoverTile == null)
            return;

        IInteractable currOccupant = currHoverTile.Occupant;
        if (currOccupant == null)
            return;

        currOccupant.OnSecondaryInteract(m_owner.Inventory);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Rotations
    // ----------------------------------------------------------------------------------------------------------------------------------

    protected IEnumerator ItemActionRoutine()
    {
        m_isBusy = true;

        Quaternion startRot = m_transform.localRotation;
        Quaternion endRot = Quaternion.Euler(0, 0, m_itemActionRotationEnd);

        float timer = 0;

        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            m_transform.localRotation = Quaternion.Lerp(startRot, endRot, timer / 0.1f);

            yield return null;
        }

        timer = 0;

        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            m_transform.localRotation = Quaternion.Lerp(endRot, startRot, timer / 0.1f);

            yield return null;
        }

        m_isBusy = false;
    }
}
