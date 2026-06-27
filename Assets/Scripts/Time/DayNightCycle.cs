using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private Image m_nightOverlay;

    [Header("Settings")]
    [SerializeField] private float m_smoothSpeed = 2f;
    [SerializeField] private float m_maxDarkness = 0.8f;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private float m_targetAlpha;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        TimeManager.Instance.OnHourChange += UpdateLightning;
        TimeManager.Instance.OnDayChange += OnDayChange;
        OnDayChange(1);
        UpdateLightning(TimeManager.Instance.Hour);
    }

    private void Update()
    {
        Color newColor = m_nightOverlay.color;
        newColor.a = Mathf.Lerp(newColor.a, m_targetAlpha, Time.deltaTime * m_smoothSpeed);
        m_nightOverlay.color = newColor;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void UpdateLightning(int hour)
    {
        m_targetAlpha = GetDarknessFromHour(hour);
    }

    private void OnDayChange(int day)
    {
        Color newColor = m_nightOverlay.color;
        newColor.a = 0;
        m_nightOverlay.color = newColor;
    }

    private float GetDarknessFromHour(int hour)
    {
        // 6 AM -> 6 PM (day)
        if (hour >= 6 && hour < 18)
            return 0f;

        // sunset (18 -> 22)
        if (hour >= 18 && hour < 22)
        {
            float t = (hour - 18) / 4f;
            return t * m_maxDarkness;
        }

        // night (22 -> 4)
        if (hour >= 22 || hour < 4)
            return m_maxDarkness;

        // sunrise (4 -> 6)
        if (hour >= 4 && hour < 6)
        {
            float t = (hour - 4) / 2f;
            return (1f - t) * m_maxDarkness;
        }

        return 0f;
    }
}
