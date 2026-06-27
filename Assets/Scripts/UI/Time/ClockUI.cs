using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private TextMeshProUGUI m_timeText;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime State
    // ----------------------------------------------------------------------------------------------------------------------------------

    private int m_day;
    private int m_hour;
    private int m_minute;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        TimeManager timeManager = TimeManager.Instance;

        timeManager.OnDayChange += UpdateDay;
        timeManager.OnHourChange += UpdateHour;
        timeManager.OnMinuteChange += UpdateMinute;

        m_hour = timeManager.Hour;
        m_minute = timeManager.Minute;
        m_day = timeManager.Day;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // On Time Change
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void UpdateDay(int day)
    {
        m_day = day;
        RefreshUI();
    }

    private void UpdateHour(int hour)
    {
        m_hour = hour;
        RefreshUI();
    }

    private void UpdateMinute(int minute)
    {
        m_minute = minute;
        RefreshUI();
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // UI
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void RefreshUI()
    {
        m_timeText.text = $"Day {m_day} - {m_hour:00}:{m_minute:00}";
    }
}
