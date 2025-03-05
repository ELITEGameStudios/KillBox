using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class HelpfulAIShooterScript : MonoBehaviour
{
    public GameObject bullet, DestObj;
    public GameObject[] DestPool;

    public string bulletName;

    public BuffsManager buffsManager;

    public Transform playerTf, Spawn, Enemytf, playerTf2;
    public float Velocity, spread, cooldown_units;
    public Rigidbody2D bulletRb, clone;
    public Vector3 SpawnRot;
    public bool CanShoot, FAS, misc_gun;
    public int BulletsPerShot, bulletDamage;
    public float FR, AimProxim, range;
    public Camera cam;
    public AIDestinationSetter TargSetter;
    public ObjectPool[] objectPool;

    public Color bullet_color, particle_color;

    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        CanShoot = true;
        SpawnRot = Spawn.localEulerAngles;
        TargSetter = GetComponent<Pathfinding.AIDestinationSetter>();

        objectPool[0] = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        objectPool[1] = GameObject.Find("BulletPool1").GetComponent<ObjectPool>();
        buffsManager = GameObject.FindWithTag("Player").GetComponent<BuffsManager>();

        playerTf2 = GameObject.Find("Player").transform;

    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (TargSetter.target != null) { Enemytf = TargSetter.target.transform; }

        RaycastHit2D hit = Physics2D.Raycast(playerTf.position, playerTf.forward, Mathf.Infinity);

        Debug.DrawRay(playerTf.position, playerTf.forward);


        if (Enemytf != null)
        {
            if (DestObj != playerTf2.gameObject && DestObj != null)
                FAS = true;
            else
                FAS = false;
        }
    }
    void Update()
    {
        DestPool = GameObject.FindGameObjectsWithTag("Enemy");


        if(DestPool.Length != 0)
        {
            float closest_dist = 100;
            for(int i = 0; i < DestPool.Length; i++)
            {
                if(Vector3.Distance(DestPool[i].transform.position, playerTf2.transform.position) < closest_dist)
                {
                    closest_dist = Vector3.Distance(DestPool[i].transform.position, playerTf2.position);
                    DestObj = DestPool[i];
                }
            }
        }
        
        if(DestObj == null)
        {
            if (Vector3.Distance(transform.position, playerTf2.position) > 1)
            {
            DestObj = playerTf2.gameObject;
            }
            else
            {
                DestObj = null;
            }

            FAS = false;
        }

        if (DestObj != null) { TargSetter.target = DestObj.transform; }

        if (FAS && CanShoot)
            Shoot();
    }

    void Shoot()
    {

        for (int i = 0; i < BulletsPerShot; i++)
        {
            Spawn.localEulerAngles += new Vector3(0, 0, Random.Range(-spread, spread));
            if (!misc_gun)
            {
                if (objectPool[0].GetPooledObject() != null)
                    clone = objectPool[0].GetPooledObject().GetComponent<Rigidbody2D>();
                else
                {
                    clone = GameObject.FindWithTag("Bullet").GetComponent<Rigidbody2D>();
                }
                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;
                clone.gameObject.GetComponent<BulletDestroy>().NewTimer(range);
                clone.gameObject.SetActive(true);

                clone.gameObject.GetComponent<BulletClass>().SetBullet(bulletName, bulletDamage);
                //clone.gameObject.GetComponent<BulletClass>().SetPenetration(buffsManager);
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
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().Play();
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
