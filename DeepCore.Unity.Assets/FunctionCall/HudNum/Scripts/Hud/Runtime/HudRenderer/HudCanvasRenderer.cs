using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NFCore.Extension
{

    public class HudCanvasRenderer : HudSubCanvasRenderer
    {
        [FoldoutGroup("Bounds", Order = 0)]
        public bool drawGizmos;
        [FoldoutGroup("Bounds", Order = 0)]
        public Vector2 center;
        [FoldoutGroup("Bounds", Order = 0)]
        public Vector2 size = new Vector2(100, 100);

        [VerticalGroup("HudSubRenderer", Order = 0)]
        public Mesh mesh;
        [VerticalGroup("HudSubRenderer", Order = 0)]
        public Material material;
        [VerticalGroup("HudSubRenderer", Order = 0)]
        public AtlasMapping atlasMapping;
        [VerticalGroup("HudSubRenderer", Order = 0)]
        public TMP_FontAsset fontAsset;

        private HudRendererBatch m_rendererBatch;
        public override HudRendererBatch rendererBatch
        {
            get
            {
                return m_rendererBatch;
            }
        }

        protected override void Awake()
        {
            canvasRenderer = this;
            m_rendererBatch = HudRendererCenter.GetRendererBatch(material, mesh, atlasMapping, fontAsset);
            m_rendererBatch?.AddExpansionNotif(ExpansionNotif);
            base.Awake();
            rendererBatch.SetBounds(transId, center, size);
        }

        protected override void OnDestroy()
        {
            m_rendererBatch?.RemoveExpansionNotif(ExpansionNotif);
            base.OnDestroy();
        }

        private void ExpansionNotif()
        {
            TriggerReorder();
        }

        public override void OnReorder()
        {
            RemoveData();
            base.OnReorder();
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            Vector3 lossyScale = transform.lossyScale;
            Vector3 cubePos = transform.position + new Vector3(center.x, center.y, 0) / 100f;
            Vector3 cubeSize = new Vector3(lossyScale.x * size.x, lossyScale.y * size.y, 0) / 100f;
            Gizmos.DrawWireCube(cubePos, cubeSize);
        }

        public override IEnumerable<Type> GetFilteredTypeList()
        {
            List<Type> typeList = new List<Type>();
            if (mesh == null || material == null)
            {
                return typeList;
            }
            if (atlasMapping != null)
            {
                typeList.Add(typeof(HudImage));
                typeList.Add(typeof(HudNum));
            }
            if (fontAsset != null)
            {
                typeList.Add(typeof(HudText));
            }
            typeList.Add(typeof(HudSub));
            return typeList;
        }

#endif
    }

}