using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class TileData
{
    public Vector3 WorldPos;
    public bool IsVisited;
}

public class MapManager : Singleton<MapManager>, IInitializableManager
{
    private Dictionary<Vector3Int, TileData> _tileDictionary = new();

    public Action<Vector3> OnCellVisited;

    public void Initialize()
    {
        for (int x = -MapConfig.GRID_SIZE_X / 2; x < MapConfig.GRID_SIZE_X / 2; x++)
        {
            for (int z = -MapConfig.GRID_SIZE_Z / 2; z < MapConfig.GRID_SIZE_Z / 2; z++)
            {
                Vector3 worldPos = new Vector3(x * MapConfig.TILE_SIZE, 0, z * MapConfig.TILE_SIZE);
                Vector3Int key = new Vector3Int(x, 0, z);

                _tileDictionary[key] = new TileData
                {
                    WorldPos = worldPos,
                    IsVisited = false
                };
            }
        }
    }

    private void LateUpdate()
    {
        VisitCell(GameManager.Instance.GetPlayerTransform().position);
    }

    public void VisitCell(Vector3 worldPos)
    {
        int gx = Mathf.FloorToInt(worldPos.x / MapConfig.TILE_SIZE);
        int gz = Mathf.FloorToInt(worldPos.z / MapConfig.TILE_SIZE);
        Vector3Int key = new Vector3Int(gx, 0, gz);
        Debug.Log(key);
        if (!IsVisited(key))
        {
            _tileDictionary[key].IsVisited = true;

            OnCellVisited?.Invoke(worldPos);
        }
    }

    public List<TileData> GetAllTiles()
    {
        return new List<TileData>(_tileDictionary.Values);
    }

    private bool IsVisited(Vector3 worldPos)
    {
        int gx = Mathf.FloorToInt(worldPos.x / MapConfig.TILE_SIZE);
        int gz = Mathf.FloorToInt(worldPos.z / MapConfig.TILE_SIZE);
        Vector3Int key = new Vector3Int(gx, 0, gz);

        return _tileDictionary.ContainsKey(key) && _tileDictionary[key].IsVisited;
    }
    public Vector3 GetRandomPosition()
    {
        var keys = new List<Vector3Int>(_tileDictionary.Keys);
        int index = UnityEngine.Random.Range(0, keys.Count);

        return _tileDictionary[keys[index]].WorldPos;
    }

    public bool IsInGround(Vector2 place)
    {
        foreach (var tile in _tileDictionary.Values)
        {
            Vector3 pos = tile.WorldPos;

            if (place.x < pos.x + 0.5f && place.x > pos.x - 0.5f && place.y < pos.y + 0.5f && place.y > pos.y - 0.5f)
                return true;
        }
        return false;
    }

    public void AddBlankTile()
    {
    }

    public void RemoveBlankTile()
    {
    }

}