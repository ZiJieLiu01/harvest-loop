using System.Collections;
using UnityEngine;

public class SwordHeldItem : HeldItem
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Field
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Collider2D m_collider2D;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime State
    // ----------------------------------------------------------------------------------------------------------------------------------

    private WeaponData m_meleeData;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Init
    // ----------------------------------------------------------------------------------------------------------------------------------

    public override void Initialize(PlayerController owner, InventoryItemData data, ItemInstance itemInstance)
    {
        base.Initialize(owner, data, itemInstance);

        if(data is WeaponData meleeData)
            m_meleeData = meleeData;

        if (m_collider2D != null)
            m_collider2D.enabled = false;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Sword Imeplementation
    // ----------------------------------------------------------------------------------------------------------------------------------

    public override void PrimaryAction()
    {
        if (m_isBusy)
            return;

        StartCoroutine(SwingCoroutine());
    }

/*    public override void SecondaryAction()
    {
        Block();
    }*/

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable == null)
            return;

        damageable.TakeDamage(m_meleeData.baseDamage, m_owner.GetFacingVector() * m_meleeData.baseKnockback);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private IEnumerator SwingCoroutine()
    {
        m_isBusy = true;

        float duration = 1f / m_meleeData.baseAttackSpeed;
        float timer = 0f;

        Vector2 facing = m_owner.GetFacingVector();
        float startAngle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg - m_meleeData.swingAngle / 2f;
        float endAngle = startAngle + m_meleeData.swingAngle;

        if (m_collider2D != null)
            m_collider2D.enabled = true;
        else
            Debug.Log($"{gameObject.name}'s Collider cannot be found!");

        while (timer < duration)
        {
            float t = timer / duration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            m_transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            timer += Time.deltaTime;
            yield return null;
        }

        if (m_collider2D != null)
            m_collider2D.enabled = false;

        m_isBusy = false;

        m_transform.rotation = Quaternion.identity;
    }

    private void Block()
    {
        // STILL IN CONSIDERATION
    }
}
