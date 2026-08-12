using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

namespace NFCore.Extension
{
    public unsafe class GPUInstanceDataCollector : IDisposable
    {
        private GPUInstanceTransform gpuInstanceTransform;
        private Dictionary<string, GPUInstanceData<float4x4, Matrix4x4>> float4x4Values;
        private NativeArray<JobHandle> jobhandles;
        private int mCapacity;
        private int* mInstanceCount;
        private bool mDispose;

        public GPUInstanceDataCollector(int _capacity)
        {
            mInstanceCount = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), UnsafeUtility.AlignOf<int>(), Allocator.Persistent);
            *mInstanceCount = 0;
            mDispose = false;
            gpuInstanceTransform = new GPUInstanceTransform(_capacity);
            float4x4Values = new Dictionary<string, GPUInstanceData<float4x4, Matrix4x4>>();
            jobhandles = new NativeArray<JobHandle>(8, Allocator.Persistent); ;
            mCapacity = _capacity;
        }

        public int instanceCount
        {
            get { return *mInstanceCount; }
        }

        public bool expansion
        {
            get { return gpuInstanceTransform.expansion; }
            set { gpuInstanceTransform.expansion = value; }
        }

        public void FullPropertyBlack(int index, MaterialPropertyBlock propertyBlock)
        {
            foreach (var item in float4x4Values)
            {
                propertyBlock.SetMatrixArray(item.Key, item.Value.GetArray(index));
            }
        }

        public Matrix4x4[] GetObjectToWorld(int index)
        {
            return gpuInstanceTransform.renderData.arrays[index];
        }

        private GPUInstanceData<float4x4, Matrix4x4> TryGetFloat4x4(string name)
        {
            GPUInstanceData<float4x4, Matrix4x4> value;
            if (!float4x4Values.TryGetValue(name, out value))
            {
                value = new GPUInstanceData<float4x4, Matrix4x4>(mCapacity);
                float4x4Values[name] = value;
            }
            return value;
        }

        public int AddFloat4x4(string name, int hashcode, RenderDataState<float4x4> value)
        {
            if (mDispose) return -1;
            var vectordata = TryGetFloat4x4(name);
            return vectordata.Add(hashcode, value);
        }

        public void SetFloat4x4(string name, int idx, RenderDataState<float4x4> value)
        {
            if (mDispose) return;
            var vectordata = TryGetFloat4x4(name);
            vectordata.Set(idx, value);
        }

        public int AddTransformId(int hashcode, RenderDataState<int> transformId)
        {
            if (mDispose) return -1;
            return gpuInstanceTransform.Add(hashcode, transformId);
        }

        public void SetTransformId(int idx, RenderDataState<int> transformId)
        {
            if (mDispose) return;
            gpuInstanceTransform.Set(idx, transformId);
        }

        public void AddExpansionNotif(Action notify)
        {
            gpuInstanceTransform.AddExpansionNotif(notify);
        }

        public void RemoveExpansionNotif(Action notify)
        {
            gpuInstanceTransform.RemoveExpansionNotif(notify);
        }

        public void TriggerNotif()
        {
            gpuInstanceTransform.TriggerNotif();
        }

        public void Remove(int hashcode)
        {
            if (mDispose) return;
            gpuInstanceTransform.Remove(hashcode);
            foreach (var value in float4x4Values)
            {
                value.Value.Remove(hashcode);
            }
        }

        public unsafe JobHandle ToJob(TransformCollector transformCollector, JobHandle dependsOn)
        {
            int jobHandleCount = 0;
            Profiler.BeginSample("RenderDataCollectorJob");
            var float4x4enumerator = float4x4Values.GetEnumerator();
            while (float4x4enumerator.MoveNext())
            {
                var value = float4x4enumerator.Current.Value;
                FillGPUInstanceFloat4x4Job job = new FillGPUInstanceFloat4x4Job();
                job.renderDataPtr = value.renderData.arrayPtrs;
                job.renderdataHashMap = value.renderdataHashMap;
                job.transformdata = transformCollector.transformData;
                job.transformSort = transformCollector.transformSort;
                job.batchMaxCount = HudRendererBatch.batchMaxCount;
                JobHandle jobHandle = job.Schedule(dependsOn);
                jobhandles[jobHandleCount] = jobHandle;
                jobHandleCount++;
            }
            {
                FillGPUInstanceTransformJob job = new FillGPUInstanceTransformJob();
                job.renderDataPtr = gpuInstanceTransform.renderData.arrayPtrs;
                job.renderdataHashMap = gpuInstanceTransform.renderdataHashMap;
                job.len = mInstanceCount;
                job.transformdata = transformCollector.transformData;
                job.transformSort = transformCollector.transformSort;
                job.batchMaxCount = HudRendererBatch.batchMaxCount;
                JobHandle jobHandle = job.Schedule(dependsOn);
                jobhandles[jobHandleCount] = jobHandle;
                jobHandleCount++;
            }
            Profiler.EndSample();
            if (jobHandleCount == 0) return dependsOn;
            Profiler.BeginSample("CombineDependencies");
            Profiler.BeginSample("NativeSlice");
            var nativeSlice = new NativeSlice<JobHandle>(jobhandles, 0, jobHandleCount);
            Profiler.EndSample();
            JobHandle jobhadle = JobHandle.CombineDependencies(nativeSlice);
            Profiler.EndSample();
            return jobhadle;
        }

        public void Dispose()
        {
            UnsafeUtility.Free(mInstanceCount, Allocator.Persistent);
            foreach (var value in float4x4Values)
            {
                value.Value.Dispose();
            }
            float4x4Values.Clear();
            jobhandles.Dispose();
            gpuInstanceTransform.Dispose();
            mDispose = true;
        }
    }
}