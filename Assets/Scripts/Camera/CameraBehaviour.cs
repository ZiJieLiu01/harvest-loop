using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Field
    // ----------------------------------------------------------------------------------------------------------------------------------

    [Header("Target")]
    [SerializeField] private Transform m_targetTransform;
    [SerializeField] private float m_posZOffset = -10;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Transform m_transform;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        m_transform = transform;
    }

    private void LateUpdate()
    {
        if (m_targetTransform == null)
            return;     // No Target

        Vector3 pos = m_targetTransform.position;
        pos.z = m_posZOffset;

        m_transform.position = pos;
    }
}
