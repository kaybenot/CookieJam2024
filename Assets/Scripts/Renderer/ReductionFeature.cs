using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class ReductionSettings
{
    [Range(0, 5)] public int downsample = 3;
}

public class ReductionFeature : ScriptableRendererFeature
{
    [SerializeField] private ReductionSettings settings;

    private ReductionPass pass;
    
    public override void Create()
    {
        pass = new ReductionPass(settings);
        pass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (pass == null)
        {
            return;
        }
        
        renderer.EnqueuePass(pass);
    }
}
