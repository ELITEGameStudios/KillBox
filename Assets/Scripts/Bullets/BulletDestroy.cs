using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    [SerializeField] private GameObject particle, hit_particle;
    private float current_range;
    public float rangeForFloat, range;
    private IEnumerator RangeCall;
    public ObjectPool objectPool;
    public bool destroy_on_any_collision, has_particles, destroy_on_shard_trigger, ignore_timer;

    // public delegate void OnPreDestroy();
    public Action PreDestroy;



    // Start is called before the first frame update
    void Start()
    {
        RangeCall = Range();
        //StartCoroutine(RangeCall);
    }

    // Update is called once per frame
    void Update()
    {
        if(!ignore_timer && current_range > 0){
            current_range -= Time.deltaTime;

            if(current_range <= 0){

                if (has_particles)
                {
                    StartCoroutine(particle.GetComponent<BulletParticle>().PlayAnim());
                    particle.transform.SetParent(null);
                }

                gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (destroy_on_any_collision)
        {
            DisableBullet();
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (destroy_on_shard_trigger && col.gameObject.GetComponent<ShardBossScript>()!= null)
        {
            DisableBullet();
        }
    }

    public void RestartTimer(){
        current_range = range;
    }

    public void NewTimer(float new_range){
        range = new_range;
        current_range = range;
    }

    public void StartRangeCall(){
        StartCoroutine(RangeCall);
    }

    public void DisableBullet(bool hitObject = true)
    {
        if(PreDestroy != null) PreDestroy.Invoke();

        StopCoroutine(RangeCall);
        RangeCall = Range();

        if (has_particles && gameObject.activeInHierarchy)
        {
            if (hitObject)
            {
                StartCoroutine(hit_particle.GetComponent<BulletParticle>().PlayAnim());
                hit_particle.transform.SetParent(null);    
            }
            else
            {
                StartCoroutine(particle.GetComponent<BulletParticle>().PlayAnim());
                particle.transform.SetParent(null);
            }
        }

        gameObject.SetActive(false);
    }


    public IEnumerator Range()
    {
        //if (has_particles)
        //{
        //    particle.transform.position = new Vector3(0, 0, 0);
        //    hit_particle.transform.position = new Vector3(0, 0, 0);
        //    hit_particle.transform.localEulerAngles = new Vector3(0, 0, 0);
        //}

        yield return new WaitForSeconds(rangeForFloat);
        DisableBullet();
    }
}
