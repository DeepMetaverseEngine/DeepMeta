using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
#if UNITY_EDITOR
using UnityEditor;
using System.Drawing;
#endif

namespace NFCore.Extension
{

    [Serializable]
    public class SpriteExpansion
    {
        [ReadOnly]
        [HideInInspector]
        public HudImage hudimage;

        [OnValueChanged("OnImageChange")]
        public Sprite sprite;

        [OnValueChanged("OnImageChange")]
        public Vector2 position;

        [OnValueChanged("OnImageChange")]
        public Vector2 size = new Vector2(100, 100);

#if UNITY_EDITOR

        [Button("Set Native Size")]
        public void SetNativeSize()
        {
            if (sprite == null) return;
            size = new Vector2(sprite.rect.width, sprite.rect.height);
            hudimage?.OnImageChange();
        }

        public void OnImageChange()
        {
            hudimage?.OnImageChange();
        }
#endif
    }

    public class HudImage : HudComponet
    {
        public enum FillMethod
        {
            Horizontal,
            Vertical,
        }

        public enum ImageType
        {
            Simple,
            Sliced,
            Filled,
        }

        public enum OriginHorizontal
        {
            Left,
            Right,
        }

        public enum OriginVertical
        {
            Bottom,
            Top,
        }

        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnImageChange")]
        private Sprite m_Sprite;

        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnImageChange")]
        private Vector2 m_Size = new Vector2(100, 100);


        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnImageChange")]
        private ImageType m_ImageType = ImageType.Simple;


        [ShowIf("m_ImageType", ImageType.Filled)]
        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnImageChange")]
        private FillMethod m_FillMethod = FillMethod.Horizontal;


        [ShowIf("m_ImageType", ImageType.Filled)]
        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [CustomValueDrawer("FillOriginDraw")]
        [OnValueChanged("OnImageChange")]
        private int m_FillOrigin = 0;


        [ShowIf("m_ImageType", ImageType.Filled)]
        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [Range(0, 1)]
        [OnValueChanged("OnImageChange")]
        private float m_FillAmount = 1.0f;

        [ShowIf("m_ImageType", ImageType.Simple)]
        [Button("Set Native Size")]
        [VerticalGroup(VERTICAL_GROUP)]
        public void SetNativeSize()
        {
            if (m_Sprite == null) return;
            m_Size = new Vector2(m_Sprite.rect.width, m_Sprite.rect.height);
            SetSprite(m_Sprite);
        }

        [ShowIf("m_ImageType", ImageType.Simple)]
        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnImageChange")]
        private List<SpriteExpansion> m_SpriteExpansion = new List<SpriteExpansion>();

        public HudImage()
        {
            m_HudType = HudType.Image;
        }

        public override void OnAwake()
        {
            SetSprite(m_Sprite);
        }

        private void SetSprite(Sprite sprite)
        {
            if (sprite == null) return;
            if (atlasMapping == null) return;

            switch (m_ImageType)
            {
                case ImageType.Simple:
                    Simple(sprite);
                    break;
                case ImageType.Filled:
                    Filled(sprite);
                    break;
                case ImageType.Sliced:
                    Sliced(sprite);
                    break;
            }
        }

        private void Simple(Sprite sprite)
        {
            var spriteInfo = atlasMapping.GetSpriteInfo(sprite.name);
            if (spriteInfo == null) return;
            ResizeDataSnippet(1);
            HudDataSnippet snippet = GetDataSnippet(0);
            snippet.ResetNineParam();

            int quadindex = 0;
            snippet.SetSpriteId(quadindex, spriteInfo.index);
            snippet.SetSpriteQuad(quadindex, new float2(-m_Size.x / 2, -m_Size.y / 2), m_Size);
            snippet.SetAmount(1, 0, 0);
            if (m_SpriteExpansion != null)
            {
                int spriteCount = Mathf.Min(m_SpriteExpansion.Count, 8);
                for (int i = 0; i < spriteCount; i++)
                {
                    SpriteExpansion spriteexpansion = m_SpriteExpansion[i];
                    spriteexpansion.hudimage = this;
                    if (spriteexpansion != null && spriteexpansion.sprite != null)
                    {
                        spriteInfo = atlasMapping.GetSpriteInfo(spriteexpansion.sprite.name);
                        if (spriteInfo != null)
                        {
                            quadindex++;
                            snippet.SetSpriteId(quadindex, spriteInfo.index);
                            float2 size = spriteexpansion.size;
                            float2 pos = spriteexpansion.position;
                            snippet.SetSpriteQuad(quadindex, new float2(-size.x / 2, -size.y / 2) + pos, size);
                        }
                    }
                }
            }
            snippet.WriteParamData();
        }

        private void Filled(Sprite sprite)
        {
            var spriteInfo = atlasMapping.GetSpriteInfo(sprite.name);
            if (spriteInfo == null) return;
            ResizeDataSnippet(1);
            HudDataSnippet snippet = GetDataSnippet(0);
            snippet.ResetNineParam();
            int quadindex = 0;
            snippet.SetSpriteId(quadindex, spriteInfo.index);
            float2 spritePos = new float2(-m_Size.x / 2, -m_Size.y / 2);
            float2 spriteSize = m_Size;
            int method = (int)(m_FillMethod);
            spritePos[method] = spritePos[method] + spriteSize[method] * (1 - m_FillAmount) * m_FillOrigin;
            spriteSize[method] = spriteSize[method] * m_FillAmount;
            snippet.SetSpriteQuad(quadindex, spritePos, spriteSize);
            snippet.SetAmount(m_FillAmount, m_FillOrigin, (int)m_FillMethod);
            snippet.WriteParamData();
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            Rect originalRect = adjustedRect;

            for (int axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;
                if (originalRect.size[axis] != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
                float combinedBorders = border[axis] + border[axis + 2];
                if (adjustedRect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }
            return border;
        }

        static readonly Vector2[] s_VertScratch = new Vector2[4];
        private void Sliced(Sprite sprite)
        {
            if (sprite == null) return;

            if (sprite.border.SqrMagnitude() > 0)
            {
                ResizeDataSnippet(1);
                HudDataSnippet snippet = GetDataSnippet(0);
                snippet.ResetNineParam();
                Vector4 border, padding;
                padding = UnityEngine.Sprites.DataUtility.GetPadding(sprite);
                border = sprite.border;
                Rect rect = new Rect(-m_Size.x / 2, -m_Size.y / 2, m_Size.x, m_Size.y);
                Vector4 adjustedBorders = GetAdjustedBorders(border, rect);
                s_VertScratch[0] = new Vector2(padding.x, padding.y);
                s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);
                s_VertScratch[1].x = adjustedBorders.x;
                s_VertScratch[1].y = adjustedBorders.y;
                s_VertScratch[2].x = rect.width - adjustedBorders.z;
                s_VertScratch[2].y = rect.height - adjustedBorders.w;
                for (int i = 0; i < 4; ++i)
                {
                    s_VertScratch[i].x += rect.x;
                    s_VertScratch[i].y += rect.y;
                }
                int slicedIndex = 0;
                for (int x = 0; x < 3; ++x)
                {
                    int x2 = x + 1;
                    for (int y = 0; y < 3; ++y)
                    {
                        int y2 = y + 1;
                        string spriteName = sprite.name + "_" + slicedIndex;
                        var spriteInfo = atlasMapping.GetSpriteInfo(spriteName);
                        if (spriteInfo != null)
                        {
                            snippet.SetSpriteId(slicedIndex, spriteInfo.index);
                            float2 pos = new float2(s_VertScratch[x].x, s_VertScratch[y].y);
                            float2 maxpos = new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y);
                            snippet.SetSpriteQuad(slicedIndex, pos, maxpos - pos);
                            snippet.SetAmount(1, 0, 0);
                        }
                        slicedIndex++;
                    }
                }
                snippet.WriteParamData();
            }
            else
            {
                Simple(sprite);
            }
        }

#if UNITY_EDITOR
        private IEnumerable GetFillOrigin()
        {
            if (m_FillMethod == FillMethod.Horizontal)
            {
                return new ValueDropdownList<OriginHorizontal>()
            {
              { "Left", OriginHorizontal.Left  },
              { "Right", OriginHorizontal.Right  },
            };
            }
            return new ValueDropdownList<OriginVertical>()
            {
              { "Bottom", OriginVertical.Bottom},
              { "Top", OriginVertical.Top  },
            };
        }

        private int FillOriginDraw(int value, GUIContent label)
        {
            if (m_FillMethod == FillMethod.Horizontal)
            {
                OriginHorizontal originHorizontal = (OriginHorizontal)EditorGUILayout.EnumPopup(label, (OriginHorizontal)value);
                return (int)originHorizontal;
            }
            OriginVertical originVertical = (OriginVertical)EditorGUILayout.EnumPopup(label, (OriginVertical)value);
            return (int)originVertical;
        }

        public void OnImageChange()
        {
            SetSprite(m_Sprite);
        }

#endif
    }
}