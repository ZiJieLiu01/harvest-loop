using UnityEngine;

public class Tile
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Vector2Int m_gridPosition;
    private TileType m_tileType;
    private IInteractable m_occupant;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Init
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void Init(Vector2Int gridPos, TileType type)
    {
        m_tileType = type;
        m_gridPosition = gridPos;

    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // To String
    // ----------------------------------------------------------------------------------------------------------------------------------

    public string GetDebugInfo()
    {
        string output = $"=== Tile ===\n" +
            $"Type: {m_tileType}\n" +
            $"Hovered Tile Pos: {m_gridPosition} \n";

        if(m_occupant != null)
        {
            output += m_occupant.GetDebugInfo();
        }

        return output;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters / Setters
    // ----------------------------------------------------------------------------------------------------------------------------------

    public TileType TileType { get => m_tileType; set => m_tileType = value; }
    public IInteractable Occupant { get => m_occupant; set => m_occupant = value; }
    public Vector2Int GridPos => m_gridPosition;
}