using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SongInitializer : MonoBehaviour
{
    [SerializeField]
    private AudioSystemMaster master;
    
    [SerializeField]
    private string name;

    [SerializeField]
    private AudioClip in_transition, out_transition;
    
    [SerializeField]
    private AudioClip[] ambience, pre_round, action, post_round;

    [SerializeField]
    private int BPM;


    // Start is called before the first frame update
    void Start()
    {
    //    Song song = new Song(in_transition, out_transition, ambience, pre_round, action, post_round, BPM, name);
    //    master.AddSong(song);
    //    Destroy(this);

        in_transition = Resources.Load<AudioClip>("AudioClips/prismatic_crystalys/prismatic crystalys");
    }
}
