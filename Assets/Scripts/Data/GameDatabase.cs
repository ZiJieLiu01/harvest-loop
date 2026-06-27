using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Database/GameDatabase")]
public class GameDatabase : ScriptableObject
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Serialize Fields
    // ----------------------------------------------------------------------------------------------------------------------------------

    [Header("Items")]
    [SerializeField] private List<InventoryItemData> items;

    [Header("Placeables")]
    [SerializeField] private List<PlaceableData> placeables;

    [Header("World Objects")]
    [SerializeField] private List<TileObjectData> tileObjects;

    [Header("Tiles")]
    [SerializeField] private List<TileData> tiles;
    [SerializeField] private List<TileMapping> tileMapping;

    [Header("Crops")]
    [SerializeField] private List<CropData> crops;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Lookup Dictionaries
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Dictionary<string, InventoryItemData> m_itemLookup;
    private Dictionary<string, PlaceableData> m_placeableLookup;
    private Dictionary<string, TileObjectData>  m_tileObjectLookup;
    private Dictionary<string, TileData> m_tileLookup;
    private Dictionary<string, CropData> m_cropLookup;
    private Dictionary<TileBase, TileType> m_tileTypeLookup;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void Init()
    {
        m_itemLookup = BuildLookup(items);
        m_placeableLookup = BuildLookup(placeables);
        m_tileObjectLookup = BuildLookup(tileObjects);
        m_tileLookup = BuildLookup(tiles);
        m_cropLookup = BuildLookup(crops);
        m_tileTypeLookup = BuildLookup(tileMapping);
    }

    private Dictionary<string, T> BuildLookup<T>(List<T> list) where T : BaseData
    {
        var dict = new Dictionary<string, T>();
        foreach (var data in list)
        {
            if (data == null || string.IsNullOrEmpty(data.id))
                continue;

            if(dict.ContainsKey(data.id))
            {
                Debug.LogWarning($"Duplicate ID: {data.id}");
                continue;
            }

            dict.Add(data.id, data);
        }

        return dict;
    }

    private Dictionary<TileBase, TileType> BuildLookup(List<TileMapping> list)
    {
        var dict = new Dictionary<TileBase, TileType>();

        foreach (var mapping in list)
        {
            if (mapping.tile == null)
                continue;

            dict[mapping.tile] = mapping.type;
        }

        return dict;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Random Select (I will put this here for now, will try to move to like util class later)
    // ----------------------------------------------------------------------------------------------------------------------------------

    public T GetRandom<T>(IReadOnlyList<T> list)
    {
        if (list == null || list.Count == 0)
            return default;

        int index = Random.Range(0, list.Count - 1);
        return list[index];
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Getters
    // ----------------------------------------------------------------------------------------------------------------------------------

    // Lookup
    public InventoryItemData GetItem(string id) => m_itemLookup.TryGetValue(id, out var data) ? data : null;
    public PlaceableData GetPlaceable(string id) => m_placeableLookup.TryGetValue(id, out var data) ? data : null;
    public TileObjectData GetTileObject(string id) => m_tileObjectLookup.TryGetValue(id, out var data) ? data : null;
    public TileData GetTileData(string id) => m_tileLookup.TryGetValue(id, out var data) ? data : null;
    public CropData GetCrop(string id) => m_cropLookup.TryGetValue(id, out var data) ? data : null;
    public TileType GetTileType(TileBase tilebase)
    {
        if (m_tileTypeLookup.TryGetValue(tilebase, out var tileType))
            return tileType;

        return TileType.Default;
    }

    // Readonly Datas
    public IReadOnlyList<InventoryItemData> Items => items;
    public IReadOnlyList<PlaceableData> Placeables => placeables;
    public IReadOnlyList<TileObjectData> TileObjects => tileObjects;
    public IReadOnlyList<TileData> Tiles => tiles;
    public IReadOnlyList<CropData> Crops => crops;
}
