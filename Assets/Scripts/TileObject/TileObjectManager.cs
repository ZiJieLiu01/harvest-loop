using System.Collections.Generic;
using UnityEngine;

public class TileObjectManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    // Singleton
    // ----------------------------------------------------------------------------------------------------------------------------------

    public static TileObjectManager Instance;

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Runtime States
    // ----------------------------------------------------------------------------------------------------------------------------------

    private Dictionary<Vector2Int, TileObjectInstance> m_spawnedTileObject = new();
    private Transform m_transform;

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
        m_transform = transform;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Spawn Tile Object
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void TrySpawnTileObject(Vector2Int gridPos, TileObjectData tileObjectData)
    {
        if (m_spawnedTileObject.ContainsKey(gridPos))
            return;

        SpawnTileObject(gridPos, tileObjectData);
    }

    public bool TrySpawnTileObject(Vector2Int gridPos, PlaceableData placeableData)
    {
        if (m_spawnedTileObject.ContainsKey(gridPos))
            return false;

        TileType tileType = TileManager.Instance.GetTile(gridPos).TileType;
        if(placeableData.tileObjectData.spawnableTile != TileType.Default &&
            placeableData.tileObjectData.spawnableTile != tileType)
        {
            return false;
        }

        SpawnTileObject(gridPos, placeableData);
        return true;
    }

    private void SpawnTileObject(Vector2Int gridPos, TileObjectData tileObjectData)
    {
        Vector3 worldPos = TileManager.Instance.GridToWorld(gridPos);
        worldPos += new Vector3(.5f, .5f);  // Centering

        TileObjectInstance tileObject = Instantiate(tileObjectData.tileObjectPrefab, worldPos, Quaternion.identity, m_transform);
        if (tileObject != null)
        {
            tileObject.Init(gridPos, tileObjectData);
            TileManager.Instance.GetTile(gridPos).Occupant = tileObject;
            m_spawnedTileObject.Add(gridPos, tileObject);
        }
    }

    private void SpawnTileObject(Vector2Int gridPos, PlaceableData placeableData)
    {
        Vector3 worldPos = TileManager.Instance.GridToWorld(gridPos);
        worldPos += new Vector3(.5f, .5f);  // Centering

        TileObjectInstance tileObject = Instantiate(placeableData.tileObjectData.tileObjectPrefab, worldPos, Quaternion.identity, m_transform);
        if (tileObject != null)
        {
            tileObject.Init(gridPos, placeableData);
            TileManager.Instance.GetTile(gridPos).Occupant = tileObject;
            m_spawnedTileObject.Add(gridPos, tileObject);
        }
    }

    public void RemoveTileObject(Vector2Int gridPos) 
    {
        if (!m_spawnedTileObject.ContainsKey(gridPos))
            return;

        // Remove from the dictionary
        m_spawnedTileObject.Remove(gridPos);

        // Remove from tile's occupant
        TileManager.Instance.GetTile(gridPos).Occupant = null;
    }

    // ----------------------------------------------------------------------------------------------------------------------------------
    // Random Spawn
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void TrySpawnRandomObjectWithChance(Vector2Int gridPos, float chance)
    {
        if (m_spawnedTileObject.ContainsKey(gridPos))
            return; // Tile is already occupant

        if (Random.value > chance)
            return;

        SpawnRandomObject(gridPos);
    }

    private void SpawnRandomObject(Vector2Int gridPos)
    {
        GameDatabase gameDatabase = GameManager.Instance.GameDatabase;
        var tileObjectDatas = gameDatabase.TileObjects;
        if (tileObjectDatas.Count <= 0)
        {
            Debug.LogWarning("GameDatabase.TileObjects Count <= 0");
            return;
        }

        int index = Random.Range(0, tileObjectDatas.Count);
        TileObjectData data = gameDatabase.TileObjects[index];

        if (data.spawnableTile == TileType.Default ||
            data.spawnableTile == TileManager.Instance.GetTile(gridPos).TileType)
        {
            // Only spawn on desired tile
            SpawnTileObject(gridPos, data);
        }
    }
}
