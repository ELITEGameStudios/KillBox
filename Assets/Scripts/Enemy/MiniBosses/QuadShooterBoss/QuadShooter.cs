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

    public struct GunObject{
        Transform gunTransform;
        AIShooterScript shooterScript;
    }

    [Header("Spawner prefabs")]
    public GameObject projectile;

    [Header("Fire Rate Timers")]
    public float fireTimer;
    public float fireInterval;

    [Header("Animation Curves")]
    public AnimationCurve fireLerpCurve;
    public AnimationCurve beamWidthCurve;
    public AnimationCurve specialBeamWidthCurve;

    [Header("Debug")]
    public RaycastHit2D[] raysss;

    // Start is called before the first frame update
    void Awake()
    {
        bossType = null;

        mainPhase.statesInPhase = new BossStateData[] { };
        mainPhase.minHealth = 0f;

        phases = new Phase[1] { mainPhase };
        // movement_script.enabled = false;
    }

    public void UpdateRotation(){
        mainRotator.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime)
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
