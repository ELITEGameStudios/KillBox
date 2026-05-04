using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : BulletClass
{
    [SerializeField] private GameObject prefab, ExplosionObject, ExplosionClone;
    public readonly Rigidbody2D ExplosionPrefab;

    float range_clock;
    private bool has_exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        // range_clock = range;
    }

    void OnEnable(){
        // range_clock = range;
        bulletDestroy.PreDestroy += OnPreDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        // if(range_clock > 0){
        //     range_clock -= Time.deltaTime;
        // }
        // else
        // {
        //     RemoteExplosion();
        // }
    }

    void OnPreDestroy()
    {
        ExplosionClone = Instantiate(ExplosionObject, gameObject.transform);
        if(ExplosionClone.GetComponent<explosionClass>() != null){
            if(damage < 125){
                ExplosionClone.GetComponent<explosionClass>().SetExplosion(damage, 10, damage / 25, damage / 10);
            }
            else{
                ExplosionClone.GetComponent<explosionClass>().SetExplosion(damage, 10, damage / 60, damage / 5);
            }
            
        }

        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
        ExplosionClone.transform.SetParent(null);
        
        bulletDestroy.PreDestroy -= OnPreDestroy;
    }

    public void RemoteExplosion(){
        bulletDestroy.DisableBullet();
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag != "Grenade"){
            if(collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player"){
                RemoteExplosion();
            }
        }
    }

    protected override void TriggerEnter(Collider2D collider){
        if(collider.gameObject.tag != "Grenade"){
            if(collider.gameObject.tag != "Bullet" && collider.gameObject.tag != "Player"){
                RemoteExplosion();
            }
        }
    }
}
