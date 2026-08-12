using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Profiling;

public unsafe class TransformCollector : IDisposable
{
    private TransformAccessArray transformArray;
    public NativeList<TransformData> transformData;
    public NativeList<TransformSort> transformSort;
    private NativeQueue<ushort> remaining;
    private int readIndex;
    private bool dispose;
    private int mCapacity;

    public TransformCollector(int capacity)
    {
        mCapacity = capacity;
        dispose = false;
        transformArray = new TransformAccessArray(capacity);
        remaining = new NativeQueue<ushort>(Allocator.Persistent);
        transformData = new NativeList<TransformData>(capacity, Allocator.Persistent);
        transformSort = new NativeList<TransformSort>(capacity, Allocator.Persistent);
        readIndex = 0;
    }

    public int count
    {
        get { return readIndex; }
    }

    private void TryExpansion()
    {
        if (readIndex < transformData.Length) return;
        int len = transformData.Length + mCapacity;
        transformArray.capacity = len;
        transformData.Capacity = len;
    }

    public int Add(Transform transform, bool root)
    {
        if (dispose) return 0;
        if (remaining.Count > 0)
        {
            int remainingindex = remaining.Dequeue();
            transformArray[remainingindex] = transform;
            TransformData data = new TransformData();
            data.root = root ? (byte)1 : (byte)0;
            transformData[remainingindex] = data;
            return remainingindex;
        }
        else
        {
            TryExpansion();
            int index = readIndex;
            transformArray.Add(transform);
            TransformData data = new TransformData();
            data.root = root ? (byte)1 : (byte)0;
            transformData.Add(data);
            readIndex++;
            return index;
        }
    }

    public void Remove(int index)
    {
        if (dispose) return;
        if (index >= transformData.Length) return;
        transformArray[index] = null;
        TransformData data = transformData[index];
        data.disable = 1;
        transformData[index] = data;
        remaining.Enqueue((ushort)index);
    }

    public void SetEnable(int index, Transform transform)
    {
        if (dispose) return;
        if (index >= transformData.Length) return;
        transformArray[index] = transform;
        TransformData data = transformData[index];
        data.disable = 0;
        transformData[index] = data;
    }

    public void SetDisable(int index)
    {
        if (dispose) return;
        if (index >= transformData.Length) return;
        transformArray[index] = null;
        TransformData data = transformData[index];
        data.disable = 1;
        transformData[index] = data;
    }

    public void SetBounds(int index, float2 center, float2 size)
    {
        if (dispose) return;
        if (index >= transformData.Length) return;
        transformArray[index] = null;
        TransformData data = transformData[index];
        data.boundCenter = center;
        data.boundSize = size;
        transformData[index] = data;
    }

    public JobHandle ToJob(float4x4 vpMatrix, float3 forward)
    {
        if (dispose) return new JobHandle();
        Profiler.BeginSample("TransformCollector");
        LocalToWorldJob job = new LocalToWorldJob();
        job.transformData = transformData;
        job.vpMatrix = vpMatrix;
        job.forward = forward;
        JobHandle localToWorldJobHandle = job.ScheduleReadOnly(transformArray, 32);
        TransformSortJob sortJob = new TransformSortJob();
        sortJob.transformSort = transformSort;
        sortJob.transformdataList = transformData;
        JobHandle jobHandle = sortJob.Schedule(localToWorldJobHandle);
        Profiler.EndSample();
        return jobHandle;
    }

    public void Dispose()
    {
        dispose = true;
        remaining.Dispose();
        transformData.Dispose();
        transformArray.Dispose();
        transformSort.Dispose();
    }
}
