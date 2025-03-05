using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ParticleLimiter : MonoBehaviour
{
    [SerializeField] private int ticks = 5;

    [SerializeField] public int maxParticleCount {get; private set;}
    
    [SerializeField] private GameObject[] particleList;
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient sliderGradient;
    [SerializeField] private Image[] sliderGraphics;
    [SerializeField] private Text sliderText;

    // Start is called before the first frame update
    void Awake()
    {
        maxParticleCount = PlayerPrefs.GetInt("particle_limit", 60);
        slider.value = maxParticleCount;   
    }

    // Update is called once per frame
    void Update()
    {
        // if(QualityControl.main.Index == 0){maxParticleCount = 15;}
        // else {maxParticleCount = 40;}
        
        // if(maxParticleCount <= 1){
        //     maxParticleCount = 1;
        // }

        maxParticleCount = (int)slider.value;
        PlayerPrefs.SetInt("particle_limit", maxParticleCount);

        float ratio = maxParticleCount/60f; 
        // Debug.Log(ratio);
        // Debug.Log("max count: " + maxParticleCount);

        foreach(Image image in sliderGraphics){
            image.color = sliderGradient.Evaluate(ratio);
        }
        sliderText.color = sliderGradient.Evaluate(ratio);
        sliderText.text = maxParticleCount.ToString();

        

        if(ticks > 0){

            ticks--;
            return;
        }
        else{
            particleList = GameObject.FindGameObjectsWithTag("particle");
            if(particleList.Length > 0 && particleList.Length > maxParticleCount){
                
                int count = particleList.Length;
                for (int i = 0; count > maxParticleCount; i++){
                    particleList[i].SetActive(false);
                    count --;
                }
            
            }
            ticks = 5;
        }
    }

    public void SetMaxParticleCount(int max){
        maxParticleCount = max;
    }
}
