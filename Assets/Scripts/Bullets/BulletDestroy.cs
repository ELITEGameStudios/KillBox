using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    [SerializeField] private GameObject Bullet, particle, hit_particle;
    public PoolManager poolManager;
    public GameManager gameManager;
    public int PoolIndex;
    private float current_range;
    public float rangeForFloat, range;
    private IEnumerator RangeCall;
    public ObjectPool objectPool;
    public bool destroy_on_any_collision, has_particles, destroy_on_shard_trigger, ignore_timer;



    // Start is called before the first frame update
    void Start()
    {


        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        poolManager = GameObject.Find("Manager").GetComponent<PoolManager>();
        if(PoolIndex == 0)
            objectPool = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        else
            objectPool = GameObject.Find("BulletPool"+PoolIndex.ToString()).GetComponent<ObjectPool>();
        RangeCall = Range();
        //StartCoroutine(RangeCall);
    }

    // Update is called once per frame
    void Update()
    {
        if(!ignore_timer){
            if(current_range > 0){
                current_range -= Time.deltaTime;
                if(current_range <= 0){
                    if (has_particles)
                    {
                        StartCoroutine(particle.GetComponent<BulletParticle>().PlayAnim());
                        particle.transform.SetParent(null);
                    }
                    gameObject.SetActive(false);
                }
                //int OffCounter = 0;
                //for(int i = 0; i < objectPool.amountToPool; i++){
                //    if(objectPool.pooledObjects[i].activeInHierarchy){
                //        OffCounter++;
                //        if(OffCounter == objectPool.amountToPool-1 && i == objectPool.amountToPool-1){
                //            gameObject.SetActive(false);
                //        }
                //    }
                //}
            }
        }
    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    if(collider.gameObject.tag == "Wall")
    //    {
    //        Destroy(gameObject);
    //    }
    //    if (collider.gameObject.tag == "Floor")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (destroy_on_any_collision)
        {
            ResetRangeCallWhenHit();
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (destroy_on_shard_trigger && col.gameObject.GetComponent<ShardBossScript>()!= null)
        {
            ResetRangeCallWhenHit();
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
    public void ResetRangeCall(){
        StopCoroutine(RangeCall);
        RangeCall = Range();

        if (has_particles)
        {
            //particle.transform.position = new Vector3(0, 0, 0);
            StartCoroutine(particle.GetComponent<BulletParticle>().PlayAnim());
            particle.transform.SetParent(null);
        }

        gameObject.SetActive(false);
    }

    public void ResetRangeCallWhenHit()
    {
        StopCoroutine(RangeCall);
        RangeCall = Range();

        if (has_particles && gameObject.activeInHierarchy)
        {
            //hit_particle.transform.position = new Vector3(0, 0, 0);
            StartCoroutine(hit_particle.GetComponent<BulletParticle>().PlayAnim());
            hit_particle.transform.SetParent(null);

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
        ResetRangeCall();
    }
}
