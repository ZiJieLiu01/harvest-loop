using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private PlayerMovement m_playerMovement;
    [SerializeField] private Health m_health;
    [SerializeField] private Hotbar m_hotbar;
    [SerializeField] private Transform m_itemHoldPoint;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Direction m_facingDir = Direction.Up;
    private HeldItem m_currentHeldItem;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // MonoBehaviour Events
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void OnEnable()
    {
        // Setup Input
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.Move.performed += OnMove;
            inputManager.Move.canceled += OnMove;
            inputManager.PrimaryAction.performed += OnPrimaryAction;
            inputManager.SelectHotbar.performed += OnSelectHotbar;
            inputManager.Interact.performed += OnInteract;
        }
        else
            Debug.LogError("InputManager Not Found!");

        m_hotbar.Inventory.OnInventoryChange += OnInventoryChange;

        // Health
        if (m_health != null)
            m_health.OnDeath += HandleDeath;
        else
            Debug.LogError($"{gameObject.name}'s Health Component is not setup correctly!");
    }

    private void OnDisable()
    {
        // Disable Input
        InputManager inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.Move.performed -= OnMove;
            inputManager.PrimaryAction.performed -= OnPrimaryAction;
            inputManager.SelectHotbar.performed -= OnSelectHotbar;
            inputManager.Interact.performed -= OnInteract;
        }

        // Disable Health
        if (m_health != null)
            m_health.OnDeath -= HandleDeath;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Controller
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();

        if (direction == Vector2.zero)
        {
            m_playerMovement.SetMoveInput(Vector2.zero);
            return;
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction = new Vector2(Mathf.Sign(direction.x), 0);
            m_facingDir = direction.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            direction = new Vector2(0, Mathf.Sign(direction.y));
            m_facingDir = direction.y > 0 ? Direction.Up : Direction.Down;
        }

        m_playerMovement.SetMoveInput(direction);
        UpdateItemHoldPointFlip();
    }

    private void OnPrimaryAction(InputAction.CallbackContext ctx)
    {
        if (m_currentHeldItem != null)
        {
            m_currentHeldItem.PrimaryAction();
        }
    }

    private void OnSelectHotbar(InputAction.CallbackContext ctx) 
    {
        int value = Mathf.RoundToInt(ctx.ReadValue<float>());

        int slotIndex = value - 1;

        m_hotbar.Select(slotIndex);
        EquipItem(m_hotbar.GetSelectedItem());
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (m_currentHeldItem != null)
        {
            m_currentHeldItem.SecondaryAction();
        }
        else
        {
            Tile currHoverTile = TileManager.Instance.CurrHoverTile;
            if (currHoverTile != null)
            {
                currHoverTile.Occupant?.OnSecondaryInteract(m_hotbar.Inventory);
            }
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Helper Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void EquipItem(ItemInstance item)
    {
        if(m_currentHeldItem != null)
            Destroy(m_currentHeldItem.gameObject);

        if (item == null)
            return;

        GameObject prefab = item.Data.selectedItemPreviewPrefab;
        if (prefab == null)
            return;

        GameObject heldGO = Instantiate(prefab, m_itemHoldPoint);
        m_currentHeldItem = heldGO.GetComponent<HeldItem>();
        m_currentHeldItem?.Initialize(this, item.Data, item);
    }

    private void HandleDeath()
    {
        Destroy(gameObject);

        // Later can have player death animation
        // Disable input

    }
     
    private void OnInventoryChange()
    {
        // Check if we are holding a valid item
        ItemInstance itemSelected = m_hotbar.GetSelectedItem();
        if(itemSelected == null && m_currentHeldItem != null)
        {
            Destroy(m_currentHeldItem.gameObject);
            m_currentHeldItem = null;
            return;
        }

        EquipItem(itemSelected);
    }

    private void UpdateItemHoldPointFlip()
    {
        // Flip X only when facing left, normal otherwise
        m_itemHoldPoint.localScale = new Vector3(
            m_facingDir == Direction.Left ? -1 : 1,
            1,
            1
        );                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters / Setters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public Vector2 GetFacingVector()
    {
        switch (m_facingDir)
        {
            case Direction.Up: return Vector2.up;
            case Direction.Down: return Vector2.down;
            case Direction.Left: return Vector2.left;
            case Direction.Right: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    public Inventory Inventory => m_hotbar.Inventory;
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}