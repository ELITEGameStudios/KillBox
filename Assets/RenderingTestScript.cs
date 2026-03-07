using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderingTestScript : MonoBehaviour
{
    [SerializeField] public RenderTexture textureSource, textureDest;

    [SerializeField] public RenderTexture[] rescaleTextures;
    [SerializeField] public List<Material> sampleMaterial;
    [SerializeField] public CommandBuffer buffer;
    [SerializeField] public bool doubleBuffered;
    // [SerializeField] public CustomRenderTexture renderTexture;

    public int[] temporaryIds;
    public int[] doubleBufferIds;

    void Awake()
    {
        if(sampleMaterial == null)
        {
            sampleMaterial = new();
        }
    }


    void Start()
    {
        RenderPipelineManager.endFrameRendering += OnEndContextRendering;
        RenderPipelineManager.beginFrameRendering += OnBeginContextRendering;
    }

    void Update()
    {
        
    }

    public void AddMaterial(Material material, bool front = false)
    {
        if(front){sampleMaterial.Insert(0, material);}
        else { sampleMaterial.Add(material); }
    }

    public void RemoveMaterial(Material material)
    {
        if (sampleMaterial.Contains(material))
        {        
            sampleMaterial.Remove(material);
        }
    }

    private void OnBeginContextRendering(ScriptableRenderContext context, Camera[] arg2)
    {
        // if(context.)
        
        
    }

    void OnEndContextRendering(ScriptableRenderContext context, Camera[] arg2)
    {   

        // Debug.Log(textureSource.descriptor.width + " " + textureSource.descriptor.height);
        buffer = new CommandBuffer();
        buffer.name = "Test";
        // buffer.

        // Setting up ids
        temporaryIds = new int[sampleMaterial.Count+1];
        // doubleBufferIds = new int[2];

        RenderTargetIdentifier[] tempRenderTextures = new RenderTargetIdentifier[temporaryIds.Length];
        // RenderTargetIdentifier[] doubleRenderTextures = new RenderTargetIdentifier[doubleBufferIds.Length];

        for (int i = 0; i < temporaryIds.Length; i++)
        {
            temporaryIds[i] = Shader.PropertyToID("_customBuffer"+i.ToString());   
            tempRenderTextures[i] = new RenderTargetIdentifier(temporaryIds[i]);
            buffer.GetTemporaryRT(temporaryIds[i], textureSource.descriptor);
        }

        // for(int i = 0; i < doubleBufferIds.Length; i++)
        // {   
        //     doubleBufferIds[i] = Shader.PropertyToID("_doubleBuffer"+i.ToString());   
        //     // doubleRenderTextures[i] = new RenderTargetIdentifier(doubleBufferIds[i]);
        // }
        // RenderTargetIdentifier primary = new RenderTargetIdentifier(Shader.PropertyToID("_doubleBuffer"+0.ToString()));
        // RenderTargetIdentifier secondary = new RenderTargetIdentifier(Shader.PropertyToID("_doubleBuffer"+1.ToString()));
        
        // buffer.GetTemporaryRT(doubleBufferIds[0], textureSource.descriptor);
        // buffer.GetTemporaryRT(doubleBufferIds[1], textureSource.descriptor);

        // Retrieve from source
        // if (doubleBuffered)
        // {
        //     buffer.CopyTexture(textureSource, doubleRenderTextures[0]);
        // }
        // else
        // {
            buffer.CopyTexture(textureSource, tempRenderTextures[0]);   
        // }
        
        RenderTargetIdentifier lastTarget = tempRenderTextures[0]; 

        // Doing materials
        for (int i = 0; i < sampleMaterial.Count; i++)
        {
            // break;
            // if(doubleBuffered)
            // {
            //     RenderTargetIdentifier target = i % 2 == 0 ? secondary : primary;    
            //     RenderTargetIdentifier source = i % 2 == 0 ? primary : secondary;

            //     buffer.Blit(source, target, sampleMaterial[i]);
            //     lastTarget = target;
            // }
            // else
            // {
                buffer.Blit(tempRenderTextures[i], tempRenderTextures[i+1], sampleMaterial[i]);
                lastTarget = tempRenderTextures[i+1];
            // }
        }
        

        // Bring result to target and execute
        buffer.CopyTexture(lastTarget, new RenderTargetIdentifier(textureDest));
        // if(doubleBuffered)
        // {
        //     RenderTargetIdentifier target = (tempRenderTextures.Length-1) % 2 == 0 ? secondary : primary;    
        //     buffer.CopyTexture(target, new RenderTargetIdentifier(textureDest)); 
        //     Debug.Log("DoubleBufferTest");  
        // }
        // else
        // {
        //     buffer.CopyTexture(tempRenderTextures[tempRenderTextures.Length-1], new RenderTargetIdentifier(textureDest));
        // }
        Graphics.ExecuteCommandBuffer(buffer);

        // Releasing Data
        for (int i = 0; i < temporaryIds.Length; i++) { buffer.ReleaseTemporaryRT(temporaryIds[i]); }
        for (int i = 0; i < doubleBufferIds.Length; i++) { buffer.ReleaseTemporaryRT(doubleBufferIds[i]); }
        buffer.Release();
        for (int i = 0; i < rescaleTextures.Length; i++)
        {
            RenderTextureDescriptor descriptor = rescaleTextures[i].descriptor;
            descriptor.width = Screen.width;
            descriptor.height = Screen.height;
            rescaleTextures[i].Release();
            rescaleTextures[i] = new RenderTexture(descriptor);
            rescaleTextures[i].Create();
        }

    }

    void OnDestroy()
    {
        RenderPipelineManager.endFrameRendering -= OnEndContextRendering;
    }
}
