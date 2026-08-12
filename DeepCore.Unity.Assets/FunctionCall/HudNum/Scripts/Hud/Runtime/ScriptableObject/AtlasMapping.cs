using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
#endif

[System.Serializable]
public class SpriteInfo
{
    public int index;
    public Vector2Int size;
}
public class AtlasMapping : SerializedScriptableObject
{
    [SerializeField]
    private SpriteAtlas m_SpriteAtlas;

    [ReadOnly]
    [SerializeField]
    private Texture2D m_AtlasMappingTex;
    public Texture2D atlasMappingTex { get { return m_AtlasMappingTex; } }

    [ReadOnly]
    [SerializeField]
    private int m_Width;
    public int width { get { return m_Width; } }

    [ReadOnly]
    [SerializeField]
    private int m_Height;
    public int height{ get { return m_Height; } }

    [ReadOnly]
    [SerializeField]
    private int m_AtlasMappingWidth;
    public int atlasMappingWidth { get { return m_AtlasMappingWidth; } }

    [ReadOnly]
    [SerializeField]
    private int m_AtlasMappingHeight;
    public int atlasMappingHeight { get { return m_AtlasMappingHeight; } }


    [ReadOnly]
    [SerializeField]
    private Dictionary<string, SpriteInfo> nameToSpriteInfo = new Dictionary<string, SpriteInfo>();

    private Texture m_AtlasTex;

    public Texture atlasTex
    {
        get 
        {
            if (m_AtlasTex != null) return m_AtlasTex;
            foreach (var item in nameToSpriteInfo)
            {
                Sprite sprite = m_SpriteAtlas.GetSprite(item.Key);
                if (sprite != null)
                {
                    m_AtlasTex = sprite.texture;
                    return m_AtlasTex;
                }
            }
            return null;
        }
    }

    public SpriteInfo GetSpriteInfo(string name)
    {
        SpriteInfo spriteInfo = null;
        nameToSpriteInfo.TryGetValue(name, out spriteInfo);
        return spriteInfo;
    }

    [NonSerialized]
    private bool isGenAtlasMapping = false;
    [Button("GenAtlasMapping")]
    public void GenAtlasMappingInfo()
    {
#if UNITY_EDITOR
        if (isGenAtlasMapping) return;
        isGenAtlasMapping = true;
        m_AtlasTex = null;
        CollectSprite();
        GenMappingTexture();
        GenAtlasMappingTextrue();
#endif
    }

#if UNITY_EDITOR
    private void GenAtlasMappingTextrue()
    {
        if (!Application.isPlaying) return;
        if (m_SpriteAtlas.spriteCount == 0) return;
        Sprite[] sprits = new Sprite[m_SpriteAtlas.spriteCount];
        m_SpriteAtlas.GetSprites(sprits);
        Texture atlasTex = sprits[0].texture;
        int atlasWidth = atlasTex.width;
        int atlasHeight = atlasTex.height;
        int mappingWidth = m_AtlasMappingTex.width;
        m_Width = atlasWidth;
        m_Height = atlasHeight;
        for (int i = 0; i < sprits.Length; i++)
        {
            Sprite sprite = sprits[i];
            string name = sprite.name.Replace("(Clone)", "");
            SpriteInfo spriteInfo;
            if (nameToSpriteInfo.TryGetValue(name, out spriteInfo))
            {
                var uv = UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
                SetSpriteUV(spriteInfo, new Vector2(uv.x, uv.y), new Vector2(uv.z, uv.w));
            }
            if (sprite.border.SqrMagnitude() > 0)
            {
                Vector4 outer, inner, padding, border;
                outer = UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
                inner = UnityEngine.Sprites.DataUtility.GetInnerUV(sprite);
                padding = UnityEngine.Sprites.DataUtility.GetPadding(sprite);
                border = sprite.border;
                Vector2[] s_UVScratch = new Vector2[4];
                s_UVScratch[0] = new Vector2(outer.x, outer.y);
                s_UVScratch[1] = new Vector2(inner.x, inner.y);
                s_UVScratch[2] = new Vector2(inner.z, inner.w);
                s_UVScratch[3] = new Vector2(outer.z, outer.w);
                int spritesliceId = 0;
                for (int x = 0; x < 3; ++x)
                {
                    int x2 = x + 1;
                    for (int y = 0; y < 3; ++y)
                    {
                        int y2 = y + 1;
                        SpriteInfo info;
                        if (nameToSpriteInfo.TryGetValue(name + "_" + spritesliceId, out info))
                        {
                            SetSpriteUV(info, new Vector2(s_UVScratch[x].x, s_UVScratch[y].y), new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
                            spritesliceId++;
                        }
                    }
                }
            }
        }
        m_AtlasMappingTex.Apply();
    }

    private void SetSpriteUV(SpriteInfo spriteInfo, Vector2 min, Vector2 max)
    {
        int atlasWidth = atlasTex.width;
        int atlasHeight = atlasTex.height;
        int mappingWidth = m_AtlasMappingTex.width;
        ushort spriteWidth = (ushort)((max.x - min.x) * atlasWidth);
        ushort spriteHeight = (ushort)((max.y - min.y) * atlasHeight);
        ushort spriteX = (ushort)(min.x * atlasWidth);
        ushort spriteY = (ushort)(min.y * atlasHeight);
        spriteInfo.size = new Vector2Int(spriteWidth, spriteHeight);
        byte posx0bytes = (byte)(spriteX % 256);
        byte posx1bytes = (byte)(spriteX / 256);
        byte posy0bytes = (byte)(spriteY % 256);
        byte posy1bytes = (byte)(spriteY / 256);

        byte spritew0bytes = (byte)(spriteWidth % 256);
        byte spritew1bytes = (byte)(spriteWidth / 256);
        byte spriteh0bytes = (byte)(spriteHeight % 256);
        byte spriteh1bytes = (byte)(spriteHeight / 256);

        int firstindex = spriteInfo.index * 2;
        int secondindex = firstindex + 1;

        int firstX = firstindex % mappingWidth;
        int firstY = firstindex / mappingWidth;

        int secondX = secondindex % mappingWidth;
        int secondY = secondindex / mappingWidth;

        m_AtlasMappingTex.SetPixel(firstX, firstY, new Color32(posx1bytes, posx0bytes, posy1bytes, posy0bytes));
        m_AtlasMappingTex.SetPixel(secondX, secondY, new Color32(spritew1bytes, spritew0bytes, spriteh1bytes, spriteh0bytes));
    }

    private void GenMappingTexture()
    {
        int size = GetTexSize();
        m_AtlasMappingWidth = size;
        m_AtlasMappingHeight = size;
        if (m_AtlasMappingTex != null && m_AtlasMappingTex.width == size)
        {
            m_AtlasMappingTex.SetPixels(new Color[size * size]);
            m_AtlasMappingTex.Apply();
            return;
        }
        if (m_AtlasMappingTex != null)
        {
            m_AtlasMappingTex.Reinitialize(size, size);
            m_AtlasMappingTex.SetPixels(new Color[size * size]);
            m_AtlasMappingTex.Apply();
            return;
        }
        m_AtlasMappingTex = new Texture2D(size, size, TextureFormat.RGBA32, false, PlayerSettings.colorSpace == ColorSpace.Linear);
        m_AtlasMappingTex.wrapMode = TextureWrapMode.Clamp;
        m_AtlasMappingTex.filterMode = FilterMode.Point;
        m_AtlasMappingTex.SetPixels(new Color[size* size]);
        m_AtlasMappingTex.Apply();
        string path = AssetDatabase.GetAssetPath(this);
        AssetDatabase.AddObjectToAsset(m_AtlasMappingTex, path);
        EditorUtility.SetDirty(m_SpriteAtlas);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CollectSprite()
    {
        nameToSpriteInfo.Clear();
        SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { m_SpriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
        if (m_SpriteAtlas.spriteCount == 0) return;
        Sprite[] sprits = new Sprite[m_SpriteAtlas.spriteCount];
        m_SpriteAtlas.GetSprites(sprits);
        Array.Sort(sprits, (a, b) => { return a.name.CompareTo(b.name); });
        int spriteId = 0;
        for (int i = 0; i < sprits.Length; i++)
        {
            Sprite sprite = sprits[i];

            if(sprite.border.SqrMagnitude() > 0)
            {
                SpriteInfo spriteInfo = new SpriteInfo();
                spriteInfo.index = spriteId;
                string name = sprits[i].name.Replace("(Clone)", "");
                nameToSpriteInfo[name] = spriteInfo;
                spriteId++;
                for (int s = 0; s < 9; s++)
                {
                    SpriteInfo info = new SpriteInfo();
                    info.index = spriteId;
                    nameToSpriteInfo[name + "_" + s] = info;
                    spriteId++;
                }
            }
            else
            {
                SpriteInfo spriteInfo = new SpriteInfo();
                spriteInfo.index = spriteId;
                string name = sprits[i].name.Replace("(Clone)", "");
                nameToSpriteInfo[name] = spriteInfo;
                spriteId++;
            }
        }
    }

    private int GetTexSize()
    {
        int size = 1;
        int spriteCount = nameToSpriteInfo.Count;
        for (int i = 0; i <= 10; i++)
        {
            size = (int)Mathf.Pow(2, i);
            if (size * size >= spriteCount * 2)
            {
                return size;
            }
        }
        return size;
    }
#endif
}
