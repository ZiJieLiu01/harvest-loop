using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [Header("Setup")]
    [SerializeField] private string m_playerTag = "Player";
    [SerializeField] private Health m_health;
    [SerializeField] private Rigidbody2D m_rb2D;
    [SerializeField] private GameObject m_detectedIndicator;

    [Header("Speed")]
    [SerializeField] private float m_moveSpeed = 1f;

    [Header("Attack")]
    [SerializeField] private float m_attackRange = 0.6f;
    [SerializeField] private float m_attackCooldown = 1f;
    [SerializeField] private int m_attackDamage = 1;
    [SerializeField] private float m_attackKnockback = 2f;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Transform m_transform;
    private Transform m_targetTransform;
    private IDamageable m_damageableTarget;
    private bool m_isKnockedBack = false;
    private float m_attackTimer;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        m_transform = transform;

        if (m_detectedIndicator != null)
            m_detectedIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        if (m_health != null)
        {
            m_health.OnDeath += HandleDeath;
            m_health.OnDamaged += HandleOnDamaged;
        }
        else
            Debug.LogError($"{gameObject.name}'s Health Component is not setup correctly!");
    }

    private void OnDisable()
    {
        if (m_health != null)
        {
            m_health.OnDeath -= HandleDeath;
            m_health.OnDamaged -= HandleOnDamaged;
        }
    }

    private void FixedUpdate()
    {
        if (m_targetTransform == null || m_isKnockedBack)
            return;     // No Target

        Vector2 direction = (m_targetTransform.position - m_transform.position).normalized;
        float distance = Vector2.Distance(m_targetTransform.position, m_transform.position);

        if (distance > m_attackRange)
            m_rb2D.MovePosition(m_rb2D.position + direction * m_moveSpeed * Time.fixedDeltaTime);
        else
            TryAttack(direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(m_playerTag))
        {
            StartCoroutine(ShowDetectedIndicator());
            m_targetTransform = other.transform;
            m_damageableTarget = other.GetComponent<IDamageable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(m_playerTag))
        {
            m_targetTransform = null;
            m_damageableTarget = null;
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void HandleDeath()
    {
        Destroy(gameObject);
    }

    private void HandleOnDamaged(int value, Vector2 knockback)
    {
        StartCoroutine(KnockbackRoutine(knockback));
    }

    private IEnumerator ShowDetectedIndicator()
    {
        if (m_detectedIndicator == null)
            yield break;

        m_detectedIndicator.SetActive(true);
        yield return new WaitForSeconds(.5f);
        m_detectedIndicator.SetActive(false);
    }

    private IEnumerator KnockbackRoutine(Vector2 knockback)
    {
        m_isKnockedBack = true;

        m_rb2D.linearVelocity = Vector2.zero;
        m_rb2D.AddForce(knockback, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        m_isKnockedBack = false;
    }

    private void TryAttack(Vector2 direction)
    {
        if (m_attackTimer > 0f)
        {
            m_attackTimer -= Time.fixedDeltaTime;
            return;
        }

        if (m_damageableTarget != null)
        {
            Vector2 knockback = direction * m_attackKnockback;
            m_damageableTarget.TakeDamage(m_attackDamage, knockback);
        }

        m_attackTimer = m_attackCooldown;
    }
}
