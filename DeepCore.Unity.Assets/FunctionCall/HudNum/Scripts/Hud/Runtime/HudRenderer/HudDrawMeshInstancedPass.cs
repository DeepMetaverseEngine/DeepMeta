using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace NFCore.Extension
{
    public class HudDrawMeshInstancedPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var buffer = HudRendererBatch.buffer;
            if(buffer != null)
                context.ExecuteCommandBuffer(buffer);
        }
    }
}
