using System.Collections.Generic;
using UnityEngine;
public class MasterAudioSystem : MonoBehaviour
{
    public static MasterAudioSystem instance {get; private set;}
    public float masterVolume;
    public List<AudioTrack> tracks;
    public List<SoundClip> menusMusic;
    public List<SoundClip> mainMusicLibrary;
    public List<PhaseClipSet> earlyPhaseSets;
    public PhaseClipSet currentPhaseSet;

    [SerializeField] private AudioTrack musicTrack, menuTrack;

    [System.Serializable]
    public struct SoundClip
    {
        public AudioClip clip;
        public float volume;
        public bool loop;
        // public float pitch;
    }

    [System.Serializable]
    public struct PhaseClipSet
    {
        public SoundClip[] rounds;
        public float volume;
        // public float pitch;
    }

    void Awake()
    {
        if (instance == null){instance = this;}
        else if(instance != this){Destroy(this);}

        if(tracks == null){ tracks = new List<AudioTrack>(); }
        
        // else {
        //     tracks = new List<AudioTrack>();
        // }
        

        foreach (AudioTrack track in tracks){
            track.OnAwake();
        }
    }

    public void Start()
    {
        PlayMenuMusic();  
        musicTrack = GetTrack("Music");    
        menuTrack = GetTrack("Music/Menu");  
    }


    void Update()
    {
        foreach (AudioTrack track in tracks){
            track.OnUpdate();
        }
    }

    public void OnPreNextRound()
    {

        // musicTrack.PlayClip(currentPhaseSet.rounds[GameManager.main.GetCurrentRoundInPhase()], 1);
        // if(GameManager.main.LvlCount%4 == 0){QueueSet();}   
    }

    public AudioTrack GetTrack(string name)
    {
        foreach (AudioTrack track in tracks)
        {
            if(track.name == name)
            {
                return track;
            }
        }

        return null;
    }

    public void PlayMenuMusic()
    {
        AudioTrack thisTrack = menuTrack;
        if(thisTrack != null)
        {
            List<SoundClip> candadites = menusMusic;  
            for (int i = candadites.Count-1; i >= 0; i--)
            {
                if(thisTrack.currentActiveAudioInstance.sound.clip == candadites[i].clip)
                {
                    candadites.RemoveAt(i);
                }
            } 
            thisTrack.PlayClip(candadites[Random.Range(0, candadites.Count)], 1);
        }
    }

    public void QueueLibrary()
    {
        currentPhaseSet = earlyPhaseSets[Random.Range(0, earlyPhaseSets.Count)];
    }

    public void QueueSet()
    {
        currentPhaseSet = earlyPhaseSets[Random.Range(0, earlyPhaseSets.Count)];
    }

    public void SetTrackVolume(float volume, params string[] trackNames)
    {
        foreach (AudioTrack track in tracks)
        {
            for (int i = 0; i < trackNames.Length; i++)
            {
                if(track.name == trackNames[i])
                {
                    track.volume = volume;
                }
            }
        }
    }
}