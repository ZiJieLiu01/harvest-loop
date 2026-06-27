using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Variables
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static InputManager Instance { get; private set; }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Input Action
    // ----------------------------------------------------------------------------------------------------------------------------------

    private GameInputAction m_gameInputAction;

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        if(Instance != null && Instance == this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_gameInputAction = new GameInputAction();
    }

    private void OnEnable()
    {
        m_gameInputAction.Enable();
    }

    private void OnDisable()
    {
        m_gameInputAction.Disable();
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public InputAction Move => m_gameInputAction.Player.Move;
    public InputAction PrimaryAction => m_gameInputAction.Player.PrimaryAction;
    public InputAction SelectHotbar => m_gameInputAction.Player.SelectHotbar;
    public InputAction Interact => m_gameInputAction.Player.Interact;
    public InputAction OpenInventory => m_gameInputAction.Player.OpenInventory;
    public InputAction ToggleDebug => m_gameInputAction.Player.ToggleDebug;
    public GameInputAction.PlayerActions PlayerInputAction => m_gameInputAction.Player;
}
