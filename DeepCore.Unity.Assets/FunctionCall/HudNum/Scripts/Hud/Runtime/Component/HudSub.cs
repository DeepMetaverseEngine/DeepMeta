using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NFCore.Extension
{
    public class HudSub : HudComponetBase
    {
        [HideLabel, PreviewField(55)]
        [BoxGroup("$hudType")]
        [VerticalGroup(VERTICAL_GROUP)]
        [HorizontalGroup(VERTICAL_HORIZONTAL_GROUP, 55)]
        [ReadOnly]
        [SerializeField]
        private Texture m_icon;

        [HorizontalGroup(VERTICAL_HORIZONTAL_GROUP)]
        [VerticalGroup(VERTICAL_HORIZONTAL_VERTICAL_GROUP, Order = 2)]
        [SerializeField]
        [CustomValueDrawer("DrawerHudSubRenderer")]
        private HudSubCanvasRenderer m_SubRender;

        public HudSub()
        {
            m_HudType = HudType.Sub;
        }

        public override void OnReorder()
        {
            if (m_SubRender == null) return;
            m_SubRender.OnReorder();
        }

#if UNITY_EDITOR
        private Object DrawerHudSubRenderer(Object value, GUIContent label)
        {
            Object lastObject = value;
            Object newObject = UnityEditor.EditorGUILayout.ObjectField(label, value, typeof(HudSubCanvasRenderer), true);
            if (lastObject != newObject)
            {
                if (newObject!=null)
                {
                    HudSubCanvasRenderer newHudSubRenderer = newObject as HudSubCanvasRenderer;
                    newHudSubRenderer.canvasRenderer = parent as HudCanvasRenderer;
                    newHudSubRenderer.ExecuteAwake();
                }
                if (lastObject != null)
                {
                    HudSubCanvasRenderer lastHudSubRenderer = lastObject as HudSubCanvasRenderer;
                    lastHudSubRenderer.ExecuteDestroy();
                    lastHudSubRenderer.canvasRenderer = null;
                }
            }
            return newObject;
        }
#endif
    }
}