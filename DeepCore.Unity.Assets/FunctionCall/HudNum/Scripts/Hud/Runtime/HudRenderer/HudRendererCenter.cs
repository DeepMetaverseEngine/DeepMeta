using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace NFCore.Extension
{
    [BurstCompile]
    public class HudRendererCenter
    {
        private static Dictionary<int, HudRendererBatch> renderCollector = new Dictionary<int, HudRendererBatch>();

        private static Camera camera;
        private static Transform cameraTransform;

        /// <summary>
        /// math.mul(camera.projectionMatrix, camera.worldToCameraMatrix) 是将投影矩阵 P 与视图矩阵 V 相乘，得到一个组合矩阵 PV。
        /// 这个组合矩阵将直接把世界坐标系中的点转换为裁剪空间坐标。
        /// </summary>
        public static float4x4 cameravp
        {
            get
            {
                if (camera == null) camera = HudRendererBatch.UICamera;
                if (camera == null) return Matrix4x4.identity;
                float4x4 vp = math.mul(camera.projectionMatrix, camera.worldToCameraMatrix);
                return vp;
            }
        }

        public static float3 forward
        {
            get
            {
                if (cameraTransform == null) cameraTransform= camera.transform;
                return cameraTransform.forward;
            }
        }

        public static HudRendererBatch GetRendererBatch(Material _material, Mesh _mesh, AtlasMapping _atlasMapping, TMP_FontAsset fontAsset)
        {
            int hashCode = GetHashCode(_material, _mesh, _atlasMapping);
            if (hashCode == 0) return null;
            HudRendererBatch collector;
            if (!renderCollector.TryGetValue(hashCode, out collector))
            {
                collector = new HudRendererBatch(_material, _mesh, _atlasMapping, fontAsset);
                renderCollector[hashCode] = collector;
            }
            return collector;
        }

        public static int GetHashCode(Material _material, Mesh _mesh, AtlasMapping _atlasMapping)
        {
            if (_material == null || _mesh == null) return 0;
            int hashCode = _material.GetHashCode() ^ _mesh.GetHashCode();
            if (_atlasMapping != null)
            {
                hashCode = hashCode ^ _atlasMapping.GetHashCode();
            }
            return hashCode;
        }

        public static void Update()
        {
            var enumerator = renderCollector.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.OnUpdate();
            }
        }

        public static void LateUpdate()
        {
            var enumerator = renderCollector.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.OnLateUpdate();
            }
        }

        public static void Destory()
        {
            var enumerator = renderCollector.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.OnDestroy();
            }
            renderCollector.Clear();
        }
    }
}
