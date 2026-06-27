using UnityEngine;
using UnityEngine.Rendering;

public class ToolDepthSort2D : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private SortingGroup m_sortingGroup;
    private SpriteRenderer m_playerSpriteRenderer;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        m_sortingGroup = GetComponent<SortingGroup>();
        m_playerSpriteRenderer = transform.GetComponentInParent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (m_sortingGroup == null || m_playerSpriteRenderer == null)
            return;

        m_sortingGroup.sortingOrder = Mathf.RoundToInt(m_playerSpriteRenderer.sortingOrder + 1);
    }
}
