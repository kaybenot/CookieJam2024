using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class ReductionPass : ScriptableRenderPass
{
    private ReductionSettings settings;
    private RenderTextureDescriptor reductionTextureDescriptor;
    private const string k_ReductionTextureName = "_ReductionTexture";
    private const string k_ReductionPassName = "ReductionRenderPass";
    
    public ReductionPass(ReductionSettings settings)
    {
        this.settings = settings;

        reductionTextureDescriptor = new RenderTextureDescriptor(Screen.width / this.settings.downsample,
            Screen.height / this.settings.downsample);
    }

    public override void RecordRenderGraph(RenderGraph renderGraph,
        ContextContainer frameData)
    {
        var resourceData = frameData.Get<UniversalResourceData>();
        var cameraData = frameData.Get<UniversalCameraData>();

        if (resourceData.isActiveTargetBackBuffer)
        {
            return;
        }
        
        reductionTextureDescriptor.width = cameraData.cameraTargetDescriptor.width / settings.downsample;
        reductionTextureDescriptor.height = cameraData.cameraTargetDescriptor.height / settings.downsample;
        reductionTextureDescriptor.depthBufferBits = 0;
        
        var srcCamColor = resourceData.activeColorTexture;
        var dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph,
            reductionTextureDescriptor, k_ReductionTextureName, false);

        if (!srcCamColor.IsValid() || !dst.IsValid())
        {
            return;
        }
        
        RenderGraphUtils.BlitMaterialParameters paraReduction = new(srcCamColor, dst, null, 0);
        renderGraph.AddBlitPass(paraReduction, k_ReductionPassName);
    }
}
