using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace NFCore.Extension
{

    public class HudSubCanvasRenderer : SerializedMonoBehaviour
    {
        [TypeFilter("GetFilteredTypeList", DrawValueNormally = false)]
        [VerticalGroup("componets", Order = 1)]
        [SerializeField]
        [OnCollectionChanged("Before", "After")]
        protected List<HudComponetBase> componets = new List<HudComponetBase>();

        private Dictionary<string, HudComponet> componetDir = new Dictionary<string, HudComponet>();

        [SerializeField]
        [ReadOnly]
        [HideInInspector]
        public HudCanvasRenderer canvasRenderer;

        private Transform m_trans;
        public Transform trans { get { return m_trans; } }

        private int mTransId = -1;
        public int transId { get { return mTransId; } }

        public int rootId
        {
            get
            {
                if (canvasRenderer == null) return -1;
                return canvasRenderer.transId;
            }
        }

        public virtual HudRendererBatch rendererBatch
        {
            get
            {
                if (canvasRenderer == null) return null;
                return canvasRenderer.rendererBatch; ;
            }
        }

        public AtlasMapping atlasMapping
        {
            get
            {
                if (canvasRenderer == null) return null;
                return canvasRenderer.atlasMapping;
            }
        }

        public TMP_FontAsset fontAsset
        {
            get
            {
                if (canvasRenderer == null) return null;
                return canvasRenderer.fontAsset;
            }
        }

        public Material material
        {
            get
            {
                if (canvasRenderer == null) return null;
                return canvasRenderer.material;
            }
        }

        protected virtual void Awake()
        {
            m_trans = transform;
            ExecuteAwake();
        }

        public void ExecuteAwake()
        {
            if (canvasRenderer == null) return;
            if (rendererBatch != null)
            {
                bool root = this as HudCanvasRenderer;
                mTransId = rendererBatch.AddTransform(m_trans, root);
            }
            for (int i = 0; i < componets.Count; i++)
            {
                if (componets[i].hudType != HudType.Sub)
                {
                    HudComponet hudcomponet = componets[i] as HudComponet;
                    if (!string.IsNullOrEmpty(hudcomponet.name))
                    {
                        componetDir[hudcomponet.name] = hudcomponet;
                    }
                    componets[i].Awake();
                }
            }
        }

        public T GetHudComponet<T>(string name) where T : HudComponet
        {
            HudComponet hudcomponet = null;
            componetDir.TryGetValue(name, out hudcomponet);
            return hudcomponet as T;
        }

        protected virtual void OnDestroy()
        {
            ExecuteDestroy();
        }

        public void ExecuteDestroy()
        {
            if (canvasRenderer == null) return;
            if (mTransId == -1) return;
            for (int i = 0; i < componets.Count; i++)
            {
                if (componets[i].hudType != HudType.Sub)
                {
                    componets[i].Destroy();
                }
            }
            RemoveData();
            rendererBatch?.RemoveTransform(transId);
            mTransId = -1;
        }

        public virtual void OnEnable()
        {
            rendererBatch.SetTransformEnable(transId, trans);
            for (int i = 0; i < componets.Count; i++)
            {
                componets[i].Enable();
            }
        }

        public virtual void OnDisable()
        {
            rendererBatch.SetTransformDisable(transId);
            for (int i = 0; i < componets.Count; i++)
            {
                componets[i].Disable();
            }
        }

        public virtual void OnReorder()
        {
            for (int i = componets.Count - 1; i >= 0; i--)
            {
                componets[i].OnReorder();
            }
        }

        public void TriggerReorder()
        {
            if (rendererBatch == null || canvasRenderer == null) return;
            rendererBatch.TriggerReorder(canvasRenderer);
        }

        public void RemoveData()
        {
            if (canvasRenderer == null) return;
            rendererBatch.RemoveData(rootId);
        }

#if UNITY_EDITOR
        public virtual IEnumerable<Type> GetFilteredTypeList()
        {
            List<Type> typeList = new List<Type>();
            if (canvasRenderer == null) return typeList;
            if (canvasRenderer.mesh == null || canvasRenderer.material == null)
            {
                return typeList;
            }
            if (canvasRenderer.atlasMapping != null)
            {
                typeList.Add(typeof(HudImage));
                typeList.Add(typeof(HudNum));
            }
            if (canvasRenderer.fontAsset != null)
            {
                typeList.Add(typeof(HudText));
            }
            return typeList;
        }

        public virtual void Before(CollectionChangeInfo info, object value)
        {
            //TriggerReorder();
        }

        public virtual void After(CollectionChangeInfo info, object value)
        {
            HudComponetBase hud = info.Value as HudComponetBase;
            if (hud != null)
            {
                hud.parent = this;
                if (info.ChangeType == CollectionChangeType.Add)
                {
                    hud.Awake();
                }
            }
            TriggerReorder();
        }
#endif

    }
}