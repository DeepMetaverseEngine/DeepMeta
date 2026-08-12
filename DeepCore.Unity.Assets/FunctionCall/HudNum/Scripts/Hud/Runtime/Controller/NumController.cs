using System;
using System.Drawing;
using UnityEngine;

namespace NFCore.Extension
{
    public class NumController
    {
        //public static float DefaultDuration = 1f;
        private const byte STATUS_ERROR = 2;
        private const byte STATUS_RUN = 0;
        private const byte STATUS_STOP = 1;


        private byte _STATUS;
        private HudNum _Num;
        private Transform _NumTransform;

        /// <summary>
        /// 默认字体大小
        /// </summary>
        public float DefaultFontSize { get; set; } = 0.5f;
        public float DurationSeconds { get => _Duration; set => _Duration = value; }
        private float _Duration;
        private string curveName;

        /// <summary>
        /// 坐标偏移数据
        /// </summary>
        private Vector3 _PositionOffsetData;

        /// <summary>
        /// 拍摄相机的世界坐标
        /// </summary>
        private Vector3 _AfterConvertWorldPos;

        /// <summary>
        /// 主摄像机拍摄的世界坐标
        /// </summary>
        private Vector3 _BeforeConvertWorldPos;

        private float _Angle=0;
        private Vector2 _Scale = Vector2.one;


        public delegate void NumberDelegate(ref Vector3 tempV, ref float angle, ref Vector2 scale);
        public NumberDelegate DoUpdate;
        public NumberDelegate DoStart;
        public NumController()
        {
        }

        private void SetSTATUS(byte status)
        {
            if (_STATUS == STATUS_ERROR)
                return;

            _STATUS = status;
        }

        public void Bind(GameObject go)
        {
            HudCanvasRenderer canvasRenderer = go.GetComponent<HudCanvasRenderer>();
            if (canvasRenderer != null)
            {
                HudNum hudNum = canvasRenderer.GetHudComponet<HudNum>("Num");
                if (hudNum == null)
                {
                    Debug.LogError("BattleNumController bind Error: can not find HudNum");
                }
                else
                {
                    _Num = hudNum;
                    _NumTransform = go.transform;
                }
            }
        }

        public void Init(Vector3 worldPos)
        {
            //ClearParam();
            //InitShowParam();
            _PositionOffsetData = Vector3.zero;
            _Angle = 0;
            _Scale = Vector2.one;
            var pos = worldPos;
         
            _BeforeConvertWorldPos = pos;
            _AfterConvertWorldPos = GetRenderPos(pos); 
            if (DoStart != null)
            {
                DoStart(ref _PositionOffsetData, ref _Angle, ref _Scale);
            }
            _NumTransform.position = _AfterConvertWorldPos;//世界坐标转换;
        }

        /// <summary>
        /// 根据世界坐标转换成拍摄的坐标
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        private Vector3 GetRenderPos(in Vector3 worldPos)
        {
            return HudRendererBatch.WordCameraPosToUICameraPos(worldPos);
        }

        /// <summary>
        /// 获取插值坐标，屏幕不动的情况下，应该是0
        /// </summary>
        /// <returns></returns>
        private Vector3 CalDiffPos()
        {
            var dif = GetRenderPos(_BeforeConvertWorldPos) - _AfterConvertWorldPos;
            //LogUtil.Log($"dif = {dif}");
            //----------------------------------------------------------------
            //因为底层会对传进的坐标进行*0.001f操作，这里要先将偏移量放大
            dif *= 100;
            //----------------------------------------------------------------
            return dif;
        }

        public void Update()
        {
            if (_STATUS == STATUS_RUN)
            {
                //if (a != 0 && b != 0 && c != 0)
                {
                    //--------------------------------------------------
                    //1.飘字效果部分
                    //--------------------------------------------------
                    //x偏移
                    var tempV = _PositionOffsetData;
                    //tempV.x += xSpeed * Time.deltaTime;
                    //var x = tempV.x;
                    //计算新的y
                    //tempV.y = -a * x * x - b * x;

                    if (DoUpdate != null)
                    {
                        DoUpdate(ref tempV, ref _Angle, ref _Scale);
                    }
                    //                     tempV.y += Tiny.Data.GameConfig;
                    //                     DeepCore.CMath.RandomPosInRound();
                    //--------------------------------------------------
                    //2.伤害数字效果是world模式，所以要计算镜头移动时产生的位移量，补偿给offset，不然玩家移动时，伤害数字位置始终会在原地
                    //--------------------------------------------------
                    var newScreenOffset = CalDiffPos();
                    //--------------------------------------------------
                    //3.效果叠加
                    //--------------------------------------------------
                    var finalVec = Vector3.zero;
                    finalVec.x = newScreenOffset.x + tempV.x;
                    finalVec.y = newScreenOffset.y - tempV.y;//坐标系不一样，这里是 + (-temp.y)
                                                             //--------------------------------------------------
                    _Num.SetPosition(finalVec);
                    _Num.SetAngle(_Angle);
                    _Num.SetScale(_Scale);
                    _Num.Flush();
                    _PositionOffsetData = tempV;
                }

                _Duration -= Time.deltaTime;
                if (_Duration < 0)
                {
                    SetSTATUS(STATUS_STOP);
                    _Num.SetShow(false);
                }
            }
        }


        //         private void InternalSetNum(int num)
        //         {
        //             _Num.Num = num;
        //         }
        // 
        //         private void InternalSetNum(string type, int num)
        //         {      _Num.SetNum(type, num);
        //             _Num.SetNum(type, num);
        //         }
        // 
        //         private void InternalSetVisible(bool visible)
        //         {
        //             _Num.SetShow(visible);
        //             //这里先不要去管，setactive 本身有较高的GC
        //             //_NumTransform.gameObject.SetActive(visible);
        //         }


        public bool IsEnd()
        {
            return _STATUS == STATUS_ERROR || _STATUS == STATUS_STOP;
        }

        private void Reset()
        {
            if (_NumTransform) _NumTransform.gameObject.SetActive(true);
            _Num.SetPosition(Vector2.zero);
            _Duration = 1;
        }
// 
// 
//         public void ShowHeal(string type, int v)
//         {
//             Reset();
//             SetSTATUS(STATUS_RUN);
//             _Num.SetNum(type, v);
//             _Num.SetShow(true);
//             _Num.Flush();
//         }

//         public void Show(int v, string tweenName = null)
//         {
//             Reset();
//             SetSTATUS(STATUS_RUN);
//             _Num.fontSize = DefaultFontSize; 
//             _Num.Num = v;
//             _Num.SetShow(true);
//             _Num.Flush();
//         }

        /*   public void Show(int v, int tweenType = 0)
           {
               Reset();
               SetSTATUS(STATUS_RUN);
               InternalSetNum(v);
               InternalSetVisible(true);
               InternalSetFontSize(DefaultFontSize);
           }*/

        public void Show(string type, int num, string tweenName)
        {
            Reset();
            curveName = tweenName;
            SetSTATUS(STATUS_RUN);
            _Num.fontSize = DefaultFontSize;
            _Num.SetNum(type, num);
            _Num.SetShow(true);
            _Num.Flush();
        }

        public void ShowMiss(string spriteName, string tweenName)
        {
            Reset();
            SetSTATUS(STATUS_RUN);
            curveName = tweenName;
            _Num.fontSize = DefaultFontSize;
            _Num.SetText(spriteName);
            _Num.SetShow(true);
            _Num.Flush();
        }

        public void ShowImune(string spriteName)
        {
            Reset();
            SetSTATUS(STATUS_RUN);
            _Num.fontSize = DefaultFontSize;
            _Num.SetText(spriteName);
            _Num.SetShow(true);
            _Num.Flush();
        }

        public void Release()
        {
            DoStart = null;
            DoUpdate = null;
            if (_NumTransform)
            {
                GameObject.Destroy(_NumTransform.gameObject);
            }
        }

        public void Hide()
        {
            _Num.SetShow(false);
            if (_NumTransform) _NumTransform.gameObject.SetActive(false);
        }
    }
}