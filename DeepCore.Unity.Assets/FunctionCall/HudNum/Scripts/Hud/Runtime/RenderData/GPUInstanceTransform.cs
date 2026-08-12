using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace NFCore.Extension
{
    public unsafe struct GPUInstanceTransform : IDisposable
    {
        public ArrayPtr<float4x4, Matrix4x4> renderData;
        public NativeParallelMultiHashMap<int, RenderDataState<int>> renderdataHashMap;
        private HashSet<Action> expansionNotif;
        private bool dispose;

        public GPUInstanceTransform(int _capacity)
        {
            dispose = false;
            expansion = false;
            expansionNotif = new HashSet<Action>();
            renderData = new ArrayPtr<float4x4, Matrix4x4>(_capacity);
            renderdataHashMap = new NativeParallelMultiHashMap<int, RenderDataState<int>>(_capacity, Allocator.Persistent);
        }

        public int count
        {
            get { return renderdataHashMap.Count(); }
        }

        public bool expansion
        {
            get;
            set;
        }

        public unsafe int Add(int hashCode, RenderDataState<int> value)
        {
            if (dispose) return -1;
            int idx = 0;
            int lastcapacity = renderdataHashMap.Capacity;
            //renderdataHashMap.Add(hashCode, value, out idx);//这个API木有了
            renderdataHashMap.Add(hashCode, value);
            renderdataHashMap.TryGetFirstValue(hashCode, out var rds, out var it);
            idx = it.GetEntryIndex();

            int curcapacity = renderdataHashMap.Capacity;
            if (curcapacity != lastcapacity)
            {
                renderData.Resize(curcapacity);
                expansion = true;
            }
            return idx;
        }

        public void Remove(int hashCode)
        {
            if (dispose) return;
            renderdataHashMap.Remove(hashCode);
        }

        public unsafe void Set(int idx, RenderDataState<int> value)
        {
            if (dispose) return;
            var bucketdata = renderdataHashMap.GetUnsafeBucketData();
            UnsafeUtility.WriteArrayElement(bucketdata.values, idx, value);
        }

        public void TriggerNotif()
        {
            foreach (var notif in expansionNotif)
            {
                notif?.Invoke();
            }
        }

        public void AddExpansionNotif(Action notify)
        {
            expansionNotif.Add(notify);
        }

        public void RemoveExpansionNotif(Action notify)
        {
            expansionNotif.Remove(notify);
        }

        public void Dispose()
        {
            dispose = true;
            renderData.Dispose();
            renderdataHashMap.Dispose();
            expansionNotif.Clear();
        }
    }
}
