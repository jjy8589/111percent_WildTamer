using UnityEngine;

public class FogOfWarGrid : MonoBehaviour
{
    [Header("Fog Settings")]
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float worldWidth = 100f;   // 맵 실제 가로 크기
    public float worldHeight = 100f;
    public float revealRadius = 5f;    // 시야 반경(월드 단위)

    [Header("References")]
    public Renderer fogRenderer;       // 맵 전체를 덮는 Plane의 Renderer

    private Texture2D fogTexture;
    private Color[] fogPixels;
    private Transform player;

    // 탐색 퍼센트 (가산점: 저장용)
    public float ExploredPercent
    {
        get
        {
            int revealed = 0;
            foreach (var c in fogPixels) if (c.a < 0.5f) revealed++;
            return (float)revealed / fogPixels.Length * 100f;
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        InitFogTexture();
    }

    void InitFogTexture()
    {
        fogTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        fogPixels = new Color[textureWidth * textureHeight];

        // 전부 검정(미탐색)으로 초기화
        for (int i = 0; i < fogPixels.Length; i++)
            fogPixels[i] = new Color(0, 0, 0, 1f);

        fogTexture.SetPixels(fogPixels);
        fogTexture.Apply();
        fogRenderer.material.mainTexture = fogTexture;
    }

    void Update()
    {
        RevealAroundPlayer();
    }

    void RevealAroundPlayer()
    {
        // 월드 좌표 → 텍스처 픽셀 좌표 변환
        int px = WorldToPixelX(player.position.x);
        int py = WorldToPixelY(player.position.z);
        int pixelRadius = (int)(revealRadius / worldWidth * textureWidth);

        bool changed = false;
        for (int y = py - pixelRadius; y <= py + pixelRadius; y++)
        {
            for (int x = px - pixelRadius; x <= px + pixelRadius; x++)
            {
                if (x < 0 || x >= textureWidth || y < 0 || y >= textureHeight) continue;
                if ((x - px) * (x - px) + (y - py) * (y - py) > pixelRadius * pixelRadius) continue;

                int idx = y * textureWidth + x;
                if (fogPixels[idx].a > 0.01f)
                {
                    // 경계부 부드럽게: 거리에 따라 알파 감소
                    float dist = Mathf.Sqrt((x - px) * (x - px) + (y - py) * (y - py));
                    float alpha = Mathf.Lerp(0f, 0.4f, dist / pixelRadius); // 외곽은 옅은 안개
                    fogPixels[idx] = new Color(0, 0, 0, alpha);
                    changed = true;
                }
            }
        }

        if (changed)
        {
            fogTexture.SetPixels(fogPixels);
            fogTexture.Apply();
        }
    }

    int WorldToPixelX(float wx)
    {
        float u = (wx + worldWidth * 0.5f) / worldWidth;
        u = 1f - u;
        return Mathf.RoundToInt(u * textureWidth);
    }

    int WorldToPixelY(float wz)
    {
        float v = (wz + worldHeight * 0.5f) / worldHeight;

        // Z축 반전 필요할 경우
        v = 1f - v;

        return Mathf.RoundToInt(v * textureHeight);
    }


    // 저장용: 픽셀 배열 직렬화
    public float[] SerializeFog()
    {
        float[] data = new float[fogPixels.Length];
        for (int i = 0; i < fogPixels.Length; i++) data[i] = fogPixels[i].a;
        return data;
    }

    public void DeserializeFog(float[] data)
    {
        for (int i = 0; i < Mathf.Min(data.Length, fogPixels.Length); i++)
            fogPixels[i] = new Color(0, 0, 0, data[i]);
        fogTexture.SetPixels(fogPixels);
        fogTexture.Apply();
    }
}