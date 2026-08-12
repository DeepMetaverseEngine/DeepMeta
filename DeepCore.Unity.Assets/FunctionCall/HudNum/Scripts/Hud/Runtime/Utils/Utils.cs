using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace NFCore.Jobs
{


    public class Utils
    {
        public static NativeArray<T> ArrayExpansion<T>(NativeArray<T> array, int size, Allocator allocator) where T : unmanaged
        {

            int len = array.Length;
            array.Dispose();
            NativeArray<T> newarray = CollectionHelper.CreateNativeArray<T>(size, allocator, NativeArrayOptions.UninitializedMemory);
            return newarray;
        }

        public static float ToOneFloat(float v1, float v2)
        {
            uint uv1 = math.f32tof16(v1);
            uint uv2 = math.f32tof16(v2);
            uint v = uv1 << 16 | uv2;
            return math.asfloat(v);
        }

        public static float2 ToTowFloat(float fv)
        {
            uint uv = math.asuint(fv);
            uint uv1 = uv >> 16;
            uint uv2 = uv & 0x0000ffff;
            return new float2(math.f16tof32(uv1), math.f16tof32(uv2));
        }

        public static float2 ColorToFloat(Color color)
        {
            float2 f2 = new float2();
            f2.x = ToOneFloat(color.r, color.g);
            f2.y = ToOneFloat(color.b, color.a);
            return f2;
        }
    }
}
