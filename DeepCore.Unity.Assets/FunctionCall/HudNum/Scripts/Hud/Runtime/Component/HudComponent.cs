using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using Unity.Mathematics;
using System.Collections.Generic;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NFCore.Extension
{

    public enum HudType
    {
        Text,
        Image,
        Num,
        Sub
    }



    public class HudComponetBase
    {
        protected const string VERTICAL_GROUP = "$hudType/Vertical";
        protected const string VERTICAL_HORIZONTAL_GROUP = "$hudType/Vertical/Horizontal";
        protected const string VERTICAL_HORIZONTAL_VERTICAL_GROUP = "$hudType/Vertical/Horizontal/Vertical";

        [SerializeField]
        [BoxGroup("$hudType")]
        [ReadOnly]
        protected HudType m_HudType;

        [SerializeField]
        [ReadOnly]
        [HideInInspector]
        public HudSubCanvasRenderer parent;

        public HudType hudType { get { return m_HudType; } }

        public HudRendererBatch rendererBatch
        {
            get { return parent.rendererBatch; }
        }

        public int transId { get { return parent.transId; } }

        public int rootId { get { return parent.rootId; } }

        public AtlasMapping atlasMapping { get { return parent.atlasMapping; } }

        public TMP_FontAsset fontAsset { get { return parent.fontAsset; } }

        public Material material { get { return parent.material; } }

        private bool isinit = false;

        public void Awake()
        {
            if (rendererBatch == null) return;
            if (isinit) return;
            isinit = true;
            OnAwake();
        }

        public void Enable()
        {
            if (rendererBatch == null) return;
            if (isinit)
            {
                OnEnable();
                return;
            }
            isinit = true;
            OnAwake();
            OnEnable();
        }

        public void Disable()
        {
            if (rendererBatch == null) return;
            if (!isinit) return;
            OnDisable();
        }

        public void Destroy()
        {

        }

        public virtual void OnAwake()
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnReorder()
        {

        }
    }

    public class HudComponet : HudComponetBase
    {
        protected const int quadLimit = 9;//9个片

        protected List<HudDataSnippet> dataSnippets;

        [VerticalGroup(VERTICAL_GROUP, Order = 2)]
        [SerializeField]
        [OnValueChanged("OnShowChange")]
        private bool m_Show = true;

        [VerticalGroup(VERTICAL_GROUP, Order = 2)]
        [SerializeField]
        protected string m_Name;

        public string name { get { return m_Name; } }

        [VerticalGroup(VERTICAL_GROUP, Order = 2)]
        [SerializeField]
        [OnValueChanged("OnPositionChange")]
        protected Vector2 m_Position;

        public Vector2 position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                SetPosition(m_Position);
            }
        }

        //[VerticalGroup(VERTICAL_GROUP, Order = 2)]
        //[SerializeField]
        //[OnValueChanged("OnScaleChange")]
        //[HideIf("m_HudType", HudType.Text)]
        //protected Vector2 m_Scale = Vector2.one;

        //public Vector2 scale
        //{
        //    get { return m_Scale; }
        //    set
        //    {
        //        m_Scale = value;
        //        SetScale(m_Scale);
        //    }
        //}

        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnAngleChange")]
        protected float m_Angle;

        public float angle
        {
            get { return m_Angle; }
            set
            {
                m_Angle = value;
                SetAngle(m_Angle);
            }
        }

        [VerticalGroup(VERTICAL_GROUP, Order = 2)]
        [SerializeField]
        [OnValueChanged("OnColorChange")]
        protected Color m_Color = Color.white;

        public Color color
        {
            set
            {
                m_Color = value;
                SetColor(m_Color);
            }
            get
            {
                return m_Color;
            }
        }

        public override void OnEnable()
        {
            SetEnable(true);
        }

        public override void OnDisable()
        {
            SetEnable(false);
        }

        public void ResizeDataSnippet(int count)
        {
            if (dataSnippets == null) { dataSnippets = new List<HudDataSnippet>(); };
            if (dataSnippets.Count == count) return;
            if (dataSnippets.Count < count)
            {
                int deltaCount = count - dataSnippets.Count;
                for (int i = 0; i < deltaCount; i++)
                {
                    HudDataSnippet snippet = new HudDataSnippet(this);
                    snippet.Init(m_Show, transId);
                    snippet.SetColor(m_Color);
                    snippet.SetPosition(m_Position / 100f);
                    snippet.SetAngle(m_Angle);
                    snippet.WriteData();
                    dataSnippets.Add(snippet);
                }
                parent.TriggerReorder();
            }
            else
            {
                int deltaCount = dataSnippets.Count - count;
                for (int i = 0; i < deltaCount; i++)
                {
                    dataSnippets.RemoveAt(dataSnippets.Count - 1);
                }
                parent.TriggerReorder();
            }
        }

        public HudDataSnippet GetDataSnippet(int index)
        {
            if (dataSnippets == null) return null;
            if (index >= dataSnippets.Count) return null;
            return dataSnippets[index];
        }

        public override void OnReorder()
        {
            if (dataSnippets == null) return;
            for (int i = dataSnippets.Count - 1; i >= 0; i--)
            {
                dataSnippets[i].OnReorder();
            }
            dirty = true;
        }

        public void SetColor(Color color)
        {
            if (dataSnippets == null) return;
            for (int i = 0; i < dataSnippets.Count; i++)
            {
                dataSnippets[i].SetColor(color);
            }
            dirty = true;
        }

        public void SetPosition(Vector2 position)
        {
            if (dataSnippets == null) return;
            for (int i = 0; i < dataSnippets.Count; i++)
            {
                dataSnippets[i].SetPosition(position * 0.01f);//这里是将数值变小，避免位移过大
            }
            dirty = true;
        }

        public void SetAngle(float angle)
        {
            if (dataSnippets == null) return;
            for (int i = 0; i < dataSnippets.Count; i++)
            {
                dataSnippets[i].SetAngle(angle);
            }
            dirty = true;
        }
        public void SetShow(bool show)
        {
            if (dataSnippets == null) return;
            for (int i = 0; i < dataSnippets.Count; i++)
            {
                dataSnippets[i].SetShow(show);
            }
           // dirty = true;
        }

        public void SetEnable(bool enable)
        {
            if (dataSnippets == null) return;
            for (int i = 0; i < dataSnippets.Count; i++)
            {
                dataSnippets[i].SetEnable(enable);
            }
          //  dirty = true;
        }

        private void OnShowChange()
        {
            SetShow(m_Show);
        }

        private void OnAngleChange()
        {
            SetAngle(m_Angle);
        }

        private void OnPositionChange()
        {
            SetPosition(m_Position);
        }

        private void OnColorChange()
        {
            SetColor(m_Color);
        }

        //-------------------------------------------------------------------------------------
        #region 狗写的代码

        public void SetScale(Vector2 scale)
        {
            if (dataSnippets == null) return;
            for (int i = 0; i < dataSnippets.Count; i++)
            {
                //dataSnippets[i].SetSpritePositon(q, new float2(curlen, -size.y / 2));
                //dataSnippets[i].SetSpriteSize(q, scale);
                dataSnippets[i].SetScale(scale);
            }
            dirty = true;
        }

        private bool dirty = true;
        public void Flush()
        {
            if (dataSnippets == null) return;
            if (dirty)
            {
                for (int i = 0; i < dataSnippets.Count; i++)
                {
                    dataSnippets[i].WriteParamData();
                }
            }
            dirty = false;
        }

        #endregion
    }
}
