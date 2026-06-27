using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static TileManager Instance;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [Header("Hover Settings")]
    [SerializeField] private TileHoverIndicator m_hoverIndicatorPrefab;

    [Header("Grid Settings")]
    [SerializeField] private Tile m_tilePrefab;
    [SerializeField] private int m_width = 10;
    [SerializeField] private int m_height = 10;
    [SerializeField] private int m_tileSize = 1;

    [Header("Tilemap")]
    [SerializeField] private Tilemap m_groundTilemap;       // Will be use for bound check
    [SerializeField] private Tilemap m_blockingTilemap;

    [Header("Tile Object Datas")]        // I KNOW (T_T)
    [SerializeField] private TileObjectData m_shopNpcData;
    [SerializeField] private TileObjectData m_sellBinData;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Dictionary<Vector2Int, Tile> m_tiles = new();   // Keep track of all tiles
    private List<Vector2Int> m_tilledTilesPos = new();       // Keep track of all tilled tiles
    private TileHoverIndicator m_hoverIndicator;            // Highlights which tile the mouse is hover over
    private Tile m_currHoverTile;

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
        m_hoverIndicator = Instantiate(m_hoverIndicatorPrefab, transform);

        TimeManager.Instance.OnDayChange += OnDayChange;

        SetupTileData();
    }

    private void Update()
    {
        HandleMouseHover();
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Create Grid
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void SetupTileData()
    {
        BoundsInt bounds = m_groundTilemap.cellBounds;

        m_width = bounds.size.x;
        m_height = bounds.size.y;

        GameDatabase gameDatabase = GameManager.Instance.GameDatabase;

        foreach(Vector3Int cellPos in bounds.allPositionsWithin)
        {
            if (!m_groundTilemap.HasTile(cellPos))
                continue;   // Skip empty cells

            TileBase tileBase = m_groundTilemap.GetTile(cellPos);
            if (tileBase == null)
                continue;

            Vector2Int gridPos = new(cellPos.x, cellPos.y);

            Tile tile = new Tile();
            tile.Init(gridPos, gameDatabase.GetTileType(tileBase));

            m_tiles[gridPos] = tile;

            // I KNOW THIS SHOULD NOT BE HERE
            if(gridPos == new Vector2Int(1, 1))
            {
                TileObjectManager.Instance.TrySpawnTileObject(gridPos, m_shopNpcData);
                continue;
            }
            else if(gridPos == new Vector2Int (3, 1))
            {
                TileObjectManager.Instance.TrySpawnTileObject(gridPos, m_sellBinData);
                continue;
            }

            TileObjectManager.Instance.TrySpawnRandomObjectWithChance(gridPos, .25f);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Mouse Hover Tile Logic
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void HandleMouseHover()
    {
        // Get the mouse world position
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        // Convert world to grid
        Vector3Int cellPos = m_groundTilemap.WorldToCell(mouseWorldPos);

        // Check bounds
        if (!m_groundTilemap.HasTile(cellPos))
        {
            m_hoverIndicator.Hide();
            m_currHoverTile = null;
            return;     // Out of bound
        }

        m_currHoverTile = m_tiles[new Vector2Int(cellPos.x, cellPos.y)];
        m_hoverIndicator.Show(m_groundTilemap.GetCellCenterWorld(cellPos));
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Player Action on Tile
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void ApplyAction(IItemReceiver itemReceiver, Vector2Int gridPos, ToolData toolData)
    {
        Tile tile = m_tiles[gridPos];
        if (tile == null)
            return;

        if (tile.Occupant != null)
        {
            tile.Occupant.OnPrimaryInteract(itemReceiver, toolData);
        }
        else
        {
            if(toolData.toolType == ToolType.Hoe)
            {
                TileData tileData = GameManager.Instance.GameDatabase.GetTileData("tilled_tile");       // the string should be a const variable
                m_groundTilemap.SetTile((Vector3Int)gridPos, tileData.tileBase);
                m_tiles[gridPos].TileType = TileType.TilledFloor;
                m_tilledTilesPos.Add(gridPos);
                return;
            }
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Tile On New Day
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void OnDayChange(int day)
    {
        // Change all tilled tiles that is not occupied back to grass tile
        for(int i = m_tilledTilesPos.Count - 1; i >= 0; i--)
        {
            Vector2Int tilePos = m_tilledTilesPos[i];

            if (m_tiles.TryGetValue(tilePos, out Tile tile))
            {
                if (tile.Occupant != null)
                    continue;   // Is occupied

                TileData tileData = GameManager.Instance.GameDatabase.GetTileData("grass_tile");       // the string should be a const variable
                m_groundTilemap.SetTile((Vector3Int)tilePos, tileData.tileBase);
                m_tiles[tilePos].TileType = TileType.Grass;

                m_tilledTilesPos.RemoveAt(i);
            }
        }

        foreach(var(tilePos, tile) in m_tiles)
        {
            if (tile.Occupant != null)
                continue;

            TileObjectManager.Instance.TrySpawnRandomObjectWithChance(tilePos, .25f);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Tile Util Functions
    // ----------------------------------------------------------------------------------------------------------------------------------

    public Tile GetTile(Vector2Int gridPos)
    {
        return m_tiles[gridPos];
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / m_tileSize + 0.5f);
        int y = Mathf.FloorToInt(worldPos.y / m_tileSize + 0.5f);

        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * m_tileSize,
            gridPos.y * m_tileSize,
            0f
            );
    }

    private bool IsInsideGrid(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < m_width &&
            gridPos.y >= 0 && gridPos.y < m_height;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters / Setters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public Tile CurrHoverTile => m_currHoverTile;
}
