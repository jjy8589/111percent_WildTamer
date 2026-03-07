using UnityEngine;

public class FogOfWarGrid : MonoBehaviour
{
    [Header("Fog Settings")]
    private int _textureWidth = 256;
    private int _textureHeight = 256;

    [Header("References")]
    [SerializeField] private Renderer _fogRenderer;        // 맵 전체를 덮는 Plane의 Renderer

    private Texture2D _fogTexture;
    private Color[] _fogPixels;

    void Start()
    {
        InitFogTexture();
        MapManager.Instance.OnCellVisited += RevealTile;
    }

    private void InitFogTexture()
    {
        _fogTexture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
        _fogPixels = new Color[_textureWidth * _textureHeight];

        // 전부 검정(미탐색)으로 초기화
        for (int i = 0; i < _fogPixels.Length; i++)
        {
            _fogPixels[i] = new Color(0, 0, 0, 1f);
        }
        foreach(var visitTile in DataManager.Instance.GetVisitedMaps())
        {
            RevealTile(new Vector3Int(visitTile.X, 0, visitTile.Z));
        }

        _fogTexture.SetPixels(_fogPixels);
        _fogTexture.Apply();
        _fogRenderer.material.mainTexture = _fogTexture;
    }

    void RevealTile(Vector3Int key)
    {
        Vector3 worldPos = new Vector3(key.x * MapConfig.TILE_SIZE, 0, key.z * MapConfig.TILE_SIZE);

        RevealAroundPlayer(worldPos);
    }

    void RevealAroundPlayer(Vector3 pos)
    {
        int px = WorldToPixelX(pos.x);
        int py = WorldToPixelY(pos.z);
        int pixelRadius = (int)(MapConfig.FOG_REVEAL_RANGE / MapConfig.GRID_SIZE_X * _textureWidth);

        bool changed = false;
        for (int y = py - pixelRadius; y <= py + pixelRadius; y++)
        {
            for (int x = px - pixelRadius; x <= px + pixelRadius; x++)
            {
                if (x < 0 || x >= _textureWidth || y < 0 || y >= _textureHeight) continue;
                if ((x - px) * (x - px) + (y - py) * (y - py) > pixelRadius * pixelRadius) continue;

                int idx = y * _textureWidth + x;
                if (_fogPixels[idx].a > 0.01f)
                {
                    float dist = Mathf.Sqrt((x - px) * (x - px) + (y - py) * (y - py));
                    float alpha = Mathf.Lerp(0f, 0.4f, dist / pixelRadius);
                    _fogPixels[idx] = new Color(0, 0, 0, alpha);
                    changed = true;
                }
            }
        }

        if (changed)
        {
            _fogTexture.SetPixels(_fogPixels);
            _fogTexture.Apply();
        }
    }


    int WorldToPixelX(float wx)
    {
        float u = (wx + MapConfig.GRID_SIZE_X * 0.5f) / MapConfig.GRID_SIZE_X;
        u = 1f - u;
        return Mathf.RoundToInt(u * _textureWidth);
    }

    int WorldToPixelY(float wz)
    {
        float v = (wz + MapConfig.GRID_SIZE_Z * 0.5f) / MapConfig.GRID_SIZE_Z;
        v = 1f - v;

        return Mathf.RoundToInt(v * _textureHeight);
    }
}