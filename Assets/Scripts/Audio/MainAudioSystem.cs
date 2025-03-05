using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainAudioSystem : MonoBehaviour
{
    public static MainAudioSystem main {get; private set;}

    [SerializeField]
    private AudioFader source;

    [SerializeField]
    private AudioClip[] main_loop_playlist, boss_playlist, boss_ambience;

    private float clip_length, time_played;

    [SerializeField]
    private AudioMixerGroup action, rest, hurt, ambience, silence;

    [SerializeField]
    public int main_loop_playlist_index {get; private set;}
    
    [SerializeField]
    private float transition_length, ambient_transition_length;

    [SerializeField]
    public AudioClip current_song {get; private set;}
    public bool looping {get; private set;}
    public bool exterior_track {get; private set;}

    void Awake(){
        if (MainAudioSystem.main == null){
            main = this;
        }
        else{
            Destroy(this);
        }

        main.transition_length = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        main_loop_playlist_index = Random.Range(0, 5);
        PlayMainLoop(false);
    }

    public void PlayMainLoop(bool next = true){

        if(next){ UpdateIndex(); }
        current_song = main_loop_playlist[main_loop_playlist_index];

        clip_length = current_song.length;
        time_played = 0;

        source.Play(current_song, transition_length);
        exterior_track = false;
    }
    
    void UpdateIndex(){
        // int old_index = main_loop_playlist_index;
        main_loop_playlist_index++;
        if(main_loop_playlist_index == main_loop_playlist.Length){
            main_loop_playlist_index = 0;
        }
        // main_loop_playlist_index = Random.Range(1, main_loop_playlist.Length);
        // if(main_loop_playlist_index == main_loop_playlist_index){
        //     main_loop_playlist_index--;
        // }
    }
    
    public void PlayBossAmbience(int ambienceIndex, bool _looping = false){

        if(ambienceIndex < 5) { current_song = boss_ambience[ambienceIndex]; }
        else{ current_song = boss_ambience[1]; }

        clip_length = current_song.length;
        time_played = 0;

        source.Play(current_song, ambient_transition_length);
        looping = _looping;
        exterior_track = true;
    }

    public void PlaySong(AudioClip song, bool _looping = false){

        current_song = song;

        clip_length = current_song.length;
        time_played = 0;

        source.Play(current_song, transition_length);
        looping = _looping;
        exterior_track = true;
    }
    public void ReplaySong(){

        time_played = 0;
        source.Play(current_song, transition_length);
    }

    public void Rest(){
        source.ChangeMixerGroup(rest);
    }

    //public void Hurt(){
    //    source.ChangeMixerGroup(hurt);
    //}

    public void Action(){
        source.ChangeMixerGroup(action);
    }

    public void Ambience(){
        source.ChangeMixerGroup(ambience);
    }

    public void Silence(){
        VolumeControl.main.SetSilentSnapshot(true, 1);
    }

    void Update()
    {
        time_played += Time.deltaTime;

        if(!source.active_source.isPlaying){//(time_played >= clip_length - transition_length){
            if(looping){
                ReplaySong();
            }
            else{
                PlayMainLoop();
            }
        }
    }

    public void TriggerBossMusic(int bossRound){

        AudioClip target_clip = boss_playlist[1];

        if(bossRound < 5){
             target_clip = boss_playlist[bossRound];
             looping = true;
        }

        // if(!exterior_track){
        PlaySong(target_clip, true);
        // }


        // switch(boss_name){
        //     case "SHARD":
        //         target_clip = boss_playlist[0]; 
        //         break;
        //     case "ALPHA TRIAD":
        //         target_clip = boss_playlist[1]; 
        //         break;
        //     case "CUTTER":
        //         target_clip = boss_playlist[2]; 
        //         break;
        // }

        // if(!exterior_track){
        //     PlaySong(target_clip, true);
        // }
    }

    public float transition_length_accessor{
        get{
            return transition_length;
        }
        set{
            transition_length = value;
        }
    }

}
