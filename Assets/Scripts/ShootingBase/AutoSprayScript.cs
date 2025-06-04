using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSprayScript : MonoBehaviour
{
    public int bulletDamage;
    public float cooldown_units;
    public string bulletName;
    public GameObject bullet, GraphicClone;
    public Transform Spawn;
    public float Velocity, spread, range;
    public Rigidbody2D bulletRb, clone;
    public Vector3 SpawnRot;
    public bool CanShoot, FullAuto, FAS, is_dual, misc_gun, display_flash;
    public int BulletsPerShot, poolIndex, graphicPoolIndex;
    public float FR, spreadX, spreadY;
    public Camera cam;
    public AudioSource audio;
    public ObjectPool[] objectPool;

    public Color bullet_color, particle_color;

    // Start is called before the first frame update
    void Start()
    {
        CanShoot = true;
        SpawnRot = Spawn.localEulerAngles;
        objectPool[0] = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        objectPool[1] = GameObject.Find("BulletPool1").GetComponent<ObjectPool>();
        //poolManager = GameObject.Find("Manager").GetComponent<PoolManager>();
    }
    // Update is called once per frame
    void Update()
    {
        while (CanShoot)
            Shoot();
        
    }

    void Shoot()
    {

        for(int i = 0; i < BulletsPerShot; i++)
        {
            Spawn.localEulerAngles += new Vector3(0, 0, Random.Range(-spread, spread));
            if(!misc_gun){
                if(objectPool[0].GetPooledObject() != null)
                    clone = objectPool[0].GetPooledObject().GetComponent<Rigidbody2D>();
                else{
                    clone = GameObject.FindWithTag("Bullet").GetComponent<Rigidbody2D>();
                }
                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;
                clone.gameObject.GetComponent<BulletDestroy>().NewTimer(range);
                clone.gameObject.SetActive(true);

                clone.gameObject.GetComponent<BulletClass>().SetDmg(bulletDamage);
                clone.gameObject.GetComponent<BulletClass>().SetName(bulletName);
            }
            else
            {
                GameObject misc_bullet = Instantiate(bullet, Spawn.transform);
                clone = misc_bullet.GetComponent<Rigidbody2D>();
                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;
                clone.gameObject.transform.SetParent(null);
            }

            //setting color
            GameObject clone_sprite = clone.gameObject.transform.GetChild(0).gameObject;
            ParticleSystem clone_particle = clone_sprite.GetComponent<ParticleSystem>();
            var trails = clone_particle.trails;

            clone_sprite.GetComponent<SpriteRenderer>().color = bullet_color;
            clone_particle.startColor = particle_color;
            trails.colorOverLifetime = particle_color;
            trails.colorOverTrail = particle_color;

            //AddingForces
            clone.AddForce(Spawn.up * Velocity);
            Spawn.localEulerAngles = SpawnRot;
            audio.pitch = Random.Range(0.9f, 1.1f);
            audio.Play();
            
        }

        //GraphicClone = Instantiate(ShootGraphic, Spawn.position, Spawn.rotation);
        if(objectPool[1].GetPooledObject() != null && display_flash){
            GraphicClone = objectPool[1].GetPooledObject();
            GraphicClone.gameObject.transform.position = Spawn.position;
            GraphicClone.gameObject.transform.rotation = Spawn.rotation;
            GraphicClone.gameObject.GetComponent<BulletDestroy>().RestartTimer();
            GraphicClone.gameObject.SetActive(true);
            
            ParticleSystem flash_particle = GraphicClone.GetComponent<ParticleSystem>();
            flash_particle.startColor = particle_color;
            flash_particle.Play();
        }

        CanShoot = false;
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(FR);
        CanShoot = true;
    }
}
