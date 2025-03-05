using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] private GameObject prefab, ExplosionObject, ExplosionClone;
    public float range;
    public readonly Rigidbody2D ExplosionPrefab;

    private BulletClass bullet_data;

    float range_clock;
    private bool has_exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        bullet_data = gameObject.GetComponent<BulletClass>();
        range = bullet_data.range;
        range_clock = range;
    }

    void OnEnable(){
        range_clock = range;
    }

    // Update is called once per frame
    void Update()
    {
        if(range_clock > 0){
            range_clock -= Time.deltaTime;
        }
        else
        {
            ExplosionClone = Instantiate(ExplosionObject, gameObject.transform);

            if(ExplosionClone.GetComponent<explosionClass>() != null){
                if(bullet_data.damage < 125){
                    ExplosionClone.GetComponent<explosionClass>().SetExplosion(bullet_data.damage, 10, bullet_data.damage / 25, bullet_data.damage / 10);
                }
                else{
                    ExplosionClone.GetComponent<explosionClass>().SetExplosion(bullet_data.damage, 10, bullet_data.damage / 60, bullet_data.damage / 5);
                }
                
            }

            gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.clear;

            ExplosionClone.transform.SetParent(null);

            gameObject.SetActive(false);
        }
    }

    public void RemoteExplosion(){
        range_clock = 0;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag != "Grenade"){
            if(collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player"){
                range_clock = 0;
            }
        }
    }
    

}
