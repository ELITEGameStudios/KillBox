using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;

public class shooterScript2D : MonoBehaviour
{
    public int bulletDamage;

    public float cooldown_units, burst_quantity;
    public string bulletName;
    public GameObject bullet, ShootGraphic, GraphicClone;
    public Transform playerTf, Spawn;
    public float Velocity, spread, range, burst_interval, recoilForce;
    public Rigidbody2D bulletRb, clone;
    public Vector3 SpawnRot;
    public bool shootInputIsPressed, is_dual, misc_gun, uniform_spread, is_in_ui;
    public int bulletsPerShot, poolIndex, graphicPoolIndex, penetration;
    public float FR;
    public Camera cam;
    public Joystick shootJoystick;
    public AudioSource audio;
    public ObjectPool[] objectPool;

    public Color bullet_color, particle_color, default_color;

    [SerializeField]
    private bool delay_bool, in_coroutine, burst, burst_status, set_on_awake, themed;

    [SerializeField]
    private string set_on_awake_key;

    [SerializeField]
    private TwoDPlayerController player_controller;

    private IEnumerator delayClone;

    public BuffsManager buffsManager;

    [SerializeField]
    private int burst_rounds;
 
    
    private float shoot_warmup_time, shoot_warmup_timer;

    [SerializeField]
    private GameManager manager;

    private float shootCooldownTimer;
    public bool CanShoot {get {return shootCooldownTimer <= 0; }}
    Vector2 mousePos;

    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private WeaponItem weaponItem;

    [SerializeField]
    private List<float> uniform_directions;

    GameObject clone_sprite;
    ParticleSystem clone_particle;
    ParticleSystem.TrailModule trails;
    [SerializeField] private AnimationCurve recoilCurve;
    [SerializeField] private Transform gunGraphicTf;
    [SerializeField] private Vector3 initGunPos;
    [SerializeField] private float recoilTime, initRecoilTime;

    void Awake(){
        initGunPos = gunGraphicTf.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        // CanShoot = true;
        SpawnRot = Spawn.localEulerAngles;
        objectPool[0] = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        objectPool[1] = GameObject.Find("BulletPool1").GetComponent<ObjectPool>();
        buffsManager = GameObject.FindWithTag("Player").GetComponent<BuffsManager>();
        manager = GameObject.Find("Manager").GetComponent<GameManager>();

        weaponItem = WeaponItemList.Instance.GetItem("Pistol");
        weapon = weaponItem.weapon;

        

        delayClone = Delay();
        delay_bool = true;

        if(set_on_awake){
            SetWeapon( WeaponItemList.Instance.GetItem(set_on_awake_key) );
        }

        Themify();

        //poolManager = GameObject.Find("Manager").GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!CanShoot){
            shootCooldownTimer -= Time.deltaTime;
        }

        if(manager == null || !manager.started_game){
            return;
        }

        if(GunHandler.Instance.in_ui){
            is_in_ui = true;
        }
        else{
            is_in_ui = false;
        }

        if(player_controller.mobile || DetectInputDevice.main.isController){
            if (player_controller.Rotating && CanShoot && !GunHandler.Instance.cooldown.cooling_down && shootInputIsPressed && !is_in_ui)
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
                    shootInputIsPressed = true;
                }
            }

            if (!player_controller.Rotating && !delay_bool)
            {
                delay_bool = true;
            }

            if (!player_controller.Rotating && shootInputIsPressed)
            {
                shootInputIsPressed = false;
            }
        }
        else{
        //if (shootJoystick.Vertical != 0 && CanShoot && FullAuto){
        //    Debug.Log("Catch3");
        //    FAS = true;
        //}
            if (Input.GetKey(CustomKeybinds.main.Shoot))
                shootInputIsPressed = true;
            else
                shootInputIsPressed = false;

            if (shootInputIsPressed && CanShoot && !GunHandler.Instance.cooldown.cooling_down && !is_in_ui)
            {
                Shoot();
            }
        }



        // if(shoot_warmup_timer > 0 && !CanShoot){
        //     shoot_warmup_timer -= Time.deltaTime;
        //     if(shoot_warmup_timer <= 0){
        //         // CanShoot = true;
        //     }
        // }

        if(recoilTime > 0 && gunGraphicTf != null){
            
            gunGraphicTf.localPosition = initGunPos - Vector3.up * recoilCurve.Evaluate(recoilTime/initRecoilTime);
            recoilTime -= Time.deltaTime;
        }
        else if(gunGraphicTf != null ){
            gunGraphicTf.localPosition = initGunPos;

        }
    }

    void OnEnable(){
        shootCooldownTimer = 0.25f;
        // if(!CanShoot){
        //     shoot_warmup_timer = 0.25f;
        // }
    }

    void StartPauseWarmupTimer(){

    }

    void Shoot()
    {
        if(uniform_spread){
            uniform_directions = new List<float>();

            float step = spread / bulletsPerShot;
            for (int i = 1; i <= bulletsPerShot; i++){
                uniform_directions.Add((step * i) + ((- spread - step )/ 2));
            }
        }

        for(int i = 0; i < bulletsPerShot; i++)
        {
            if(!uniform_spread){
                Spawn.localEulerAngles += new Vector3(0, 0, Random.Range(-spread, spread));
            }
            else{
                Spawn.localEulerAngles += new Vector3(0, 0, uniform_directions[i]);
            }

            recoilTime = Mathf.Clamp(FR, 0.15f, Mathf.Infinity);
            initRecoilTime = recoilTime;

            if(!misc_gun){
                clone = objectPool[0].GetPooledObject().GetComponent<Rigidbody2D>();

                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;
                clone.gameObject.SetActive(true);
                clone.gameObject.GetComponent<BulletDestroy>().NewTimer(range);


                clone.gameObject.GetComponent<BulletClass>().SetBullet(bulletName, bulletDamage, penetration_input: penetration, _range: range);
            }
            else
            {
                GameObject misc_bullet = objectPool[0].GetPooledObject();

                clone = misc_bullet.GetComponent<Rigidbody2D>();
                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;
                clone.gameObject.SetActive(true);
                
                if(clone.gameObject.GetComponent<BulletDestroy>() != null){
                    clone.gameObject.GetComponent<BulletDestroy>().NewTimer(range);
                }
                    
                clone.gameObject.transform.SetParent(null);
            }
            
            //setting color
            if(!misc_gun){
                clone_sprite = clone.gameObject.transform.GetChild(0).gameObject;
                clone_particle = clone_sprite.GetComponent<ParticleSystem>();
                trails = clone_particle.trails;
                clone_sprite.GetComponent<SpriteRenderer>().color = bullet_color;
            }

            if(weapon.is_support){
                bullet_color = Color.cyan;
                particle_color = Color.cyan;
            }
            else if(!misc_gun){
                Themify();
            }

            if(!misc_gun){
                clone_particle.startColor = particle_color;
                trails.colorOverLifetime = particle_color;
                trails.colorOverTrail = particle_color;
            }

            //AddingForces
            clone.AddForce(Spawn.up * Velocity);
            Player.main.movement.AddCustomForce(
                new CustomForce(-Spawn.up * 1, time: 0.33f)
            );
            
            
            if (recoilForce > 0) { Player.main.rb.AddForce(Player.main.tf.up * -recoilForce, ForceMode2D.Impulse); }
            Spawn.localEulerAngles = SpawnRot;
            audio.pitch = Random.Range(0.9f, 1.1f) + GunHandler.Instance.cooldown.CurrentChargeNormalized;
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
        
        // CanShoot = false;
        burst_rounds++;

        GunHandler.Instance.OnShoot();
        // KillboxEventSystem.TriggerFireWeaponEvent(new WeaponEventData(weaponItem, GunHandler.Instance.current_is_primary, GunHandler.Instance.has_dual));

        
        if (burst_rounds >= burst_quantity && burst)
        {
            ShootCooldown(burst_check: true);
            // ShootCooldown(burst_check: true);
        }
        else
        {
            ShootCooldown();
            // ShootCooldown();
        }
    }

    public void SetWeapon(WeaponItem input)
    {
        weaponItem= input;
        Weapon weapon = input.weapon;

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
        uniform_spread = weapon.uniform;
        recoilForce = weapon.recoilForce;

        if (weapon.pool != 0)
        {
            objectPool[0] = GameObject.Find("BulletPool" + weapon.pool.ToString()).GetComponent<ObjectPool>();
        }
        else if (weapon.is_support)
        {
            objectPool[0] = GameObject.Find("BulletPool" + 12.ToString()).GetComponent<ObjectPool>();
        }
        else
        {
            objectPool[0] = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
        }

        misc_gun = weapon.misc;

        burst_rounds = 0;

    }

    void Themify(){
        if(themed && manager.Theme != 0){
            bullet_color = manager.ColorThemes[manager.Theme];
            particle_color = manager.ColorThemes[manager.Theme];
        }
        else{
            bullet_color = default_color;
            particle_color = default_color;
        }
    }

    void ResetCloneCoroutine()
    {
        StopCoroutine(delayClone);
        delayClone = Delay();
    }

    IEnumerator ShootCooldownNumerator(bool burst_check = false)
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

        // CanShoot = true;
    }

    void ShootCooldown(bool burst_check = false)
    {
        if (burst_check && burst)
        {
            // yield return new WaitForSeconds(burst_interval);
            shootCooldownTimer = burst_interval;
            burst_rounds = 0;
        }
        else
        {
            shootCooldownTimer = FR;
            // yield return new WaitForSeconds(FR);
        }

        // CanShoot = true;
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
