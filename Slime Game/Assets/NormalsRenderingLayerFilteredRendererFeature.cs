using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

// A custom renderer feature for Unity 6 using RenderGraph to capture depth and normals to a global texture.

public class NormalsRenderingLayerFilteredRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material overrideMaterial; // The material to use for rendering
        public LayerMask layerMask;
        public RenderingLayerMask renderingLayerMask; // The rendering layer mask to filter objects
        public RenderQueueRange renderQueueRange = RenderQueueRange.opaque;
        public RenderPassEvent passEvent;
    }

    public Settings settings = new Settings();
    RenderingLayerRenderPass renderLayerRenderPass;

    public override void Create()
    {
        renderLayerRenderPass = new RenderingLayerRenderPass(settings.overrideMaterial, settings.renderingLayerMask, settings.layerMask, settings.renderQueueRange);
        renderLayerRenderPass.renderPassEvent = settings.passEvent; // Adjust as needed
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView)
        {
            renderer.EnqueuePass(renderLayerRenderPass);
        }
    }

    class RenderingLayerRenderPass : ScriptableRenderPass
    {
        private Material overrideMaterial;
        private LayerMask layerMask;
        private RenderingLayerMask renderingLayerMask;
        private RenderQueueRange renderQueueRange;
        private ShaderTagId shaderTag;

        public RenderingLayerRenderPass(Material material, RenderingLayerMask renderingLayerMask, LayerMask layerMask, RenderQueueRange queueRange)
        {
            this.overrideMaterial = material;
            this.renderingLayerMask = renderingLayerMask;
            this.layerMask = layerMask;
            this.renderQueueRange = queueRange;

            shaderTag = new ShaderTagId("UniversalForward");
        }

        private class PassData
        {
            internal Material material;
            internal RendererListHandle rendererListHandle;
            internal ShaderTagId shaderTagId;
            internal LayerMask layerMask;
            internal RenderingLayerMask renderingLayerMask;
            internal RenderQueueRange renderQueueRange;
        }

        // Create the custom data class that contains the new texture
        public class CustomNormalTextureData : ContextItem
        {
            public TextureHandle newTextureForFrameData;

            public override void Reset()
            {
                newTextureForFrameData = TextureHandle.nullHandle;
            }
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameContext)
        {
            UniversalResourceData resourceData = frameContext.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                return;
            }

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("____NORMAL____LayerFilteredObjects", out var passData))
            {

                // Get the data needed to create the list of objects to draw
                UniversalRenderingData renderingData = frameContext.Get<UniversalRenderingData>();
                UniversalCameraData cameraData = frameContext.Get<UniversalCameraData>();
                UniversalLightData lightData = frameContext.Get<UniversalLightData>();
                SortingCriteria sortFlags = cameraData.defaultOpaqueSortFlags;
                RenderQueueRange renderQueueRange = RenderQueueRange.opaque;
                FilteringSettings filterSettings = new FilteringSettings(renderQueueRange, layerMask, renderingLayerMask);

                // Redraw only objects that have their LightMode tag set to UniversalForward 
                ShaderTagId shadersToOverride = new ShaderTagId("UniversalForward");

                // Create drawing settings
                DrawingSettings drawSettings = RenderingUtils.CreateDrawingSettings(shadersToOverride, renderingData, cameraData, lightData, sortFlags);
                // Add the override material to the drawing settings
                drawSettings.overrideMaterial = overrideMaterial;

                // Create the list of objects to draw
                var rendererListParameters = new RendererListParams(renderingData.cullResults, drawSettings, filterSettings);

                // Convert the list to a list handle that the render graph system can use
                passData.rendererListHandle = renderGraph.CreateRendererList(rendererListParameters);

                RenderTextureDescriptor textureDescriptor = new RenderTextureDescriptor(cameraData.cameraTargetDescriptor.width, cameraData.cameraTargetDescriptor.height, RenderTextureFormat.ARGBFloat, 0);
                textureDescriptor.msaaSamples = 1;
                TextureHandle texture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, textureDescriptor, "_CustomNormals", false);
                
                CustomNormalTextureData customData = frameContext.GetOrCreate<CustomNormalTextureData>();
                customData.newTextureForFrameData = texture;

                // create a custom depth texture with no MSAA for this pass so that MSAA can work.
                RenderTextureDescriptor depthTextureDescriptor = new RenderTextureDescriptor(cameraData.cameraTargetDescriptor.width, cameraData.cameraTargetDescriptor.height, RenderTextureFormat.Depth, 32);
                depthTextureDescriptor.msaaSamples = 1;
                TextureHandle depthTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, depthTextureDescriptor, "_CustomNormalsDepth", true);

                // Set the render target as the color and depth textures of the active camera texture
                builder.UseRendererList(passData.rendererListHandle);
                
                builder.SetRenderAttachment(texture, 0, AccessFlags.ReadWrite);
                
                // by not setting a depth we use the one we already have, but we have issues on MSAA that aren't easily solved.
                // builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture, AccessFlags.Read);
                builder.AllowPassCulling(false);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
                builder.SetGlobalTextureAfterPass(texture, Shader.PropertyToID("_CustomNormalsGlobal"));
            }

        }

        static void ExecutePass(PassData data, RasterGraphContext context)
        {
            // Clear the render target to black (clears everything from screen, so don't do this if overlaying)
            // context.cmd.ClearRenderTarget(true, true, Color.black);
            // Draw the objects in the list
            context.cmd.DrawRendererList(data.rendererListHandle);
        }

    }
}
