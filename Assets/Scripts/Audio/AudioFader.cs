using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFader : MonoBehaviour
{
    [SerializeField]
    private AudioSource source1, source2;

    public AudioSource active_source {get; private set;}
    public AudioSource other_source {get; private set;}
    public AudioClip clip {get; private set;}

    public bool is_fading {get; private set;}
    
    [SerializeField]
    private float fade_timer, rate, receding_volume;

    void Awake(){
        active_source = source2;
    }

    public void ChangeMixerGroup(AudioMixerGroup group){

        active_source.outputAudioMixerGroup = group;
        other_source.outputAudioMixerGroup = group;

    }

    public void Play(AudioClip clip, float time = 0.5f){
        if(active_source == source1){

            active_source = source2;
            other_source = source1;

            receding_volume = source1.volume;
        }
        else if(active_source == source2){ //otherwise active_source == source2
            active_source = source1;
            other_source = source2;

            receding_volume = source2.volume;
        }

        active_source.clip = clip;

        fade_timer = time;
        rate = 1/fade_timer;
        is_fading = true;

        active_source.Play();


    }

    void Update(){
        if(is_fading){


            active_source.volume += rate * Time.unscaledDeltaTime * (VolumeControl.main.music_value / 100);
            other_source.volume -= receding_volume * rate * Time.unscaledDeltaTime *(VolumeControl.main.music_value / 100);

            if(fade_timer <= 0){

                other_source.volume = 0 *  (VolumeControl.main.music_value / 100);
                active_source.volume = 1 *  (VolumeControl.main.music_value / 100);
                is_fading = false;
            }

            fade_timer -= Time.unscaledDeltaTime;
        }
        else{
            CheckVolume();
        }
        
    }

    public void CheckVolume(){
        other_source.volume = 0 *  (VolumeControl.main.music_value / 100);
        active_source.volume = 1 *  (VolumeControl.main.music_value / 100);
    }
}
