using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileData
{
    public Vector3 WorldPos;
    public bool IsVisited;
}

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private int _gridSizeX = 100;
    [SerializeField] private int _gridSizeZ = 100;
    [SerializeField] private float _tileSize = 1f;

    private Dictionary<Vector3, TileData> _tileDictionary = new();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int z = 0; z < _gridSizeZ; z++)
            {
                Vector3 worldPos = new Vector3(x * _tileSize, 0, z * _tileSize);
                Vector3 key = new Vector3(x, 0, z);

                _tileDictionary[key] = new TileData
                {
                    WorldPos = worldPos,
                    IsVisited = false
                };
            }
        }
    }

    public void VisitCell(Vector3 worldPos)
    {
        int gx = Mathf.FloorToInt(worldPos.x / _tileSize);
        int gz = Mathf.FloorToInt(worldPos.z / _tileSize);
        Vector3Int key = new Vector3Int(gx, 0, gz);

        if (_tileDictionary.ContainsKey(key))
        {
            _tileDictionary[key].IsVisited = true;
        }
    }

    public bool IsVisited(Vector3 worldPos)
    {
        int gx = Mathf.FloorToInt(worldPos.x / _tileSize);
        int gz = Mathf.FloorToInt(worldPos.z / _tileSize);
        Vector3Int key = new Vector3Int(gx, 0, gz);

        return _tileDictionary.ContainsKey(key) && _tileDictionary[key].IsVisited;
    }


    public List<TileData> GetAllTiles()
    {
        return new List<TileData>(_tileDictionary.Values);
    }
}