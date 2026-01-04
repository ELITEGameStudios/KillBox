using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MasterAudioSystem;

[System.Serializable]
public class AudioTrack
{
    public string name;
    public bool crossfading;
    public float timer, crossfadeTime;
    public bool isPlaying => primaryActiveAudioInstance.isPlaying || secondaryActiveAudioInstance.isPlaying;

    [System.Serializable]
    public class AudioInstance
    {
        public AudioInstance(SoundClip sound, AudioSource source)
        {
            this.sound = sound;
            this.source = source;
            Init();
        }

        public SoundClip sound;
        public AudioSource source;
        public bool isPlaying;

        public virtual void Init()
        {
            source.clip = sound.clip;
            source.loop = sound.loop;
        }

        public virtual void Update()
        {
            source.clip = sound.clip;
        }

        public void Play()
        {
            source.Play();
            isPlaying = true;
        }

        public void Stop()
        {
            source.Stop();
            isPlaying = false;
        }
    }



    [Range(0,1)]
    public float volume = 1;
    private List<AudioSource> sources;
    public AudioProfile profile; // filter (idk if this is the actual class)
    public AudioInstance[] activeAudioInstances;
    public AudioInstance primaryActiveAudioInstance;
    public AudioInstance secondaryActiveAudioInstance;
    public AudioInstance currentActiveAudioInstance;
    public AudioInstance otherActiveAudioInstance => currentActiveAudioInstance == primaryActiveAudioInstance ? secondaryActiveAudioInstance : primaryActiveAudioInstance;


    AudioSource GetAvailableSource()
    {
        // for (int i = 0; i < sources.Count; i++)
        // {
        //     foreach (AudioInstance audioInstance in activeAudioInstances)
        //     {
        //         if(audioInstance.source == sources[i]){continue;}
        //         return audioInstance.source;
        //     }
        // }
     
        AudioSource newSource = MasterAudioSystem.instance.gameObject.AddComponent<AudioSource>();
        sources.Add(newSource);
        return newSource;
    }

    public void OnAwake()
    {
        sources = new List<AudioSource>{};
        activeAudioInstances = new AudioInstance[]{primaryActiveAudioInstance, secondaryActiveAudioInstance};

        primaryActiveAudioInstance.source = GetAvailableSource();
        secondaryActiveAudioInstance.source = GetAvailableSource();

        // primaryActiveAudioInstance.Init();
        // secondaryActiveAudioInstance.Init();
    }


    public void OnUpdate()
    {
        foreach (AudioInstance instance in activeAudioInstances){
            instance.source.volume = MasterAudioSystem.instance.masterVolume * volume * instance.sound.volume;
        }

        if (crossfading)
        {
            if(timer > 0)
            {
                currentActiveAudioInstance.source.volume = MasterAudioSystem.instance.masterVolume * volume * currentActiveAudioInstance.sound.volume * timer/crossfadeTime;
                otherActiveAudioInstance.source.volume = MasterAudioSystem.instance.masterVolume * volume * currentActiveAudioInstance.sound.volume * (1 - timer/crossfadeTime);

                timer-=Time.deltaTime;
            }
            else
            {
                crossfading = false;
                currentActiveAudioInstance.source.volume = MasterAudioSystem.instance.masterVolume * volume * currentActiveAudioInstance.sound.volume;
                otherActiveAudioInstance.Stop();
            }
        }
    }

    void SwapCurrent()
    {
        currentActiveAudioInstance = currentActiveAudioInstance == primaryActiveAudioInstance ? secondaryActiveAudioInstance : primaryActiveAudioInstance;
    }

    public void PlayClip(SoundClip newClip, float crossfadeTime = 0)
    {
        
        if(crossfadeTime > 0)
        {
            SwapCurrent();
            currentActiveAudioInstance.Stop();
            currentActiveAudioInstance.sound = newClip;
            currentActiveAudioInstance.Init();
            currentActiveAudioInstance.Play();    
            
            this.crossfadeTime = crossfadeTime;
            // for (int i = 0; i < activeAudioInstances.Length; i++)
            // {
            // }
        }
        else
        {
            StopCurrentAudio();
            currentActiveAudioInstance = primaryActiveAudioInstance;
            currentActiveAudioInstance.sound = newClip;
            currentActiveAudioInstance.Init();
            currentActiveAudioInstance.Play();
        }
        
    }

    public void StopCurrentAudio()
    {
        foreach (AudioInstance instance in activeAudioInstances)
        {
            instance.source.Stop();
        }
    }


}
