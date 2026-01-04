using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProfile : MonoBehaviour
{

    [SerializeField] private AudioSource source;
    [SerializeField]
    private bool SFX;
    public bool in_transition = false;
    private VolumeControl manager;

    public float volume, coefficient;

    // Start is called before the first frame update
    void Awake()
    {
        if(source == null){
            source = gameObject.GetComponent<AudioSource>();
        }
        
        if(coefficient == 0){
            coefficient = 1;
        }

        if(MainAudioSystem.main == null) {enabled = false;}
        else{ 
            manager = MainAudioSystem.main.GetVolumeControl(); 
            if (SFX && !in_transition)
            {
                source.volume = manager.SFXSlider.value * coefficient / 100;
                volume = manager.SFXSlider.value * coefficient / 100;
            }
        };
    }

    void Start(){
    }

    // Update is called once per frame
    void Update()
    {

        if (SFX && !in_transition)
        {
            source.volume = manager.SFXSlider.value * coefficient / 100;
            volume = manager.SFXSlider.value * coefficient / 100;
        }
        else if(!in_transition)
        {
            // source.volume = manager.VolumeSlider.value * coefficient / 100;
            // volume = manager.VolumeSlider.value * coefficient / 100;
        }
    }
}
