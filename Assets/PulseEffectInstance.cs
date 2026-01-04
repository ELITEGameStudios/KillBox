using System;
using System.Collections;
using System.Collections.Generic;
using Cyan;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PulseEffectInstance : MonoBehaviour
{

    [SerializeField] Vector2 coordinates;
    [SerializeField] float strength;
    [SerializeField] float rate;
    [SerializeField] float width;
    public float time;
    [SerializeField] float targetTime;
    [SerializeField] float targetDistance;
    
    public bool active;
    public Material mainMaterial;



    void Awake()
    {
        
        // RenderPipelineManager.endContextRendering += OnEndContextRendering;
    }

    // private void OnEndContextRendering(ScriptableRenderContext context, List<Camera> list)
    // {
    //     context.
    //     throw new NotImplementedException();
    // }

    public void Feed(Vector2 worldCoordinates, float strength = 0.05f, float expandRate = 1f, float widthFactor = 0.1f, float targetTime = 5)
    {
        coordinates = Camera.main.WorldToScreenPoint(worldCoordinates);
        coordinates.x /= Camera.main.pixelWidth;
        coordinates.y /= Camera.main.pixelHeight;

        this.strength = strength;
        rate = expandRate;
        width = widthFactor;
        this.targetTime = targetTime;


        // mainMaterial = new Material(PulseEffectManager.instance.pulseShader);
        time = 0;
        targetDistance = 0;
        active = true;
        PulseEffectManager.instance.mainRenderer.AddMaterial(mainMaterial);

        // commandBuffer = new CommandBuffer();
        // commandBuffer.name = "PulseDistortionBuffer";
        // commandBuffer.Blit(PulseEffectManager.instance.baseTexture, new RenderTargetIdentifier(BuiltinRenderTextureType.CurrentActive), mainMaterial);
    }

    // void OnRenderImage(RenderTexture source, RenderTexture destination)
    // {
    //     Camera.main.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, commandBuffer);
        
    // }

    void Update()
    {
        if(time >= targetTime){End();}

        if (active)
        {
            mainMaterial.SetFloat("_strength", strength);
            mainMaterial.SetFloat("_TargetDist", targetDistance);
            mainMaterial.SetFloat("_distRange", width);
            mainMaterial.SetVector("_LocalCoords", (Vector2)coordinates);


            time += Time.deltaTime;
            targetDistance = time * rate;
        }
        else
        {
            mainMaterial.SetFloat("_strength", 0);
        }

    }

    public void End()
    {
        active = false;
        mainMaterial.SetFloat("_strength", 0);
        PulseEffectManager.instance.mainRenderer.RemoveMaterial(mainMaterial);
        // PulseEffectManager.instance.effectPool.Remove(this);
        // // Camera.main.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, commandBuffer);
        // // Destroy(this);
    }
}
