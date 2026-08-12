using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

namespace NFCore.Extension
{
    public class HudNum : HudComponet
    {
        [VerticalGroup(VERTICAL_GROUP, Order = 2)]
        [SerializeField]
        [OnValueChanged("OnNumChange")]
        private int n_NumInt = -1;
        public int Num
        {
            get { return n_NumInt; }
            set
            {
                if (n_NumInt == value) return;
                n_NumInt = value;
                SetNum(n_NumInt);
            }
        }

        protected string m_type = "";
        protected string m_spriteName = "";


        [VerticalGroup(VERTICAL_GROUP)]
        [SerializeField]
        [OnValueChanged("OnNumChange")]
        private float m_FontSize = 1;


        /// <summary>
        /// 数字类型: 类型名字 -> (数字 -> 图片名字)
        /// EG：
        /// <普通伤害,<1, "1">>
        /// <暴击伤害,<2, "2">>
        /// </summary>
        private static Dictionary<string, Dictionary<int, string>> NumTypeDict;

        /// <summary>
        /// "+"号对应显示的类型
        /// </summary>
        private static string AddSymbolRelatedType = "Heal";

        /// <summary>
        /// 设置类型数字字典
        /// </summary>
        /// <param name="dict"></param>
        public static void SetNumTypeDict(Dictionary<string, Dictionary<int, string>> dict, string addSymbolRelatedType)
        {
            NumTypeDict = dict;
            AddSymbolRelatedType = addSymbolRelatedType;
        }


        public void SetNum(string type, int num)
        {
            quardCount = 0;
            Num = num;
            m_type = type;

            if (NumTypeDict == null) return;
            if (atlasMapping == null) return;
            if (m_NumList == null) m_NumList = new List<int>(10);
            GetDigitValues(num, ref m_NumList, out short signFlag);

            if (m_NumList.Count == 0)
            {
                ResizeDataSnippet(0);
                return;
            }

            ResizeDataSnippet(1);
            HudDataSnippet snippet = GetDataSnippet(0);
            if (snippet == null) return;
            snippet.ResetNineParam();

            int quadindex = 0;
            float curlen = 0.001f;
            SpriteInfo spriteInfo = null;
            string tempKey = null;
            for (int i = -1; i < m_NumList.Count; i++)
            {
                if (i == -1)//判断符号
                {
                    if (signFlag > 0)
                    {
                        //加血要增加号
                        if (type == AddSymbolRelatedType)
                        {
                            spriteInfo = atlasMapping.GetSpriteInfo("+");
                        }
                        else
                        {
                            continue;//不是加血，不显示‘+’
                        }
                    }
                    else
                    {
                        continue;//不显示‘-’
                    }
                }
                else
                {
                    tempKey = GetSpriteInfo(type, m_NumList[i]);//根据类型取对应数字的KEY

                    if (string.IsNullOrEmpty(tempKey))
                    {
                        continue;
                    }

                    spriteInfo = atlasMapping.GetSpriteInfo(tempKey);
                }

                if (spriteInfo != null)
                {
                    float2 size = new float2(spriteInfo.size.x, spriteInfo.size.y);
                    size = size * fontSize;
                    snippet.SetSpriteId(quadindex, spriteInfo.index);
                    snippet.SetSpriteQuad(quadindex, new float2(curlen, -size.y / 2), size);
                    curlen += size.x;
                    quadindex++;
                    if (quadindex >= quadLimit) break;
                }
            }
            for (int i = 0; i < quadindex; i++)
            {
                SetAlignment(curlen, snippet, i);
            }
            snippet.WriteParamData();

            quardCount = quadindex;
        }

        private string GetSpriteInfo(string type, int num)
        {
            string ret = null;
            if (NumTypeDict == null) return null;

            if (NumTypeDict.ContainsKey(type))
            {
                if (NumTypeDict[type].TryGetValue(num, out ret))
                {
                    //ret 图片名
                    return ret;
                }
            }
            return ret;
        }

        /// <summary>
        /// 直接设置显示的图片字
        /// </summary>
        /// <param name="spriteName"></param>
        public void SetText(string spriteName)
        {
            quardCount = 0;
            m_type = "";//这里要抹去type，不然设置字体大小时无法知道当前是要显示数字还是sprite
            m_spriteName = spriteName;//SetFontSize时有用


            ResizeDataSnippet(1);
            HudDataSnippet snippet = GetDataSnippet(0);
            if (snippet == null) return;
            snippet.ResetNineParam();

            int quadindex = 0;
            float curlen = 0.001f;

            var spriteInfo = atlasMapping.GetSpriteInfo(spriteName);

            if (spriteInfo != null)
            {
                float2 size = new float2(spriteInfo.size.x, spriteInfo.size.y);
                size = size * fontSize;
                snippet.SetSpriteId(quadindex, spriteInfo.index);
                snippet.SetSpriteQuad(quadindex, new float2(curlen, -size.y / 2), size);
                curlen += size.x;
                quadindex++;
            }
            for (int i = 0; i < quadindex; i++)
            {
                SetAlignment(curlen, snippet, i);
            }
            snippet.WriteParamData();

            quardCount = quadindex;
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        public float fontSize
        {
            get { return m_FontSize; }
            set
            {
                m_FontSize = value;
//                 if (string.IsNullOrEmpty(m_type))
//                 {
//                     SetText(m_spriteName);
//                 }
//                 else
//                 {
//                     //这里要判断当前显示模式是数字，还是图片
//                     SetNum(m_type, n_NumInt);
//                 }
            }
        }
//         public void SetFontSize(float fontSize)
//         {
// 
//         }

        [VerticalGroup(VERTICAL_GROUP, Order = 2)]
        [OnValueChanged("OnAlignmentChange")]
        [SerializeField]
        private HorizontalAlignment m_Alignment = HorizontalAlignment.Middle;

        private List<int> m_NumList = new List<int>(10);

        public HudNum()
        {
            m_HudType = HudType.Num;
        }

        public override void OnAwake()
        {
        }

        private void SetNum(int num)
        {
            quardCount = 0;
            if (atlasMapping == null) return;
            if (m_NumList == null) m_NumList = new List<int>(10);
            GetDigitValues(num, ref m_NumList, out short signFlag);

            if (m_NumList.Count == 0)
            {
                ResizeDataSnippet(0);
                return;
            }

            ResizeDataSnippet(1);
            HudDataSnippet snippet = GetDataSnippet(0);
            if (snippet == null) return;
            snippet.ResetNineParam();

            int quadindex = 0;
            float curlen = 0.001f;
            SpriteInfo spriteInfo = null;
            for (int i = -1; i < m_NumList.Count; i++)
            {
                if (i == -1)//判断符号
                {
                    if (signFlag > 0)
                    {
                        continue;//"+"不显示
                    }
                    else
                    {
                        spriteInfo = atlasMapping.GetSpriteInfo("-");
                    }
                }
                else
                {
                    spriteInfo = atlasMapping.GetSpriteInfo(GetSpriteInfo(m_NumList[i]));
                }

                if (spriteInfo != null)
                {
                    float2 size = new float2(spriteInfo.size.x, spriteInfo.size.y);
                    size = size * fontSize;
                    snippet.SetSpriteId(quadindex, spriteInfo.index);
                    snippet.SetSpriteQuad(quadindex, new float2(curlen, -size.y / 2), size);
                    curlen += size.x;
                    quadindex++;
                    if (quadindex >= quadLimit) break;
                }
            }
            for (int i = 0; i < quadindex; i++)
            {
                SetAlignment(curlen, snippet, i);
            }
            snippet.WriteParamData();

            quardCount = quadindex;
        }

        /// <summary>
        /// 将数字的每一位取出
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digits"></param>
        /// <param name="signFlag"></param>
        private static void GetDigitValues(int number, ref List<int> digits, out short signFlag)
        {
            digits.Clear();
            signFlag = 1;
            if (number == 0)
            {
                digits.Add(0);
                return;
            }

            // Handle negative numbers
            if (number < 0)
            {
                signFlag = -1;
                number = -number;
            }

            // Extract digits using mathematical operations
            while (number > 0)
            {
                digits.Add((int)(number % 10));
                number /= 10;
            }

            // The digits are in reverse order, so reverse the list
            digits.Reverse();
        }

        private string GetSpriteInfo(int numChar)
        {
            switch (numChar)
            {
                case 0:
                    return "0";
                case 1:
                    return "1";
                case 2:
                    return "2";
                case 3:
                    return "3";
                case 4:
                    return "4";
                case 5:
                    return "5";
                case 6:
                    return "6";
                case 7:
                    return "7";
                case 8:
                    return "8";
                case 9:
                    return "9";
                default:
                    return "0";
            }
        }

        public void SetAlignment(float lenght, HudDataSnippet snippet, int index)
        {
            switch (m_Alignment)
            {
                case HorizontalAlignment.Left:

                    break;
                case HorizontalAlignment.Middle:
                    {
                        float2 pos = snippet.GetSpritePosition(index);
                        pos += new float2(-lenght / 2, 0);
                        snippet.SetSpritePositon(index, pos);
                    }
                    break;
                case HorizontalAlignment.Right:
                    {
                        float2 pos = snippet.GetSpritePosition(index);
                        pos += new float2(-lenght, 0);
                        snippet.SetSpritePositon(index, pos);
                    }
                    break;
            }
        }

#if UNITY_EDITOR
        public void OnNumChange()
        {
            //SetNum(m_Num);
            SetNum(n_NumInt);
        }

        public void OnAlignmentChange()
        {
            SetNum(n_NumInt);
        }

        //public override void OnScaleChange()
        //{
        //    SetNum(m_Num);
        //}

#endif
        //-------------------------------------------------------------------------------------
        #region 狗写的代码

        private int quardCount;
        public int QuadCount => quardCount;

        #endregion
        //-------------------------------------------------------------------------------------
    }
}
