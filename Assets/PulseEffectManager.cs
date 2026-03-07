using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PulseEffectManager : MonoBehaviour
{
    
    public List<PulseEffectInstance> effectPool;
    // public List<CommandBuffer> commandBuffers;
    public Camera renderTexCam;
    public Material basePulseMaterial;
    public List<Material> pulseMaterials;
    public RenderTexture baseTexture, resultTexture;
    public RenderingTestScript mainRenderer;
    [SerializeField] private int startingLength = 1;
    float time;
    public static PulseEffectManager instance {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}

        effectPool = new();
        pulseMaterials = new();

        for (int i = 0; i < startingLength; i++)
        {
            pulseMaterials.Add(new Material(basePulseMaterial));

            PulseEffectInstance newInstance = gameObject.AddComponent<PulseEffectInstance>();
            newInstance.mainMaterial = pulseMaterials[i];
            effectPool.Add(newInstance);

        }
    }

    public void AddEffect(Vector2 worldCoordinates, float strength = 0.05f, float expandRate = 1f, float widthFactor = 0.1f)
    {
        PulseEffectInstance targetInstance = null;
        for (int i = 0; i < effectPool.Count; i++)
        {
            if(!effectPool[i].active){targetInstance = effectPool[i]; break;}
        }
        
        if(targetInstance == null)
        {
            float highestElapsed = effectPool[0].time;
            int winningIndex = 0;
            for (int i = 0; i < effectPool.Count; i++)
            {
                if(effectPool[i].time > highestElapsed){highestElapsed = effectPool[i].time; winningIndex = i;}
            }

            targetInstance = effectPool[winningIndex];
        }

        targetInstance.Feed(worldCoordinates, strength, expandRate, widthFactor);
    }

    void Update()
    {
        renderTexCam.orthographicSize = Camera.main.orthographicSize;
        renderTexCam.backgroundColor = Camera.main.backgroundColor;
        renderTexCam.ResetAspect();
        // Only for testing
        // time += Time.deltaTime;
        // float distancea = 5;
        // if(time > 0.3f)
        // {
        //     AddEffect(new Vector2(Random.Range(0, distancea), Random.Range(0, distancea)));
        //     time = 0f;
        // }
    }
}
