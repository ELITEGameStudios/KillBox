using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;

public class KunaiShooterScript : MonoBehaviour
{
    public int bulletDamage;

    public float cooldown_units, burst_quantity;
    public string bulletName;
    public GameObject bullet, ShootGraphic, GraphicClone;
    public Transform playerTf, Spawn;
    public float Velocity, spread, range, burst_interval;
    public Rigidbody2D bulletRb, clone;
    public Vector3 SpawnRot;
    public bool CanShoot, FAS, is_dual, misc_gun;
    public int bulletsPerShot, poolIndex, graphicPoolIndex, penetration;
    public float FR;
    public Camera cam;
    public Joystick shootJoystick;
    public AudioSource audio;
    public ObjectPool[] objectPool;

    public Color bullet_color, particle_color, default_color;

    [SerializeField]
    private bool delay_bool, in_coroutine, burst, burst_status;

    [SerializeField]
    private TwoDPlayerController player_controller;

    private IEnumerator delayClone;

    public BuffsManager buffsManager;

    [SerializeField]
    private int burst_rounds;

    [SerializeField]
    private GameManager manager;

    Vector2 mousePos;

    [SerializeField]
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        CanShoot = true;
        SpawnRot = Spawn.localEulerAngles;
        objectPool[0] = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        objectPool[1] = GameObject.Find("BulletPool1").GetComponent<ObjectPool>();
        buffsManager = GameObject.FindWithTag("Player").GetComponent<BuffsManager>();
        manager = GameObject.Find("Manager").GetComponent<GameManager>();

        //bullet_color = default_color;

        delayClone = Delay();
        delay_bool = true;

        //poolManager = GameObject.Find("Manager").GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!manager.started_game){
            return;
        }

        if(player_controller.mobile || DetectInputDevice.main.isController){
            if (player_controller.Rotating && CanShoot && !GunHandler.Instance.cooldown.cooling_down && FAS)
            {
                Shoot();
            }


            if (player_controller.Rotating)
            {
                if (delay_bool && !in_coroutine)
                {
                    StartCoroutine(delayClone);

                }
                if (!delay_bool)
                {
                    FAS = true;
                }
            }

            if (!player_controller.Rotating && !delay_bool)
            {
                delay_bool = true;
            }

            if (!player_controller.Rotating && FAS)
            {
                FAS = false;
            }
        }
        else{
        //if (shootJoystick.Vertical != 0 && CanShoot && FullAuto){
        //    Debug.Log("Catch3");
        //    FAS = true;
        //}
            if (Input.GetKey(KeyCode.Mouse0))
                FAS = true;
            else
                FAS = false;

            if (FAS && CanShoot && !GunHandler.Instance.cooldown.cooling_down)
            {
                Shoot();
                Debug.Log("Catch4");
            }
        }
    }

    void Shoot()
    {

        for(int i = 0; i < bulletsPerShot; i++)
        {
            Spawn.localEulerAngles += new Vector3(0, 0, Random.Range(-spread, spread));
            if(!misc_gun){
                if(objectPool[0].GetPooledObject() != null)
                    clone = objectPool[0].GetPooledObject().GetComponent<Rigidbody2D>();
                else{
                    if(GameObject.FindWithTag("Bullet") != null){
                        clone = GameObject.FindWithTag("Bullet").GetComponent<Rigidbody2D>();
                        clone.gameObject.SetActive(false);
                    }
                    else{ //if(GameObject.FindWithTag("Grenade") != null){
                        //clone = GameObject.FindWithTag("Grenade").GetComponent<Rigidbody2D>();
                        //clone.GetComponent<GrenadeScript>().RemoteExplosion();
                        
                        return;
                    }
                }
                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;
                clone.gameObject.SetActive(true);
                clone.gameObject.GetComponent<BulletDestroy>().NewTimer(range);


                clone.gameObject.GetComponent<BulletClass>().SetBullet(bulletName, bulletDamage, penetration_input: penetration, _range: range);
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

            GunHandler.Instance.cooldown.AddCount(this, cooldown_units);
            
        }

        //GraphicClone = Instantiate(ShootGraphic, Spawn.position, Spawn.rotation);
        if(objectPool[1].GetPooledObject() != null){
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
        burst_rounds++;

        if (burst_rounds >= burst_quantity && burst)
        {
            StartCoroutine(ShootCooldown(burst_check: true));
        }
        else
        {
            StartCoroutine(ShootCooldown());
        }
    }

    public void SetWeapon(Weapon input)
    {
        weapon = input;

        FR = weapon.fire_rate;
        cooldown_units = weapon.cooldown_units;
        Velocity = weapon.velocity;
        spread = weapon.spread;
        range = weapon.range;
        bulletDamage = weapon.damage;
        bulletsPerShot = weapon.bullets_per_shot;
        burst = weapon.burst;
        burst_quantity = weapon.burst_quantity;
        burst_interval = weapon.burst_interval;
        penetration = weapon.penetration;

        if(weapon.pool != 0){
            objectPool[0] = GameObject.Find("BulletPool"+weapon.pool.ToString()).GetComponent<ObjectPool>();
        }
        else if(weapon.is_support){
            objectPool[0] = GameObject.Find("BulletPool"+ 12.ToString()).GetComponent<ObjectPool>();
        }
        else{
            objectPool[0] = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        }

        burst_rounds = 0;

    }

    void ResetCloneCoroutine()
    {
        StopCoroutine(delayClone);
        delayClone = Delay();
    }

    IEnumerator ShootCooldown(bool burst_check = false)
    {
        if (burst_check && burst)
        {
            yield return new WaitForSeconds(burst_interval);
            burst_rounds = 0;
        }
        else
        {
            yield return new WaitForSeconds(FR);
        }

        CanShoot = true;
    }

    IEnumerator Delay()
    {
        in_coroutine = true;
        yield return new WaitForSecondsRealtime(0.05f);
        delay_bool = false;
        in_coroutine = false;
        ResetCloneCoroutine();
    }
}
