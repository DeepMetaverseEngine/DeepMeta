using DeepCore.GUI.Data;
using DeepCore.Unity;
using System.Collections.Generic;
using UnityEngine;
using static NFCore.Extension.HudRendererBatch;

namespace NFCore.Extension
{
    public delegate void OnInitHudNum(Dictionary<string, Dictionary<int, string>> HudNumTypeDict);

    public class HudObjectManager : MonoBehaviour
    {
        private static HudObjectManager _Ins = null;

        /// <summary>
        /// HUDNum Prefab
        /// </summary>
        public GameObject HudNumObj;

        /// <summary>
        /// 负责绘制的相机
        /// </summary>
        public Camera UICamera;
        /// <summary>
        /// 负责战斗的相机
        /// </summary>
        public Camera MainCamera;

        public string RenderLayer = "UI";
        public RenderPipline Pipline = RenderPipline.Buildin;

        private static Dictionary<string, Dictionary<int, string>> HudNumTypeDict;

        public static OnInitHudNum onInitHudNum;

        #region 项目自定义TextType
        public const string TEXTTYPE_Heal = "Heal";//治疗
        public const string TEXTTYPE_MP = "MP";//治疗
        public const string TEXTTYPE_Normal = "Normal";//普通攻击
        public const string TEXTTYPE_Critical = "Critical";//暴击
        public const string TEXTTYPE_Restrain = "Restrain";//克制伤害
        public const string TEXTTYPE_Restrained = "Restrained";//被克制伤害
        public const string TEXTTYPE_Immune = "immune";//免疫
        public const string TEXTTYPE_Miss = "miss";//未命中
        #endregion

        //private List<NumController> _NumList = new List<NumController>(50);

        private int _ObjLayerId = 0;
        private bool _Enable = false;
        public static HudObjectManager Ins
        {
            get
            {
                return _Ins;
            }
        }

        private void Awake()
        {
            _Ins = this;
        }

        void Start()
        {
            Init();
        }

        private void OnDisable()
        {
            _Enable = false;
        }

        private void OnEnable()
        {
            _Enable = true;
        }

        // Update is called once per frame
        //         void Update()
        //         {
        //             NumController nc = null;
        // 
        //             for (int i = _NumList.Count - 1; i >=0; i--)
        //             {
        //                 nc =  _NumList[i];
        //                 nc.Update();
        //                 if (nc.IsEnd())
        //                 {
        //                     _NumList.Remove(nc);
        //                 }
        //             }
        //         }

        private void Init()
        {
            this.gameObject.AddComponent<HudRun>();
            InitNumTypeDict();
            /*       var cameranode = GameObject.FindWithTag("UICamera");
                   if (cameranode != null) { RenderCamera = cameranode.GetComponent<Camera>(); }*/
            {
                var camera = this.UICamera;
                if (camera == null)
                {
                    camera = Camera.main;
                }
                HudRendererBatch.UICamera = camera;
            }
            {
                var camera = this.MainCamera;
                if (GetComponent<Camera>() == null)
                {
                    camera = Camera.main;
                }
                HudRendererBatch.MainCamera = camera;
            }
            {
                HudRendererBatch.Pipline = this.Pipline;
            }
            try
            {
                if (string.IsNullOrEmpty(this.RenderLayer))
                {
                    this.RenderLayer = "UI";
                }
                HudRendererBatch.RenderLayer = LayerMask.NameToLayer(this.RenderLayer);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("HudObjectManager Init Error:" + ex.ToString());
            }
            _ObjLayerId = HudRendererBatch.RenderLayer;
        }


        /// <summary>
        /// 设置类型数字对应的文字图片信息
        /// </summary>
        private void InitNumTypeDict()
        {
            if (HudNumTypeDict == null)
            {
                HudNumTypeDict = new Dictionary<string, Dictionary<int, string>>();
               
                var subDict = new Dictionary<int, string>();
                //-----------------------------------------------------------------------------
                //普通伤害 白字0-9
                subDict.Add(0, "w0");
                subDict.Add(1, "w1");
                subDict.Add(2, "w2");
                subDict.Add(3, "w3");
                subDict.Add(4, "w4");
                subDict.Add(5, "w5");
                subDict.Add(6, "w6");
                subDict.Add(7, "w7");
                subDict.Add(8, "w8");
                subDict.Add(9, "w9");

                HudNumTypeDict.Add(TEXTTYPE_Normal, subDict);

                //-----------------------------------------------------------------------------
                //克制伤害 黄字0-9
                subDict = new Dictionary<int, string>();
                subDict.Add(0, "y0");
                subDict.Add(1, "y1");
                subDict.Add(2, "y2");
                subDict.Add(3, "y3");
                subDict.Add(4, "y4");
                subDict.Add(5, "y5");
                subDict.Add(6, "y6");
                subDict.Add(7, "y7");
                subDict.Add(8, "y8");
                subDict.Add(9, "y9");

                HudNumTypeDict.Add(TEXTTYPE_Restrain, subDict);

                //-----------------------------------------------------------------------------
                //被克制伤害 灰色0-9
                subDict = new Dictionary<int, string>();
                subDict.Add(0, "grey0");
                subDict.Add(1, "grey1");
                subDict.Add(2, "grey2");
                subDict.Add(3, "grey3");
                subDict.Add(4, "grey4");
                subDict.Add(5, "grey5");
                subDict.Add(6, "grey6");
                subDict.Add(7, "grey7");
                subDict.Add(8, "grey8");
                subDict.Add(9, "grey9");

                HudNumTypeDict.Add(TEXTTYPE_Restrained, subDict);

                //-----------------------------------------------------------------------------
                //我方头顶伤害 红色0-9
                subDict = new Dictionary<int, string>();
                subDict.Add(0, "r0");
                subDict.Add(1, "r1");
                subDict.Add(2, "r2");
                subDict.Add(3, "r3");
                subDict.Add(4, "r4");
                subDict.Add(5, "r5");
                subDict.Add(6, "r6");
                subDict.Add(7, "r7");
                subDict.Add(8, "r8");
                subDict.Add(9, "r9");

                HudNumTypeDict.Add(TEXTTYPE_Critical, subDict);

                //-----------------------------------------------------------------------------
                //治疗 绿色0-9
                subDict = new Dictionary<int, string>();

                subDict.Add(0, "g0");
                subDict.Add(1, "g1");
                subDict.Add(2, "g2");
                subDict.Add(3, "g3");
                subDict.Add(4, "g4");
                subDict.Add(5, "g5");
                subDict.Add(6, "g6");
                subDict.Add(7, "g7");
                subDict.Add(8, "g8");
                subDict.Add(9, "g9");

                HudNumTypeDict.Add(TEXTTYPE_Heal, subDict);
                //-----------------------------------------------------------------------------
                //MP 蓝色0-9
                subDict = new Dictionary<int, string>();

                subDict.Add(0, "b0");
                subDict.Add(1, "b1");
                subDict.Add(2, "b2");
                subDict.Add(3, "b3");
                subDict.Add(4, "b4");
                subDict.Add(5, "b5");
                subDict.Add(6, "b6");
                subDict.Add(7, "b7");
                subDict.Add(8, "b8");
                subDict.Add(9, "b9");

                HudNumTypeDict.Add(TEXTTYPE_MP, subDict);

                //                 //-----------------------------------------------------------------------------
                //                 //格挡 蓝色0-9
                //                 subDict = new Dictionary<int, string>();
                // 
                //                 subDict.Add(0, "b0");
                //                 subDict.Add(1, "b1");
                //                 subDict.Add(2, "b2");
                //                 subDict.Add(3, "b3");
                //                 subDict.Add(4, "b4");
                //                 subDict.Add(5, "b5");
                //                 subDict.Add(6, "b6");
                //                 subDict.Add(7, "b7");
                //                 subDict.Add(8, "b8");
                //                 subDict.Add(9, "b9");
                // 
                //                 HudNumTypeDict.Add(TEXTTYPE_Block, subDict);

                onInitHudNum?.Invoke(HudNumTypeDict);

                HudNum.SetNumTypeDict(HudNumTypeDict, TEXTTYPE_Heal);
            }
        }

        public NumController CreateNumber(Transform parent)
        {
            var ret = new NumController();
            var numGO = GameObject.Instantiate(HudNumObj);
            numGO.layer = _ObjLayerId;
            numGO.transform.Parent(parent, false);
            ret.Bind(numGO);
            //_NumList.Add(ret);

            return ret;
        }


        public void ShowNum(NumController nc, Vector3 trans, int v, bool isCritical)
        {
            if (!_Enable) return;

            //var nc = CreateNumber();
            nc.Init(trans);
            if (isCritical)
            {
                nc.Show(TEXTTYPE_Critical, v, null);
            }
            else
            {
                nc.Show(TEXTTYPE_Normal, v, null);
            }
            //_NumList.Add(nc);
        }

        public void ShowMiss(NumController nc, Vector3 trans)
        {
            if (!_Enable) return;
            //var nc = CreateNumber();
            nc.Init(trans);
            nc.Show(TEXTTYPE_Miss, 0, null);
            //_NumList.Add(nc);
        }

        public void ShowHeal(NumController nc, Vector3 trans, int v)
        {
            if (!_Enable) return;
            //var nc = CreateNumber();
            nc.Init(trans);
            nc.Show(TEXTTYPE_Heal, v, null);
            //_NumList.Add(nc);
        }
        public void ShowMP(NumController nc, Vector3 trans, int v)
        {
            if (!_Enable) return;
            //var nc = CreateNumber();
            nc.Init(trans);
            nc.Show(TEXTTYPE_MP, v, null);
            //_NumList.Add(nc);
        }
        //blue
        public void ShowBlock(NumController nc, Vector3 trans, int v)
        {
            if (!_Enable) return;
            //var nc = CreateNumber();
            nc.Init(trans);
            nc.Show(TEXTTYPE_Restrained, v, null);
            //_NumList.Add(nc);
        }
    }
}