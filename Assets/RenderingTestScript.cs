using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderingTestScript : MonoBehaviour
{
    [SerializeField] public RenderTexture textureSource, textureDest;
    [SerializeField] public List<Material> sampleMaterial;
    [SerializeField] public CommandBuffer buffer;
    // [SerializeField] public CustomRenderTexture renderTexture;

    public int[] temporaryIds;

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

    void OnEndContextRendering(ScriptableRenderContext context, Camera[] arg2)
    {   
        Debug.LogWarning("Called frame thing");
        buffer = new CommandBuffer();
        buffer.name = "Test";

        // Setting up ids
        temporaryIds = new int[sampleMaterial.Count+1];
        RenderTargetIdentifier[] tempRenderTextures = new RenderTargetIdentifier[temporaryIds.Length];

        for (int i = 0; i < temporaryIds.Length; i++)
        {
            temporaryIds[i] = Shader.PropertyToID("_customBuffer"+i.ToString());   
            tempRenderTextures[i] = new RenderTargetIdentifier(temporaryIds[i]);
            buffer.GetTemporaryRT(temporaryIds[i], textureSource.descriptor);
        }

        // Retrieve from source
        buffer.CopyTexture(textureSource, tempRenderTextures[0]);
        

        // Doing materials
        for (int i = 0; i < sampleMaterial.Count; i++)
        {
            buffer.Blit(tempRenderTextures[i], tempRenderTextures[i+1], sampleMaterial[i]);
        }
        

        // Bring result to target and execute
        buffer.CopyTexture(tempRenderTextures[tempRenderTextures.Length-1], new RenderTargetIdentifier(textureDest));
        Graphics.ExecuteCommandBuffer(buffer);

        // Releasing Data
        for (int i = 0; i < temporaryIds.Length; i++) { buffer.ReleaseTemporaryRT(temporaryIds[i]); }
        buffer.Release();
    }

    void OnDestroy()
    {
        RenderPipelineManager.endFrameRendering -= OnEndContextRendering;
    }
}
