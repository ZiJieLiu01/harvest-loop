using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Tile Data")]
public class TileData : BaseData
{
    [Header("Tile Data")]
    public TileBase tileBase;
}

public enum TileType
{
    Grass,
    Water,
    Wall,
    Floor,
    TilledFloor,
    Default,
}

[System.Serializable]
public struct TileMapping
{
    public TileBase tile;
    public TileType type;
}