using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace NFCore.Extension
{
    public class HudDrawMeshInstancedFeature : ScriptableRendererFeature
    {
        private HudDrawMeshInstancedPass _Pass;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            _Pass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
            renderer.EnqueuePass(_Pass);
        }

        public override void Create()
        {
            _Pass = new HudDrawMeshInstancedPass();
        }
    }
}
