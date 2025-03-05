using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KunaiScript : MonoBehaviour
{
    private ObjectPool objectPool;
    private GameObject explosion;

    [SerializeField]
    private BulletDestroy destroyer;
    
    [SerializeField]
    private bool boomerang;
    private bool has_elapsed;

    [SerializeField]
    private Vector3 target;

    [SerializeField]
    private float boomerang_timer, elapsed_time, normalized_time, distance;

    [SerializeField]
    private int state, rate;
    


    [SerializeField]
    private GameObject player;

    [SerializeField]
    private AIPath ai;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        objectPool = GameObject.Find("KunaiExpPool").GetComponent<ObjectPool>();
    }

    void OnEnable(){
        if(boomerang){

            float rotation = player.transform.localEulerAngles.z + 90;

            target = transform.position + new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad) * distance, Mathf.Sin(rotation* Mathf.Deg2Rad) * distance, 0);
        }

        elapsed_time = 0;
        normalized_time = 0;
        state = 0;
        has_elapsed = false;
        
    }

    void Update()
    {
        if(boomerang){
            transform.localEulerAngles += new Vector3(0, 0, rate * Time.deltaTime);
        }

        if(boomerang && state == 0){
            transform.position = Vector3.Lerp(player.transform.position, target, Mathf.Sin(normalized_time * Mathf.PI));
        }

        else if(boomerang && state == 1){
            transform.position = Vector3.Lerp(player.transform.position, target, (1 - normalized_time));
        }

        //if(normalized_time >= 1 && state == 0){
        //    elapsed_time = 0;
        //    normalized_time = 0;
        //    state++;
        //}

        if(normalized_time >= 1 && state == 0){
            gameObject.SetActive(false);
        }

        if(boomerang){
            elapsed_time += Time.deltaTime;
            normalized_time = elapsed_time / boomerang_timer;
        }

        //if(elapsed_time >= boomerang_timer && boomerang){
//
        //    GameObject player = GameObject.FindWithTag("Player");
        //    if(ai_dest != null){
        //        
        //        ai.enabled = true;
        //        ai_dest.target = player.transform;
        //        has_elapsed = true;
        //    }
        //}
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.tag == "Enemy"){

            explosion = objectPool.GetPooledObject();
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;

            ParticleSystem[] effects = new ParticleSystem[]{
                explosion.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>(),
                explosion.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>(),
                explosion.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>()
            };

            explosion.transform.SetParent(null);
        
        }

        //if(col.tag == "Player" && boomerang){
        //    destroyer.ResetRangeCallWhenHit();
        //}

        //foreach (ParticleSystem item in effects)
        //{
        //    item.Play();
        //}
    }
}
