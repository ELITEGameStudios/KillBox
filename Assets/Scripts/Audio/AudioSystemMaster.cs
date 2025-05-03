using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioSystemMaster : MonoBehaviour
{
    public static AudioSystemMaster main {get; private set;}

    public void AddToMaster(AudioSystemMaster challenger){
        if (main == null){
            main = challenger;
        }
        else{
            Destroy(this);
        }
    }



    //[SerializeField]
    //private AudioSource source;

    [SerializeField]
    private AudioFader source;

    public Song current_song {get; private set;}

    public List<Song> local_songs {get; private set;} = new List<Song>(); 

    [SerializeField]
    private float timer = 0;
    
    [SerializeField]
    private int measures_time = 1, loop_max_times = 1, loop_count = 1; // MEASURES TIME MUST BE RESET TO 1, NOT 0

    [SerializeField]
    private int target_measures_time = 2, current_mode; 

    [SerializeField]
    private float target_mps = 2f, target_bpm = 120;

    [SerializeField]
    private List<SongClip> queue = new List<SongClip>();

    [SerializeField]
    private int[] clip_number; // 1 = pre, 2 = in_action, 3 = post, 0 = ambient  

    [SerializeField]
    private int[] max_clip_number;

    [SerializeField]
    private float gap = 0;

    public bool ready_to_switch {get; private set;} 
    public bool is_switching {get; private set;} 
    public bool song_change {get; private set;} 
    public bool ending_song {get; private set;} 

    private SongClip active_clip;

    void Awake(){
        clip_number = new int[]{0, 0, 0, 0};
        AddToMaster(this);
    }
    public void Begin(){


        main.current_song = SongLibrary.beam;

        main.max_clip_number = main.current_song.max_clips;

        main.target_bpm = main.current_song.BPM;
        main.target_mps = main.current_song.MPS;

        Debug.Log(main.target_mps);
        

        main.timer = main.target_mps;
        main.timer = 0;

        PlayIntro();
        
        
        PlayQueue();
    }


    // Update is called once per frame
    void Update(){

        if(main.timer < 0){
            gap = -timer;
        }

        if (main.timer <= 0){
            main.measures_time++;

            if(main.measures_time == main.target_measures_time - 1 && !main.ready_to_switch){
                main.ready_to_switch = true;

                //if(loop_count == loop_max_times){
                //    }
                //    else{
                //        
                //    }
                //}
            }

            if(main.song_change){
                main.target_bpm = main.current_song.BPM;
                main.target_mps = main.current_song.MPS;

                main.song_change = false;
            }

            if(main.ready_to_switch){
                //play audio in main.queue
                PlayQueue();
            }


            main.timer = main.target_mps;
            
            if(main.measures_time == main.target_measures_time - 2 && !main.ready_to_switch){
                PlayViaMode();
            }

            timer -= gap;
        }
        
        main.timer -= Time.unscaledDeltaTime;

        if(queue.Count > 1){
            queue.RemoveAt(0);
        }
        if(queue.Count > 0 && !ready_to_switch){
            ready_to_switch = true;
        }
  

        //Debug.Log(main.timer);      
        //Debug.Log(main.measures_time);      
        //Debug.Log(main.target_bpm);      
    }

    public void SetSong(Song song){
        main.current_song = song;
        main.max_clip_number = song.max_clips;
        main.song_change = true;

        PlayIntro();
    }

    void PlayQueue(){

        main.active_clip = main.queue[0];
        
        main.target_measures_time = main.active_clip.intended_length;
        main.queue.RemoveAt(0);

        //main.source.clip = 
        
        main.source.Play(main.active_clip.clip, target_mps);
        

        measures_time = 0;
        main.timer = main.target_mps;
        main.timer -= Time.unscaledDeltaTime;

        main.ready_to_switch = false;
    }

    public Song GetSongByKey(string key){
        foreach (Song song in main.local_songs)
        {
            if (song.name == key){
                return song;
            }
        }

        return null;
    }

    public void AddSong(Song song, bool play = false){
        main.local_songs.Add(song);
        if(play){
            SetSong(song);
        }
    }

    void PlayViaMode(){
        switch (main.current_mode){
            case 0:
                PlayAmbient(true);
                break;
            case 1:
                PlayPreRound(true);
                break;
            case 2:
                PlayAction(true);
                break;
            case 3:
                PlayPostAction(true);
                break;
            case 4:
                PlayPreRound(true);
                break;
            case 5:
                PlayAmbient(true);
                break;

        }
    }

    public void PlayIntro(){        
        main.queue.Add(main.current_song._in_transition);
        main.current_mode = 4;
    }
    public void PlayAmbient(bool automated = false){
        if(ending_song){
            EndSong();
            return;
        }

        
        if(main.active_clip.fade_to != -1 && automated){
            main.queue.Add(main.current_song._ambience[main.active_clip.fade_to]);
        }

        else{
            if(main.current_mode != 0 && main.active_clip.to_ambient_id != -1){

                main.queue.Add(main.current_song._ambience[main.active_clip.to_ambient_id]);
                main.clip_number[0] = main.active_clip.to_ambient_id;
            }
            else{
                main.queue.Add(main.current_song._ambience[main.clip_number[0]]);
            }

        }

        main.current_mode = 0;
        UpdateClipNumber(0, automated);
    }
    
    public void PlayPreRound(bool automated = false){
        if(ending_song){
            EndSong();
            return;
        }
        
        if(main.active_clip.fade_to != -1 && automated){
            main.queue.Add(main.current_song._pre_round[main.active_clip.fade_to]);
        }
        else{
            if(main.current_mode != 1 && main.active_clip.to_pre_id != -1){

                main.queue.Add(main.current_song._pre_round[main.active_clip.to_pre_id]);
                main.clip_number[1] = main.active_clip.to_pre_id;
            }
            else{
                main.queue.Add(main.current_song._pre_round[main.clip_number[1]]);
            }
        }

        main.current_mode = 1;
        UpdateClipNumber(1, automated);
    }

    public void PlayAction(bool automated = false){
        
        if(ending_song){
            EndSong();
            return;
        }

        if(main.active_clip.fade_to != -1 && automated){
            main.queue.Add(main.current_song._action[main.active_clip.fade_to]);
        }
        else{
            if(main.current_mode != 2 && main.active_clip.to_action_id != -1){

                main.queue.Add(main.current_song._action[main.active_clip.to_action_id]);
                main.clip_number[2] = main.active_clip.to_action_id;
            }
            else{
                main.queue.Add(main.current_song._action[main.clip_number[2]]);
            }
        }

        main.current_mode = 2;
        UpdateClipNumber(2, automated);
    }

    public void PlayPostAction(bool automated = false){
        
        if(main.active_clip.fade_to != -1 && automated){
            main.queue.Add(main.current_song._post_round[main.active_clip.fade_to]);
        }
        else{
            main.queue.Add(main.current_song._post_round[main.clip_number[3]]);
        }

        main.current_mode = 3;
        UpdateClipNumber(3, automated);
    }

    public void PlayOutro(){
        main.queue.Add(main.current_song._out_transition);
        main.current_mode = 5;
    }

    void UpdateClipNumber(int id, bool automated_transition = false){
        //main.target_measures_time = main.queue[0].intended_length;

        main.clip_number[id]++;

        if(main.clip_number[id] == main.max_clip_number[id]){
            if(id == 2){
                ending_song = true;
            }
            main.clip_number[id] = 0;
        }


        main.ready_to_switch = true;
    }

    void EndSong()
    {
        
        SetSong(
            SongLibrary.prismatic_crystalys //SongLibrary.songs[Random.Range(0, songs.Count)]
            );

        main.ending_song = false;
    }


    //int GetActiveClipMode(){
    //    switch (main.song.name){
    //        case "prismatic_crystalys":
    //            return PrismaticCrystalysClipLibrary.Mode(main.active_clip);
//
    //    return -1;
    //}
}
