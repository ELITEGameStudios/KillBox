using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadShooter : BossBase
{
    [Header("General Info")]
    public float maxSpeed;
    public float rotationSpeed;
    public BossDisplayObj linkedDisplay;

    public Transform mainRotator;
    public Transform[] mainGunPositions;
    public GunObject[] guns;

    [Header("State Info")]
    public Phase mainPhase;
    public QuadShooterMainAttack mainAttack;
    public QuadShooterWait waitState;

    [Header("Spawner prefabs")]
    public GameObject projectile;

    [Header("Fire Rate Timers")]
    public int startingBullets = 8;
    public int sub50Bullets = 16;
    public AnimationCurve fireRateCurve;

    [Header("Animation Curves")]

    [Header("Debug")]
    public RaycastHit2D[] raysss;

    
    [System.Serializable]
    public struct GunObject{
        public Transform transform;
        public AIShooterScript shooterScript;
    }

    // Start is called before the first frame update
    void Awake()
    {
        bossType = null;

        mainAttack = new QuadShooterMainAttack(this);
        waitState = new QuadShooterWait(this, 3.5f, 4);

        mainPhase.statesInPhase = new BossStateData[] { mainAttack, waitState };
        mainPhase.minHealth = 0f;

        phases = new Phase[1] { mainPhase };
        // movement_script.enabled = false;
    }

    protected override void OnUpdate()
    {
        mainAttack.bullets = normalizedHealth > 0.5f ? startingBullets : sub50Bullets;
    }

    public void UpdateRotation(){
        mainRotator.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
    }

    public GameObject GetInstantiate(GameObject prefab, Transform transform){
        return Instantiate(prefab, transform);
    }

    protected override void OnFixedUpdate()
    {
        rb_self.velocity = (Player.main.tf.position - transform.position).normalized * (maxSpeed * 1 - (health.CurrentHealth / health.maxHealth)) * Time.fixedDeltaTime;
        UpdateRotation();
    }
}
