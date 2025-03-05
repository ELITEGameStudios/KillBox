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
        
        manager = GameObject.Find("Manager").GetComponent<VolumeControl>();
        if(coefficient == 0){
            coefficient = 1;
        }
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
            source.volume = manager.VolumeSlider.value * coefficient / 100;
            volume = manager.VolumeSlider.value * coefficient / 100;
        }
    }
}
