using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static DebugManager Instance;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Action Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    public event Action<bool> OnDebugToggled;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private bool m_showDebug = false;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.ToggleDebug.performed += OnToggleDebug;
        }
    }

    private void OnDisable()
    {
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.ToggleDebug.performed -= OnToggleDebug;
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Handle Key Press
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void OnToggleDebug(InputAction.CallbackContext ctx)
    {
        m_showDebug = !m_showDebug;
        if (m_showDebug)
            Debug.Log("Debug is turned on");
        else
            Debug.Log("Debug is turned off");

        OnDebugToggled?.Invoke(m_showDebug);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Debug Log
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void Log(string message)
    {
        if (!m_showDebug)
            return;     // Debug is turned off

        Debug.Log($"[DEBUG] {message}");
    }
}