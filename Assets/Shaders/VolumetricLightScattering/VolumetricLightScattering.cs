using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[System.Serializable]
public class VolumetricLightScatteringSettings
{
    [Header("Properties")]
    [Range(0.1f, 1f)]
    public float resolutionScale = 0.5f;

    [Range(0.0f, 1.0f)]
    public float intensity = 1.0f;

    [Range(0.0f, 1.0f)]
    public float blurWidth = 0.85f;
}

public class VolumetricLightScattering : ScriptableRendererFeature
{
    class LightScatteringPass : ScriptableRenderPass
    {
        private RenderTargetIdentifier cameraColorTargetIdent;
        private FilteringSettings filteringSettings =new FilteringSettings(RenderQueueRange.opaque);
        private readonly List<ShaderTagId> shaderTagIdList = new List<ShaderTagId>();
        private readonly RenderTargetHandle occluders = RenderTargetHandle.CameraTarget;
        private readonly float resolutionScale;
        private readonly float intensity;
        private readonly float blurWidth;
        private readonly Material occludersMaterial;
        private readonly Material radialBlurMaterial;


        public LightScatteringPass(VolumetricLightScatteringSettings settings)
        {
            occluders.Init("_OccludersMap");
            resolutionScale = settings.resolutionScale;
            intensity = settings.intensity;
            blurWidth = settings.blurWidth;

            occludersMaterial = new Material(Shader.Find("Hidden/RW/UnlitColor"));
            radialBlurMaterial = new Material(Shader.Find("Hidden/RW/RadialBlur"));

            shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            shaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
            shaderTagIdList.Add(new ShaderTagId("LightweightForward"));
            shaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));

        }
        public void SetCameraColorTarget(RenderTargetIdentifier cameraColorTargetIdent)
        {
            this.cameraColorTargetIdent = cameraColorTargetIdent;
        }


        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // 1
            RenderTextureDescriptor cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            // 2
            cameraTextureDescriptor.depthBufferBits = 0;
            // 3
            cameraTextureDescriptor.width = Mathf.RoundToInt(cameraTextureDescriptor.width * resolutionScale);
            cameraTextureDescriptor.height = Mathf.RoundToInt(cameraTextureDescriptor.height * resolutionScale);
            // 4
            cmd.GetTemporaryRT(occluders.id, cameraTextureDescriptor, FilterMode.Bilinear);
            // 5
            ConfigureTarget(occluders.Identifier());
        }


        public override void Execute(ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            // 1
            if (!occludersMaterial || !radialBlurMaterial)
            {
                return;
            }

            // 2
            CommandBuffer cmd = CommandBufferPool.Get();

            // 3
            using (new ProfilingScope(cmd,new ProfilingSampler("VolumetricLightScattering")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                Camera camera = renderingData.cameraData.camera;
                context.DrawSkybox(camera);

                // 1
                DrawingSettings drawSettings = CreateDrawingSettings(shaderTagIdList,ref renderingData, SortingCriteria.CommonOpaque);
                // 2
                drawSettings.overrideMaterial = occludersMaterial;

                context.DrawRenderers(renderingData.cullResults,ref drawSettings, ref filteringSettings);

                // 1
                Vector3 sunDirectionWorldSpace = RenderSettings.sun.transform.forward;
                // 2
                Vector3 cameraPositionWorldSpace = camera.transform.position;
                // 3
                Vector3 sunPositionWorldSpace = cameraPositionWorldSpace + sunDirectionWorldSpace;
                // 4
                Vector3 sunPositionViewportSpace = camera.WorldToViewportPoint(sunPositionWorldSpace);

                radialBlurMaterial.SetVector("_Center", new Vector4(sunPositionViewportSpace.x, sunPositionViewportSpace.y, 0, 0));
                radialBlurMaterial.SetFloat("_Intensity", intensity);
                radialBlurMaterial.SetFloat("_BlurWidth", blurWidth);

                Blit(cmd, occluders.Identifier(), cameraColorTargetIdent,radialBlurMaterial);

            }

            // 4
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }


        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(occluders.id);
        }

    }
    public VolumetricLightScatteringSettings settings = new VolumetricLightScatteringSettings();

    LightScatteringPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new LightScatteringPass(settings);
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }


    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
        m_ScriptablePass.SetCameraColorTarget(renderer.cameraColorTarget);

    }
}


