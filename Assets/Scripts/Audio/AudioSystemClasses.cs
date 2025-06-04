using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Song{
    public SongClip _in_transition {get; private set;}
    public SongClip _out_transition {get; private set;}
    
    public SongClip[] _ambience {get; private set;}
    public SongClip[] _pre_round {get; private set;}
    public SongClip[] _action {get; private set;}
    public SongClip[] _post_round {get; private set;}
    
    public int[] max_clips {get; private set;}
    
    public int BPM {get; private set;}
    
    //Measures per second  (seconds) 
    public float MPS {get; private set;}
    public string name {get; private set;}

    // CONSTRUCTOR
    public Song(int bpm, string _name, SongClip in_transition, SongClip out_transition, SongClip[] ambience, SongClip[] pre_round, SongClip[] action, SongClip[] post_round, float _mps){
        
        _ambience = ambience;
        _pre_round = pre_round;
        _action = action;
        _post_round = post_round;
        _in_transition = in_transition;
        name = _name;

        BPM = bpm;
        MPS = _mps;

        max_clips = new int[4] {_ambience.Length, _pre_round.Length, _action.Length, _post_round.Length};
    }
}

public class SongClip{
    public AudioClip clip {get; private set;}
    public int bpm {get; private set;}
    public float mpm {get; private set;}
    public int intended_length {get; private set;} // Passed in MPM
    public int fade_to {get; private set;} 
    public float real_length {get; private set;}
    //public string song {get; private set;}

    public int to_ambient_id {get; private set;}
    public int to_action_id {get; private set;}
    public int to_pre_id {get; private set;}
    public int max_plays {get; private set;}


    public SongClip(AudioClip _clip, int _bpm, int _intended_length, int _fade_to = -1, int _to_ambient_id = -1,int _to_action_id = -1,int _to_pre_id = -1, int _max_plays = -1){
        clip = _clip;
        bpm = _bpm;
        mpm = 240 / _bpm;
        intended_length = _intended_length;
        real_length = clip.length;
        fade_to = _fade_to;
        //song = _song;

        to_action_id = _to_action_id;
        to_ambient_id = _to_ambient_id;
        to_pre_id = _to_pre_id;
        max_plays = _max_plays;
    }
}

public class PrismaticCrystalysClipLibrary{
    private static int bpm = 120;
    private static string song = "prismatic_crystalys";

    public static readonly SongClip intro = 
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/intro"), bpm, 9);

    public static readonly SongClip outro = 
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/outro"), bpm, 10);
    
    public static readonly SongClip[] ambient = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/amb_1"), bpm, 13, _to_action_id: 1, _to_pre_id: 3),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/amb_2"), bpm, 15, _to_action_id: 1, _to_pre_id: 3)
    };

    public static readonly SongClip[] pre_round = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_1"), bpm, 9, _to_action_id: 0),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_2"), bpm, 9, 1, _to_action_id: 0),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_3"), bpm, 19, _to_action_id: 0),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_4"), bpm, 9, _to_action_id: 1),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_5"), bpm, 9, 4, _to_action_id: 1),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_5"), bpm, 9, 4, _to_action_id: 1),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_5"), bpm, 9, 4, _to_action_id: 2),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_5"), bpm, 9, 4, _to_action_id: 2)
    };

    public static readonly SongClip[] action = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/action_1"), bpm, 9, 0),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/action_2"), bpm, 17, 1),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/action_2"), bpm, 17, 2)
    };

    public static readonly SongClip[] post_round = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_3"), bpm, 9),
        new SongClip(Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/pre_4"), bpm, 9)
    };

    //public static int Mode(SongClip clip){
    //    
    //    foreach (SongClip item in ambient)
    //    { if (item == clip) return 0; }
    //    
    //    foreach (SongClip item in pre_round)
    //    { if (item == clip) return 1; }
    //    
    //    foreach (SongClip item in action)
    //    { if (item == clip) return 2; }
    //    
    //    foreach (SongClip item in post_round)
    //    { if (item == clip) return 3; }
//
    //    return -1;
    //}

}

public class BEAMClipLibrary{
    private static int bpm = 190;
    private static string song = "beam";

    public static readonly SongClip intro = 
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/intro"), bpm, 9);

    public static readonly SongClip outro = 
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/outro"), bpm, 9);
    
    public static readonly SongClip[] ambient = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/outro"), bpm, 9, _to_action_id: 1, _to_pre_id: 3),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/intro"), bpm, 9, _to_action_id: 1, _to_pre_id: 3)
    };

    public static readonly SongClip[] pre_round = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_1"), bpm, 17, 0,_to_action_id: 0),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_1"), bpm, 17, 1, _to_action_id: 1),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_1"), bpm, 17, 2, _to_action_id: 2),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_2"), bpm, 17, 3, _to_action_id: 3),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_3"), bpm, 17, 4, _to_action_id: 4),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_4"), bpm, 17, 5, _to_action_id: 5),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_4"), bpm, 17, 6, _to_action_id: 6),
    };

    public static readonly SongClip[] action = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/action_1"), bpm, 17, 0),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/action_2"), bpm, 25, 1),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/action_3"), bpm, 17, 2),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/action_4"), bpm, 17, 3),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/action_4"), bpm, 17, 4),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/action_5"), bpm, 17, 5),
    };

    public static readonly SongClip[] post_round = new SongClip[]{
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_3"), bpm, 9),
        new SongClip(Resources.Load<AudioClip>("AudioClips/beam/pre_4"), bpm, 9)
    };

}

public class SongLibrary{
    
    //DECLARE DEFAULT SONGS
    public static Song prismatic_crystalys {get; private set;} = new Song(
        //BPM
        120,
        //NAME
        "prismatic_crystalys",

        //In transition
        PrismaticCrystalysClipLibrary.intro,

        // out transition
        PrismaticCrystalysClipLibrary.outro,
        
        // ambience clips
        PrismaticCrystalysClipLibrary.ambient,

        // pre round clips
        PrismaticCrystalysClipLibrary.pre_round,
        
        // action clips
        PrismaticCrystalysClipLibrary.action,
        
        // post round clips
        PrismaticCrystalysClipLibrary.post_round,
        
        2
    );

    public static Song beam {get; private set;} = new Song(
        //BPM
        190,
        //NAME
        "beam",

        //In transition
        BEAMClipLibrary.intro,

        // out transition
        BEAMClipLibrary.outro,
        
        // ambience clips
        BEAMClipLibrary.ambient,

        // pre round clips
        BEAMClipLibrary.pre_round,
        
        // action clips
        BEAMClipLibrary.action,
        
        // post round clips
        BEAMClipLibrary.post_round,

        1.263f
    );
}
