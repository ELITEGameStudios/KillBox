using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudio : MonoBehaviour
{
    // Start is called before the first frame update

    public static BossAudio Instance {get; private set;}

    [SerializeField]
    private AudioSource source;

    
    [SerializeField]
    private AudioClip[] clips;

    private bool shard_has_spawned = false, check = false, is_playing_audio = false;

    [SerializeField]
    private GameManager manager;

    [SerializeField]
    private GameObject target_boss;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    //public void OnShardSpawn(GameObject caller_obj){

        //if(!check){
            //shard_has_spawned = true;

            //target_boss = caller_obj;
    //
            //source.clip = clips[Random.Range(0, clips.Length)];
            //source.Play();
            //    
    //
            //StartCoroutine(manager.AudioFade(1f, true));
    //
            //check = true;
            //is_playing_audio = true;
        //}
    //} 

    //void Update(){
    //    if(!check && is_playing_audio){

    //        is_playing_audio = false;

            //StartCoroutine("DeathSequence");
            //StartCoroutine(manager.AudioFade(0.2f, false));
            
    //    }
    //}

    //public bool Check {get {return check;} set{check = value;}}

    //IEnumerator DeathSequence(){

        //gameObject.GetComponent<AudioProfile>().in_transition = true;
        //while (source.volume > 0){
        //    source.volume -= 0.2f * Time.deltaTime;
//
        //    yield return null;
        //}
//
        //source.volume = 0;
        //source.Stop();
    //}


}
