
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Code from cyan, modified by me :)

public class MultiBlit : ScriptableRendererFeature
{
    public class MultiBlitPass : ScriptableRenderPass
    {
        public Material blitMaterial;

        private MultiBlitSettings settings;

        private RenderTargetHandle m_TemporaryColorTexture;

        private RenderTargetHandle m_DestinationTexture;

        private string m_ProfilerTag;

        public FilterMode filterMode { get; set; }

        private RenderTargetIdentifier source { get; set; }

        private RenderTargetIdentifier destination { get; set; }

        public MultiBlitPass(RenderPassEvent renderPassEvent, MultiBlitSettings settings, string tag)
        {
            base.renderPassEvent = renderPassEvent;
            this.settings = settings;
            blitMaterial = settings.blitMaterial;
            m_ProfilerTag = tag;
            m_TemporaryColorTexture.Init("_TemporaryColorTexture");
            if (settings.dstType == Target.TextureID)
            {
                m_DestinationTexture.Init(settings.dstTextureId);
            }
        }

        public void Setup(ScriptableRenderer renderer)
        {
            if (settings.requireDepthNormals)
            {
                ConfigureInput(ScriptableRenderPassInput.Normal);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get(m_ProfilerTag);
            RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cameraTargetDescriptor.depthBufferBits = 0;
            ScriptableRenderer renderer = renderingData.cameraData.renderer;
            if (settings.srcType == Target.CameraColor)
            {
                source = renderer.cameraColorTarget;
            }
            else if (settings.srcType == Target.TextureID)
            {
                source = new RenderTargetIdentifier(settings.srcTextureId);
            }
            else if (settings.srcType == Target.RenderTextureObject)
            {
                source = new RenderTargetIdentifier(settings.srcTextureObject);
            }

            if (settings.dstType == Target.CameraColor)
            {
                destination = renderer.cameraColorTarget;
            }
            else if (settings.dstType == Target.TextureID)
            {
                destination = new RenderTargetIdentifier(settings.dstTextureId);
            }
            else if (settings.dstType == Target.RenderTextureObject)
            {
                destination = new RenderTargetIdentifier(settings.dstTextureObject);
            }

            if (settings.setInverseViewMatrix)
            {
                Shader.SetGlobalMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
            }

            if (settings.dstType == Target.TextureID)
            {
                if (settings.overrideGraphicsFormat)
                {
                    cameraTargetDescriptor.graphicsFormat = settings.graphicsFormat;
                }

                commandBuffer.GetTemporaryRT(m_DestinationTexture.id, cameraTargetDescriptor, filterMode);
            }

            if (source == destination || (settings.srcType == settings.dstType && settings.srcType == Target.CameraColor))
            {
                commandBuffer.GetTemporaryRT(m_TemporaryColorTexture.id, cameraTargetDescriptor, filterMode);
                Blit(commandBuffer, source, m_TemporaryColorTexture.Identifier(), blitMaterial, settings.blitMaterialPassIndex);
                Blit(commandBuffer, m_TemporaryColorTexture.Identifier(), destination);
            }
            else
            {
                Blit(commandBuffer, source, destination, blitMaterial, settings.blitMaterialPassIndex);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (settings.dstType == Target.TextureID)
            {
                cmd.ReleaseTemporaryRT(m_DestinationTexture.id);
            }

            if (source == destination || (settings.srcType == settings.dstType && settings.srcType == Target.CameraColor))
            {
                cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
            }
        }
    }

    [Serializable]
    public class MultiBlitSettings
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

        public Material blitMaterial;

        public int blitMaterialPassIndex;

        public bool setInverseViewMatrix;

        public bool requireDepthNormals;

        public Target srcType;

        public string srcTextureId = "_CameraColorTexture";

        public RenderTexture srcTextureObject;

        public Target dstType;

        public string dstTextureId = "_BlitPassTexture";

        public RenderTexture dstTextureObject;

        public bool overrideGraphicsFormat;

        public GraphicsFormat graphicsFormat;

        public bool canShowInSceneView = true;
    }

    public enum Target
    {
        CameraColor,
        TextureID,
        RenderTextureObject
    }

    public MultiBlitSettings settings = new MultiBlitSettings();

    public MultiBlitPass blitPass;

    public override void Create()
    {
        int max = ((!(settings.blitMaterial != null)) ? 1 : (settings.blitMaterial.passCount - 1));
        settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, max);
        blitPass = new MultiBlitPass(settings.Event, settings, base.name);
        if (settings.graphicsFormat == GraphicsFormat.None)
        {
            settings.graphicsFormat = SystemInfo.GetGraphicsFormat(DefaultFormat.LDR);
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!renderingData.cameraData.isPreviewCamera && (settings.canShowInSceneView || !renderingData.cameraData.isSceneViewCamera))
        {
            if (settings.blitMaterial == null)
            {
                Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            }
            else
            {
                blitPass.Setup(renderer);
                renderer.EnqueuePass(blitPass);
            }
        }
    }
}
#if false // Decompilation log
'305' items in cache
------------------
Resolve: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'D:\UnityEditors\2022.3.62f3\Editor\Data\NetStandard\ref\2.1.0\netstandard.dll'
------------------
Resolve: 'Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'c:\Users\User\Github\KillBox\Library\ScriptAssemblies\Unity.RenderPipelines.Universal.Runtime.dll'
------------------
Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\UnityEditors\2022.3.62f3\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll'
------------------
Resolve: 'Unity.RenderPipelines.Core.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Unity.RenderPipelines.Core.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'c:\Users\User\Github\KillBox\Library\ScriptAssemblies\Unity.RenderPipelines.Core.Runtime.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '2.1.0.0', Got: '4.1.2.0'
Load from: 'D:\UnityEditors\2022.3.62f3\Editor\Data\NetStandard\compat\2.1.0\shims\netstandard\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'System.Runtime.CompilerServices.Unsafe, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
#endif
