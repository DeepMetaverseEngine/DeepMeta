using System;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
namespace NFCore.Extension
{
    [BurstCompile]
    public class HudRendererBatch
    {
#if UNITY_2022_1_OR_NEWER
        public static int batchMaxCount = 8191;
#else
    public static int batchMaxCount = 1023;
#endif

        /// <summary>
        /// 拍摄绘制的相机
        /// </summary>
        private static Camera _RenderCamera;

        /// <summary>
        /// 拍摄绘制的相机
        /// </summary>
        public static Camera UICamera
        {
            get
            {
                if (_RenderCamera == null)
                {
                    _RenderCamera = Camera.main;
                }
                return _RenderCamera;
            }
            set { _RenderCamera = value; }
        }


        /// <summary>
        /// 主相机
        /// </summary>
        private static Camera _MainCamera;

        /// <summary>
        /// 主相机
        /// </summary>
        public static Camera MainCamera
        {
            get
            {
                if (_MainCamera == null)
                {
                    _MainCamera = Camera.main;
                }
                return _MainCamera;
            }
            set { _MainCamera = value; }
        }

        public static RenderPipline Pipline = RenderPipline.Buildin;
        public enum RenderPipline
        {
            Buildin,
            URP,
        }

        /// <summary>
        /// 设置GPUInstance对应的Layer
        /// </summary>
        public static int RenderLayer { get; set; } = 0;

        public static Vector3 WordCameraPosToUICameraPos(in Vector3 pos)
        {
            //将主摄像机拍摄的世界坐标 转换成屏幕坐标
            //再将屏幕坐标映射到UI相机屏幕对应的世界坐标
            if (MainCamera != null && UICamera != null)
            {
                var screenPos = MainCamera.WorldToViewportPoint(pos);
                var ret = UICamera.ViewportToWorldPoint(screenPos);

                //todo jobs替换
                //var testScreenPos = MatrixWorldToScreen(pos);
                //var testWorldPos = MatrixScreenToWorld(in testScreenPos);

                return ret;
            }
            return pos;
        }

        private static Vector3 MatrixWorldToScreen(in Vector3 worldpos)
        {
            var projectionMatrix = MainCamera.projectionMatrix;
            var viewMatrix = MainCamera.worldToCameraMatrix;
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            float4 clipSpacePos = math.mul(projectionMatrix, math.mul(viewMatrix, new float4(worldpos, 1.0f)));
            float3 ndcSpacePos = clipSpacePos.xyz / clipSpacePos.w;
            float2 screenSpacePos = (ndcSpacePos.xy + 1.0f) / 2.0f * new float2(screenWidth, screenHeight);
            var testScreenPos = new float3(screenSpacePos, clipSpacePos.z);
            testScreenPos.z += 0.2f;// HACK CODE 不加0.2 会和Camera.WorldToScreenPoint不一样
            return testScreenPos;
        }

        private static Vector3 MatrixScreenToWorld(in Vector3 screenPos)
        {
            var projectionMatrix = UICamera.projectionMatrix;
            var viewMatrix = UICamera.worldToCameraMatrix;

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            float2 screenSpacePos = new float2(screenPos.x, screenPos.y);
            float2 ndcSpacePos = (screenSpacePos / new float2(screenWidth, screenHeight)) * 2.0f - 1.0f;
            float4 clipSpacePos = new float4(ndcSpacePos, 0, 1.0f);
            float4 worldPos = math.mul(math.inverse(viewMatrix), math.mul(math.inverse(projectionMatrix), clipSpacePos));
            Vector3 ret = worldPos.xyz / worldPos.w;
            ret.z = screenPos.z - 5;// HACK CODE 不-5 会和Camera.ScreenToWorldPoint不一样
            return ret;
        }


        public static int capacity = 512;
        private TransformCollector transformCollector;
        private GPUInstanceDataCollector renderDataCollector;
        private HashSet<HudCanvasRenderer> hudCanvasRenderer;
        private Material material;
        private Mesh mesh;
        private JobHandle jobHandle;
        private int instanceCount = 0;
        public static CommandBuffer buffer { private set; get; }
        private MaterialPropertyBlock propertyBlock;


        public HudRendererBatch(Material _material, Mesh _mesh, AtlasMapping _atlasMapping, TMP_FontAsset _fontAsset)
        {
            instanceCount = 0;
            propertyBlock = new MaterialPropertyBlock();
            //material = new Material(_material);
            material = _material;
            material.enableInstancing = true;
            mesh = _mesh;
            if (_atlasMapping != null)
            {
                _atlasMapping.GenAtlasMappingInfo();
                int atlasTexId = Shader.PropertyToID("_AtlasTex");
                if (material.HasProperty(atlasTexId))
                {
                    propertyBlock.SetTexture("_AtlasTex", _atlasMapping.atlasTex);
                    propertyBlock.SetInt("_AtlasWidth", _atlasMapping.width);
                    propertyBlock.SetInt("_AtlasHeight", _atlasMapping.height);

                    propertyBlock.SetTexture("_AtlasMappingTex", _atlasMapping.atlasMappingTex);
                    propertyBlock.SetInt("_AtlasMappingWidth", _atlasMapping.atlasMappingWidth);
                    propertyBlock.SetInt("_AtlasMappingHeight", _atlasMapping.atlasMappingHeight);
                }
            }
            //             if (_fontAsset != null)
            //             {
            //                 int fontTexId = Shader.PropertyToID("_MainTex");
            //                 if (material.HasProperty(fontTexId))
            //                 {
            //                     TMP_FontAsset fontAsset = TMP_Settings.defaultFontAsset;
            //                     propertyBlock.SetTexture("_MainTex", fontAsset.atlasTexture);
            //                     propertyBlock.SetInt("_TextureWidth", fontAsset.atlasWidth);
            //                     propertyBlock.SetInt("_TextureHeight", fontAsset.atlasHeight);
            // 
            //                     propertyBlock.SetTexture("_FontMappingTex", fontAsset.fontMappingTexture);
            //                     propertyBlock.SetInt("_FontMappingWidth", fontAsset.fontMappingWidth);
            //                     propertyBlock.SetInt("_FontMappingHeight", fontAsset.fontMappingHeight);
            //                 }
            //             }
            jobHandle = new JobHandle();
            hudCanvasRenderer = new HashSet<HudCanvasRenderer>();
            transformCollector = new TransformCollector(capacity);
            renderDataCollector = new GPUInstanceDataCollector(capacity);
        }

        public void AddExpansionNotif(Action notify)
        {
            renderDataCollector.AddExpansionNotif(notify);
        }

        public void RemoveExpansionNotif(Action notify)
        {
            renderDataCollector.RemoveExpansionNotif(notify);
        }

        public int AddTransform(Transform transform, bool root)
        {
            return transformCollector.Add(transform, root);
        }

        public void RemoveTransform(int index)
        {
            transformCollector.Remove(index);
        }

        public void SetTransformEnable(int index, Transform transform)
        {
            transformCollector.SetEnable(index, transform);
        }

        public void SetTransformDisable(int index)
        {
            transformCollector.SetDisable(index);
        }

        public void SetBounds(int index, float2 center, float2 size)
        {
            transformCollector.SetBounds(index, center, size);
        }

        public int AddFloat4x4(string name, int hashcode, RenderDataState<float4x4> value)
        {
            return renderDataCollector.AddFloat4x4(name, hashcode, value);
        }

        public void SetFloat4x4(string name, int idx, RenderDataState<float4x4> value)
        {
            renderDataCollector.SetFloat4x4(name, idx, value);
        }

        public int AddTransformId(int hashcode, RenderDataState<int> transId)
        {
            return renderDataCollector.AddTransformId(hashcode, transId);
        }

        public void SetTransformId(int idx, RenderDataState<int> transId)
        {
            renderDataCollector.SetTransformId(idx, transId);
        }

        public void RemoveData(int hashcode)
        {
            renderDataCollector.Remove(hashcode);
        }

        public void TriggerReorder(HudCanvasRenderer canvasRenderer)
        {
            if (hudCanvasRenderer.Contains(canvasRenderer)) return;
            hudCanvasRenderer.Add(canvasRenderer);
        }

        public void OnLateUpdate()
        {
            if (!jobHandle.IsCompleted)
            {
                jobHandle.Complete();
            }
            instanceCount = renderDataCollector.instanceCount;
            int renderCount = instanceCount / batchMaxCount;
            int lastrenderCount = instanceCount % batchMaxCount;
            int index = 0;
            if (renderDataCollector.expansion)
            {
                renderDataCollector.expansion = false;
                renderDataCollector.TriggerNotif();
            }
            buffer?.Clear();
            for (int i = 0; i < renderCount; i++)
            {
                Profiler.BeginSample("DrawMeshInstanced " + i);
                Matrix4x4[] matrix4x4 = renderDataCollector.GetObjectToWorld(index);
                renderDataCollector.FullPropertyBlack(index, propertyBlock);
                DrawMeshInstanced(mesh, material, matrix4x4, batchMaxCount, propertyBlock);
                Profiler.EndSample();
                index++;
            }
            {
                if (lastrenderCount > 0)
                {
                    Matrix4x4[] matrix4x4 = renderDataCollector.GetObjectToWorld(index);
                    Profiler.BeginSample("DrawMeshInstanced Last");
                    renderDataCollector.FullPropertyBlack(index, propertyBlock);
                    DrawMeshInstanced(mesh, material, matrix4x4, lastrenderCount, propertyBlock);
                    Profiler.EndSample();
                }
            }
        }


        public void DrawMeshInstanced(Mesh mesh, Material material, Matrix4x4[] matrix4x4, int count, MaterialPropertyBlock properties)
        {
            if (Pipline == RenderPipline.URP)
            {
                RenderWithURP(mesh, material, matrix4x4, count, properties);
            }
            else
            {
                RenderWithBuiltin(matrix4x4, count, properties);
            }
            //SRC CODE
            //RenderWithBuiltin(matrix4x4,count, properties);

            //指定绘制的LAYER和摄像机
            //Graphics.DrawMeshInstanced(mesh, 0, material, matrix4x4, count, properties, ShadowCastingMode.Off, false, RenderLayer, RenderCamera);

        }

        private void RenderWithBuiltin(Matrix4x4[] matrix4x4, int count, MaterialPropertyBlock properties)
        {
            var camera = Camera.main;
            if (MainCamera != null && UICamera != null)
            {
                camera = UICamera;
            }
            //Builtin管线下需要这样调用
            Graphics.DrawMeshInstanced(mesh, 0, material, matrix4x4, count, properties, ShadowCastingMode.Off, false, RenderLayer, camera);
        }

        private void RenderWithURP(Mesh mesh, Material material, Matrix4x4[] matrix4x4, int count, MaterialPropertyBlock properties)
        {
            //URP管线下需要这样调用
            if (buffer == null)
            {
                buffer = new CommandBuffer();
                buffer.name = "HudRendererBatch";
                //RenderCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, buffer);//这是builtin的调法 
            }
            //CMDBUFFER 无视LAYER限制
            buffer.DrawMeshInstanced(mesh, 0, material, 0, matrix4x4, count, properties);
        }


        public unsafe void OnUpdate()
        {

            if (jobHandle.IsCompleted)
            {
                Profiler.BeginSample("OnReorder");
                if (hudCanvasRenderer.Count > 0)
                {
                    foreach (var item in hudCanvasRenderer)
                    {
                        item.OnReorder();
                    }
                    hudCanvasRenderer.Clear();
                }
                Profiler.EndSample();

                Profiler.BeginSample("ToJob");
                JobHandle transformJobHandle = transformCollector.ToJob(HudRendererCenter.cameravp, HudRendererCenter.forward);
                jobHandle = renderDataCollector.ToJob(transformCollector, transformJobHandle);
                Profiler.EndSample();
            }
        }

        public void OnDestroy()
        {
            if (!jobHandle.IsCompleted) jobHandle.Complete();
            transformCollector.Dispose();
            renderDataCollector.Dispose();

            if (buffer != null)
            {
                buffer.Release();
                buffer = null;
            }
        }
    }
}
