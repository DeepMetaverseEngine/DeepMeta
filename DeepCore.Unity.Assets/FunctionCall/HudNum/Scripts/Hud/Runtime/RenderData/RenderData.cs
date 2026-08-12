using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public struct RenderData
{
    public int transId;
    public float4 color;
    public float4 posAndSize;
}

[BurstCompile]
public struct RenderDataParam
{
    public int spriteId;
    public float4 spritePosAndSize;
}

[BurstCompile]
public struct DataPackage<T> where T : struct
{
    public byte miss;
    public T Data;
}

[BurstCompile]
public struct TransformData
{
    public float4x4 localToWorld;
    public float2 boundCenter;
    public float2 boundSize;
    public byte culling;
    public byte disable;
    public byte root;
    public float zvalue;
}

public struct TransformSort
{
    public int transId;
    public float weights;
}

[BurstCompile]
public struct RenderDataState<T> where T : struct
{
    public T data;
    public byte show;

    public RenderDataState(T _data, byte _state)
    {
        data = _data;
        show = _state;
    }
}
