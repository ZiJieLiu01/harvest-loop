using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSort2D : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Vector2 m_offset = Vector2.zero;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private SpriteRenderer m_spriteRenderer;
    private Transform m_transform;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_transform = transform;
    }

    private void LateUpdate()
    {
        if (m_spriteRenderer == null)
            return;

        Vector2 pos = m_transform.position + (Vector3)m_offset;
        m_spriteRenderer.sortingOrder = Mathf.RoundToInt(-pos.y * 100);
    }
}
