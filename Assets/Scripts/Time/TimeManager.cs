using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static TimeManager Instance;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Event Action
    // ----------------------------------------------------------------------------------------------------------------------------------

    public event Action<int> OnDayChange;
    public event Action<int> OnHourChange;
    public event Action<int> OnMinuteChange;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [Header("Game Tick")]
    [SerializeField] private float m_realSecondsPerGameMinute = 1f;

    [Header("Time Setting")]
    [SerializeField] private int m_dayStartHour = 6;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private int m_day;
    private int m_hour;
    private int m_minute;

    private float m_timer;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Event
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        m_hour = m_dayStartHour;
    }

    private void Start()
    {
        NewDay();
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer >= m_realSecondsPerGameMinute)
        {
            m_timer = 0;
            AddMinute();
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void AddMinute()
    {
        m_minute += 1;
        if(m_minute >= 60)
        {
            m_minute = 0;
            AddHour();
        }

        OnMinuteChange?.Invoke(m_minute);
    }

    private void AddHour()
    {
        m_hour += 1;

        if(m_hour >= 24)
        {
            NewDay();
        }

        OnHourChange?.Invoke(m_hour);
    }

    public void NewDay()
    {
        m_day += 1;
        m_minute = 0;
        m_hour = m_dayStartHour;

        OnDayChange?.Invoke(m_day);
        OnHourChange?.Invoke(m_hour);
        OnMinuteChange?.Invoke(m_minute);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public int Day => m_day;
    public int Minute => m_minute;
    public int Hour => m_hour;
}
