using NFCore.Jobs;
using System.Drawing;
using System;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace NFCore.Extension
{

    public class Float4Value
    {
        public int idx = -1;
        public RenderDataState<float4> value;
    }

    public class Float4x4Value
    {
        public int idx = -1;
        public RenderDataState<float4x4> value;
    }

    public class TransformIdValue
    {
        public int idx = -1;
        public RenderDataState<int> value;
    }

    //public struct RenderParam
    //{
    //    public float4x4 param1;
    //    public float4x4 param2;

    //    public RenderParam(float4x4 _param1, float4x4 _param2)
    //    {
    //        param1 = _param1;
    //        param2 = _param2;
    //    }

    //    private unsafe void SetValue(int paramIndex, float value)
    //    {
    //        int index = paramIndex % 16;
    //        if (paramIndex >= 16)
    //        {
    //            fixed (float4x4* array = &param2)
    //            {
    //                ((float*)array)[index] = value;
    //            }
    //        }
    //        else
    //        {
    //            fixed (float4x4* array = &param1)
    //            {
    //                ((float*)array)[index] = value;
    //            }
    //        }
    //    }

    //    private unsafe float GetValue(int paramIndex)
    //    {
    //        int index = paramIndex % 16;
    //        if (paramIndex >= 16)
    //        {
    //            fixed (float4x4* array = &param2)
    //            {
    //                return ((float*)array)[index];
    //            }
    //        }
    //        else
    //        {
    //            fixed (float4x4* array = &param1)
    //            {
    //                return ((float*)array)[index];
    //            }
    //        }
    //    }

    //    public void SetSpriteId(int index, int spriteId)
    //    {
    //        int valueindex = index / 2;
    //        int parity = index % 2;
    //        int floatIndex = 9 + valueindex;
    //        float fv = GetValue(floatIndex);
    //        float2 curfv = Utils.ToTowFloat(fv);
    //        curfv[parity] = spriteId + 1;
    //        SetValue(floatIndex, Utils.ToOneFloat(curfv.x, curfv.y));
    //    }

    //    public void SpritePositon(int index, float2 position)
    //    {
    //        float fv = Utils.ToOneFloat(position.x, position.y);
    //        int paramIndex = index;
    //        SetValue(paramIndex, fv);
    //    }

    //    public void SpriteSize(int index, float2 size)
    //    {
    //        float fv = Utils.ToOneFloat(size.x, size.y);
    //        int paramIndex = 16 + index;
    //        SetValue(paramIndex, fv);
    //    }

    //    public void SetAlignment(float len)
    //    {
    //        float4 c3 = param2.c3;
    //        c3.y = len;
    //        param2.c3 = c3;
    //    }

    //    public void SetColor(Color32 color)
    //    {
    //        float2 color2 = Utils.ColorToFloat(color);
    //        float4 c3 = param2.c3;
    //        c3.zw = color2;
    //        param2.c3 = c3;
    //    }

    //    public void SetAngle(float angle)
    //    {
    //        float4 c3 = param2.c3;
    //        c3.x = angle * Mathf.Deg2Rad;
    //        param2.c3 = c3;
    //    }

    //    public void SetAmount(float amount, float origin, float method)
    //    {
    //        float4 c2 = param2.c2;
    //        c2.w = amount;
    //        c2.z = origin;
    //        c2.y = method;
    //        param2.c2 = c2;
    //    }

    //    public void SetTmpParam(float padding, float scale)
    //    {
    //        float4 c2 = param2.c2;
    //        c2.w = padding;
    //        c2.z = scale;
    //        c2.y = 1;
    //        param2.c2 = c2;
    //    }
    //}

    //public class HudDataSnippet
    //{
    //    private TransformIdValue transValue = new TransformIdValue();
    //    private Dictionary<string, Float4Value> float4value = new Dictionary<string, Float4Value>();
    //    private Dictionary<string, Float4x4Value> float4x4value = new Dictionary<string, Float4x4Value>();
    //    private Vector2 componentPosition= Vector2.zero;
    //    private Vector2 snippetPosition = Vector2.zero;
    //    private HudComponetBase componet;
    //    public HudDataSnippet(HudComponetBase _componet)
    //    {
    //        componet = _componet;
    //    }

    //    public void Init(bool show)
    //    {
    //        m_Show = show;
    //        m_Enable = true;
    //        SetFloat4("_PosAndSize", float4.zero);
    //        SetFloat4x4("_Param1", float4x4.identity);
    //        SetFloat4x4("_Param2", float4x4.identity);
    //    }

    //    private HudRendererBatch rendererBatch 
    //    { 
    //        get { return componet.rendererBatch; }
    //    }

    //    private int rootId { get { return componet.rootId; } }

    //    private bool m_Show = true;

    //    public void SetShow(bool show)
    //    {
    //        if (show == m_Show) return;
    //        m_Show = show;
    //        UpdateShowState();
    //    }

    //    private bool m_Enable = true;

    //    public void SetEnable(bool enable)
    //    {
    //        if (enable == m_Enable) return;
    //        m_Enable = enable;
    //        UpdateShowState();
    //    }

    //    private byte CanShow() { return (m_Show && m_Enable)?(byte)1:(byte)0; }

    //    private void UpdateShowState()
    //    {
    //        foreach (var item in float4value)
    //        {
    //            RenderDataState<float4> datastate = item.Value.value;
    //            datastate.show = CanShow();
    //            item.Value.value = datastate;
    //            int idx = item.Value.idx;
    //            if (idx != -1)
    //            {
    //                rendererBatch.SetFloat4(item.Key, idx, datastate);
    //            }
    //        }
    //        foreach (var item in float4x4value)
    //        {
    //            RenderDataState<float4x4> datastate = item.Value.value;
    //            datastate.show = CanShow();
    //            item.Value.value = datastate;
    //            int idx = item.Value.idx;
    //            if (idx != -1)
    //            {
    //                rendererBatch.SetFloat4x4(item.Key, idx, datastate);
    //            }
    //        }
    //        {
    //            RenderDataState<int> datastate = transValue.value;
    //            datastate.show = CanShow();
    //            transValue.value = datastate;
    //            int idx = transValue.idx;
    //            if (idx != -1)
    //            {
    //                rendererBatch.SetTransformId(transValue.idx, datastate);
    //            }
    //        }
    //    }

    //    public void SetColor(Color32 color)
    //    {
    //        float4x4 f4x4 = GetFloat4x4("_Param2");
    //        float2 color2 = Utils.ColorToFloat(color);
    //        float4 c3 = f4x4.c3;
    //        c3.zw = color2;
    //        f4x4.c3 = c3;
    //        SetFloat4x4("_Param2", f4x4);
    //    }

    //    public void SetAngle(float angle)
    //    {
    //        float4x4 f4x4 = GetFloat4x4("_Param2");
    //        float4 c3 = f4x4.c3;
    //        c3.x = angle * Mathf.Deg2Rad;
    //        f4x4.c3 = c3;
    //        SetFloat4x4("_Param2", f4x4);
    //    }

    //    public void SetComPosition(Vector2 position)
    //    {
    //        componentPosition = position;
    //        float4 f4 = GetFloat4("_PosAndSize");
    //        f4.xy = componentPosition + snippetPosition;
    //        SetFloat4("_PosAndSize", f4);
    //    }

    //    public Vector2 GetComPosition()
    //    {
    //        return componentPosition;
    //    }

    //    public void SetSnippetPosition(Vector2 position)
    //    {
    //        snippetPosition = position;
    //        float4 f4 = GetFloat4("_PosAndSize");
    //        f4.xy = componentPosition + snippetPosition;
    //        SetFloat4("_PosAndSize", f4);
    //    }

    //    public Vector2 GetSnippetPosition()
    //    {
    //        return snippetPosition;
    //    }

    //    public void SetScale(Vector2 scale)
    //    {
    //        float4 f4 = GetFloat4("_PosAndSize");
    //        f4.zw = scale;
    //        SetFloat4("_PosAndSize", f4);
    //    }

    //    public RenderParam GetRenderParam()
    //    {
    //        float4x4 parma1 = GetFloat4x4("_Param1");
    //        ClearNineParam(ref parma1);
    //        float4x4 parma2 = GetFloat4x4("_Param2");
    //        ClearNineParam(ref parma2);
    //        RenderParam param = new RenderParam(parma1, parma2);
    //        return param;
    //    }

    //    private void ClearNineParam(ref float4x4 parma)
    //    {
    //        parma.c0 = new float4();
    //        parma.c1 = new float4();
    //        float4 c2 = parma.c2;
    //        c2.x = 0;
    //        parma.c2 = c2;
    //    }

    //    public void SetRenderParam(RenderParam param)
    //    {
    //        SetFloat4x4("_Param1", param.param1);
    //        SetFloat4x4("_Param2", param.param2);
    //    }

    //    public void SetFloat4(string name, float4 value)
    //    {
    //        RenderDataState<float4> datastate = new RenderDataState<float4>(value, CanShow());
    //        Float4Value f4v = null;
    //        if (!float4value.TryGetValue(name, out f4v))
    //        {
    //            f4v = new Float4Value();
    //            f4v.value = datastate;
    //            float4value[name] = f4v;
    //            return;
    //        }
    //        f4v.value = datastate;
    //        if (f4v.idx != -1)
    //        {
    //            rendererBatch.SetFloat4(name, f4v.idx, datastate);
    //        }
    //    }

    //    public float4 GetFloat4(string name)
    //    {
    //        Float4Value f4v = null;
    //        if (float4value.TryGetValue(name, out f4v))
    //        {
    //            return f4v.value.data;
    //        }
    //        return new float4();
    //    }

    //    private void SetFloat4x4(string name, float4x4 value)
    //    {
    //        RenderDataState<float4x4> datastate = new RenderDataState<float4x4>(value, CanShow());
    //        Float4x4Value f4x4v = null;
    //        if (!float4x4value.TryGetValue(name, out f4x4v))
    //        {
    //            f4x4v = new Float4x4Value();
    //            f4x4v.value = datastate;
    //            float4x4value[name] = f4x4v;
    //            return;
    //        }
    //        f4x4v.value = datastate;
    //        if (f4x4v.idx != -1)
    //        {
    //            rendererBatch.SetFloat4x4(name, f4x4v.idx, datastate);
    //        }
    //    }

    //    private float4x4 GetFloat4x4(string name)
    //    {
    //        Float4x4Value f4x4v = null;
    //        if (float4x4value.TryGetValue(name, out f4x4v))
    //        {
    //            return f4x4v.value.data;
    //        }
    //        return new float4x4();
    //    }

    //    public void SetTransform(int transId)
    //    {
    //        RenderDataState<int> datastate = new RenderDataState<int>(transId, CanShow());
    //        transValue.value = datastate;
    //        if (transValue.idx != -1)
    //        {
    //            rendererBatch.SetTransformId(transValue.idx, datastate);
    //        }
    //    }

    //    public void OnReorder()
    //    {
    //        int idx = -1;
    //        transValue.idx = rendererBatch.AddTransformId(rootId, transValue.value);
    //        if (idx == -1) idx = transValue.idx;

    //        foreach (var item in float4value)
    //        {
    //            Float4Value f4v = item.Value;
    //            f4v.idx = rendererBatch.AddFloat4(item.Key, rootId, f4v.value);
    //            if (idx == -1) idx = f4v.idx;
    //            if (f4v.idx != idx)
    //            {
    //                Debug.LogError("data mismatch error!!!");
    //            }
    //        }

    //        foreach (var item in float4x4value)
    //        {
    //            Float4x4Value f4v = item.Value;
    //            f4v.idx = rendererBatch.AddFloat4x4(item.Key, rootId, f4v.value);
    //            if (idx == -1) idx = f4v.idx;
    //            if (f4v.idx != idx)
    //            {
    //                Debug.LogError("data mismatch error!!!" + item.Key);
    //            }
    //        }
    //    }
    //}


    public class HudDataSnippet
    {
        private TransformIdValue transValue = new TransformIdValue();
        private Float4x4Value param1 = new Float4x4Value();
        private Float4x4Value param2 = new Float4x4Value();

        private HudComponetBase componet;
        public HudDataSnippet(HudComponetBase _componet)
        {
            componet = _componet;
        }

        public void Init(bool show, int transId)
        {
            m_Show = show;
            m_Enable = true;
            param1.value = new RenderDataState<float4x4>(new float4x4(), CanShow());
            param2.value = new RenderDataState<float4x4>(new float4x4(), CanShow());
            transValue.value = new RenderDataState<int>(transId, CanShow());
        }

        private HudRendererBatch rendererBatch
        {
            get { return componet.rendererBatch; }
        }

        private int rootId { get { return componet.rootId; } }

        private bool m_Show = true;

        public void SetShow(bool show)
        {
            if (show == m_Show) return;
            m_Show = show;
            UpdateShowState();
        }

        private bool m_Enable = true;

        public void SetEnable(bool enable)
        {
            if (enable == m_Enable) return;
            m_Enable = enable;
            UpdateShowState();
        }

        private byte CanShow() { return (m_Show && m_Enable) ? (byte)1 : (byte)0; }

        private void UpdateShowState()
        {
            var f4x4dataState = param1.value;
            f4x4dataState.show = CanShow();
            param1.value = f4x4dataState;

            f4x4dataState = param2.value;
            f4x4dataState.show = CanShow();
            param2.value = f4x4dataState;

            var intdataState = transValue.value;
            intdataState.show = CanShow();
            transValue.value = intdataState;
            WriteData();
        }

        public void WriteData()
        {
            WriteParam1Data();
            WriteParam2Data();
            WriteTransformData();
        }

        public void WriteParamData()
        {
            WriteParam1Data();
            WriteParam2Data();
        }

        private void WriteParam1Data()
        {
            if (param1.idx == -1) return;
            rendererBatch.SetFloat4x4("_Param1", param1.idx, param1.value);
        }

        private void WriteParam2Data()
        {
            if (param2.idx == -1) return;
            rendererBatch.SetFloat4x4("_Param2", param2.idx, param2.value);
        }

        private void WriteTransformData()
        {
            if (transValue.idx == -1) return;
            rendererBatch.SetTransformId(transValue.idx, transValue.value);
        }

        public void SetColor(Color32 color)
        {
            float4x4 f4x4 = param2.value.data;
            float2 color2 = Utils.ColorToFloat(color);
            float4 c3 = f4x4.c3;
            c3.zw = color2;//param2.c3 ZW 颜色
            f4x4.c3 = c3;
            param2.value.data = f4x4;
        }

        public void SetAngle(float angle)
        {
            float4x4 f4x4 = param2.value.data;
            float4 c3 = f4x4.c3;
            c3.x = angle * Mathf.Deg2Rad;//X 负责角度
            f4x4.c3 = c3;
            param2.value.data = f4x4;
        }

        public void SetPosition(Vector2 position)
        {
            /*   C0   C1   C2   C3      第四列 (m03, m13, m23, m33) 用于存储平移/位置数据：
               | m00, m01, m02, m03 |   m03 对应 x 坐标
               | m10, m11, m12, m13 |   m13 对应 y 坐标
               | m20, m21, m22, m23 |   m23 对应 z 坐标
               | m30, m31, m32, m33 |   m33 通常为 1
            */

            float4x4 f4x4 = param1.value.data;//param1控制位置相关
            float4 c3 = f4x4.c3;
            c3.z = position.x;
            c3.w = position.y;//?
            f4x4.c3 = c3;
            param1.value.data = f4x4;
        }

        public void SetTextOrImage(bool text)
        {
            float4x4 f4x4 = param2.value.data;
            float4 c3 = f4x4.c3;
            c3.y = text ? 1 : 0;
            f4x4.c3 = c3;
            param2.value.data = f4x4;
        }
        public void ResetNineParam()
        {
            quadCount = 0;
            bounds = Rect.zero;
            float4x4 f4x4 = param2.value.data;
            ClearNineParam(ref f4x4);
            param2.value.data = f4x4;
            f4x4 = param1.value.data;
            ClearNineParam(ref f4x4);
            param1.value.data = f4x4;
        }

        private static void ClearNineParam(ref float4x4 parma)
        {
            parma.c0 = new float4();
            parma.c1 = new float4();
            float4 c2 = parma.c2;
            c2.x = 0;
            parma.c2 = c2;
        }

        private unsafe void SetValue(int paramIndex, float value)
        {
            int index = paramIndex % 16;
            if (paramIndex >= 16)
            {
                fixed (float4x4* array = &param2.value.data)
                {
                    ((float*)array)[index] = value;
                }
            }
            else
            {
                fixed (float4x4* array = &param1.value.data)
                {
                    ((float*)array)[index] = value;
                }
            }
        }

        private unsafe float GetValue(int paramIndex)
        {
            int index = paramIndex % 16;
            if (paramIndex >= 16)
            {
                fixed (float4x4* array = &param2.value.data)
                {
                    return ((float*)array)[index];
                }
            }
            else
            {
                fixed (float4x4* array = &param1.value.data)
                {
                    return ((float*)array)[index];
                }
            }
        }

        public void SetSpriteId(int index, int spriteId)
        {
            int valueindex = index / 2;
            int parity = index % 2;
            int floatIndex = 9 + valueindex;
            float fv = GetValue(floatIndex);
            float2 curfv = Utils.ToTowFloat(fv);
            curfv[parity] = spriteId + 1;
            var fvv = Utils.ToOneFloat(curfv.x, curfv.y);
            SetValue(floatIndex, fvv);
            SetValue(ref dog_param1, ref dog_param2, floatIndex, fvv);
        }



        public void SetSpritePositon(int index, float2 position)
        {
            bounds.x = Math.Min(position.x, bounds.x);
            bounds.y = Math.Min(position.y, bounds.y);
            float fv = Utils.ToOneFloat(position.x, position.y);
            int paramIndex = index;
            SetValue(paramIndex, fv);
            SetValue(ref dog_param1, ref dog_param2, paramIndex, fv);
        }

        public float2 GetSpritePosition(int index)
        {
            float fv = GetValue(index);
            return Utils.ToTowFloat(fv);
        }
        public float2 GetSpriteSize(int index)
        {
            int paramIndex = 16 + index;
            float fv = GetValue(paramIndex);
            return Utils.ToTowFloat(fv);
        }

        public void SetAlignment(float len)
        {
            float4x4 f4x4 = param2.value.data;
            float4 c3 = f4x4.c3;
            c3.y = len;
            f4x4.c3 = c3;
            param2.value.data = f4x4;
        }

        public void SetAmount(float amount, float origin, float method)
        {
            float4x4 f4x4 = param2.value.data;
            float4 c2 = f4x4.c2;
            c2.w = amount;
            c2.z = origin;
            c2.y = method;
            f4x4.c2 = c2;
            param2.value.data = f4x4;
        }

        public void SetTmpParam(float padding, float scale)
        {
            float4x4 f4x4 = param2.value.data;
            float4 c2 = f4x4.c2;
            c2.w = padding;
            c2.z = scale;
            c2.y = 1;
            f4x4.c2 = c2;
            param2.value.data = f4x4;
        }

        public void OnReorder()
        {
            int idx = -1;
            transValue.idx = rendererBatch.AddTransformId(rootId, transValue.value);
            if (idx == -1) idx = transValue.idx;

            param1.idx = rendererBatch.AddFloat4x4("_Param1", rootId, param1.value);
            if (param1.idx != idx)
            {
                Debug.LogError("data mismatch error!!!");
            }

            param2.idx = rendererBatch.AddFloat4x4("_Param2", rootId, param2.value);
            if (param2.idx != idx)
            {
                Debug.LogError("data mismatch error!!!");
            }
        }

        //-------------------------------------------------------------------------------------
        #region 狗写的代码
        private Rect bounds;
        private int quadCount = 0;
        private Float4x4Value dog_param1 = new Float4x4Value();
        private Float4x4Value dog_param2 = new Float4x4Value();
        private static unsafe void SetValue(ref Float4x4Value param1, ref Float4x4Value param2, int paramIndex, float value)
        {
            int index = paramIndex % 16;
            if (paramIndex >= 16)
            {
                fixed (float4x4* array = &param2.value.data)
                {
                    ((float*)array)[index] = value;
                }
            }
            else
            {
                fixed (float4x4* array = &param1.value.data)
                {
                    ((float*)array)[index] = value;
                }
            }
        }
        private static unsafe float GetValue(ref Float4x4Value param1, ref Float4x4Value param2, int paramIndex)
        {
            int index = paramIndex % 16;
            if (paramIndex >= 16)
            {
                fixed (float4x4* array = &param2.value.data)
                {
                    return ((float*)array)[index];
                }
            }
            else
            {
                fixed (float4x4* array = &param1.value.data)
                {
                    return ((float*)array)[index];
                }
            }
        }
        public void SetScale(Vector2 scale)
        {
            //if (scale != Vector2.one)
            {
                var new_w = bounds.width * scale.x;
                var new_x = bounds.x - (new_w - bounds.width) / 2;
                for (int index = 0; index < quadCount; index++)
                {
                    var old_quad = GetSpriteQuad(index);
                    var size = new Vector2(old_quad.Item2.x * scale.x, old_quad.Item2.y * scale.y);
                    var pos = new Vector2(new_x, old_quad.Item1.y);
                    {
                        float fv = Utils.ToOneFloat(size.x, size.y);
                        int paramIndex = 16 + index;
                        SetValue(paramIndex, fv);
                    }
                    {
                        float fv = Utils.ToOneFloat(pos.x, pos.y);
                        int paramIndex = index;
                        SetValue(paramIndex, fv);
                    }
                    new_x += size.x;
                }
            }
//             else
//             {
//                 for (int index = 0; index < quadCount; index++)
//                 {
//                     var old_quad = GetSpriteQuad(index);
//                     SetSpriteQuad(index, old_quad.Item1, old_quad.Item2);
//                 }
//             }
        }
        public void SetSpriteQuad(int index, float2 position, float2 size)
        {
            quadCount = Math.Max(index + 1, quadCount);
            bounds.x = Math.Min(position.x, bounds.x);
            bounds.y = Math.Min(position.y, bounds.y);
            bounds.width = Math.Max(position.x + size.x - bounds.x, bounds.width);
            bounds.height = Math.Max(position.y + size.y - bounds.y, bounds.height);
            {
                float fv = Utils.ToOneFloat(position.x, position.y);
                int paramIndex = index;
                SetValue(paramIndex, fv);
                SetValue(ref dog_param1, ref dog_param2, paramIndex, fv);
            }
            {
                float fv = Utils.ToOneFloat(size.x, size.y);
                int paramIndex = 16 + index;
                SetValue(paramIndex, fv);
                SetValue(ref dog_param1, ref dog_param2, paramIndex, fv);
            }
        }
        public (float2, float2) GetSpriteQuad(int index)
        {
            (float2, float2) ret;
            {
                float fv = GetValue(ref dog_param1, ref dog_param2, index);
                ret.Item1 = Utils.ToTowFloat(fv);
            }
            {
                int paramIndex = 16 + index;
                float fv = GetValue(ref dog_param1, ref dog_param2, paramIndex);
                ret.Item2 = Utils.ToTowFloat(fv);
            }
            return ret;
        }


        #endregion
        //-------------------------------------------------------------------------------------
    }
}
