using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace NFCore.Extension
{

    public struct GPUInstanceData<T, U> : IDisposable where T : unmanaged where U : unmanaged
    {
        public ArrayPtr<T, U> renderData;
        public NativeParallelMultiHashMap<int, RenderDataState<T>> renderdataHashMap;
        private bool dispose;

        public GPUInstanceData(int _capacity)
        {
            dispose = false;
            renderData = new ArrayPtr<T, U>(_capacity);
            renderdataHashMap = new NativeParallelMultiHashMap<int, RenderDataState<T>>(_capacity, Allocator.Persistent);
        }

        public U[] GetArray(int index)
        {
            return renderData.arrays[index];
        }

        public unsafe int Add(int hashCode, RenderDataState<T> value)
        {
            if (dispose) return -1;
            int idx = 0;
            int lastcapacity = renderdataHashMap.Capacity;
            //renderdataHashMap.Add(hashCode, value, out idx);//这个API米有了
            //--Test
            renderdataHashMap.Add(hashCode, value);
            renderdataHashMap.TryGetFirstValue(hashCode, out var rds, out var it);
            idx = it.GetEntryIndex();
            //--

            int curcapacity = renderdataHashMap.Capacity;
            if (curcapacity != lastcapacity)
            {
                renderData.Resize(curcapacity);
            }
            return idx;
        }

        public void Remove(int hashCode)
        {
            if (dispose) return;

            renderdataHashMap.Remove(hashCode);
        }

        public unsafe void Set(int idx, RenderDataState<T> value)
        {
            if (dispose) return;
            var bucketdata = renderdataHashMap.GetUnsafeBucketData();
            UnsafeUtility.WriteArrayElement(bucketdata.values, idx, value);
        }

        public void Dispose()
        {
            renderData.Dispose();
            renderdataHashMap.Dispose();
            //idxtoindex.Dispose();
            dispose = true;
        }
    }
}
