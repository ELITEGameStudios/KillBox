using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueBoss : BossBase
{
    [Header("General Info")]
    public float epilogueTimer;
    public float maxSpeed;
    public BossDisplayObj linkedDisplay;


    [Header("Rune Info")]
    // public FixedRotator runesRotator;
    public PrologueRuneScript[] runeList;
    public Transform[] runeParentTf;


    [Header("State Info")]
    public Phase prologuePhase;
    public Phase epiloguePhase;

    // Prologue
    public PrologueDrainAttack healthDrain, speedDrain;
    public PrologueShootAttack shootAttack;
    public PrologueRuneLaserAttack runeLaserAttack;

    // Epilogue
    public PrologueSpecialRuneLaserAttack epilogueLasers;
    public PrologueAOERuneAttack epilogueAOE;

    [Header("Drain Graphic")]
    public SpriteRenderer[] drainGraphics;
    public Transform drainTransform, hitEffectTransform;
    public Color[] debuffColor;
    public Color specialColor;
    public GameObject beamObject;
    public GameObject[] specialBeamObjects;
    public bool drainedThisFrame;


    [Header("Spawner prefabs")]
    public GameObject drainSpawn;
    public GameObject runeProjectile;
    public SweepingIndicator sweepingIndicator;
    public List<PrologueRuneProjectile> runeExplosionPool;
    public AOEAttackRune AOEPrefab;
    public GameObject[] entitiesToSpawn;

    [Header("Fire Rate Timers")]
    public float fireTimer;
    public float fireInterval;

    [Header("Animation Curves")]
    public AnimationCurve fireLerpCurve;
    public AnimationCurve beamWidthCurve;
    public AnimationCurve specialBeamWidthCurve;

    [Header("Debug")]
    public RaycastHit2D[] raysss;

    public enum DebuffType
    {
        HEALTH,
        SPEED,
        CAPACITY,
        LIFESTEAL
    }

    public enum SpecialDebuffType
    {
        UPGRADES, // Megadebuffs all stats
        WEAPONS, // Disables Weapons
        NECRO, // Spawns Many special enemies
        EQUIPMENT, // Removes all Kills towards equipment/deactivats equipment?
    }

    // Start is called before the first frame update
    void Awake()
    {
        bossType = BossRoundManager.BossType.PROLOGUE;
        
        healthDrain = new PrologueDrainAttack(this, 9, DebuffType.HEALTH);
        speedDrain = new PrologueDrainAttack(this, 9, DebuffType.SPEED);
        shootAttack = new PrologueShootAttack(this, 10, 0.33f, 3, 12);
        runeLaserAttack = new PrologueRuneLaserAttack(this, 4, 2, 1, 9);
        epilogueAOE = new PrologueAOERuneAttack(this, iterations: 5);

        // prologuePhase.statesInPhase = new BossStateData[] {  shootAttack, epilogueAOE };
        prologuePhase.statesInPhase = new BossStateData[] { healthDrain, runeLaserAttack, shootAttack, speedDrain, runeLaserAttack };
        // prologuePhase.statesInPhase = new BossStateData[] { epilogueAOE };
        prologuePhase.minHealth = 0f;

        epiloguePhase.statesInPhase = new BossStateData[] { epilogueLasers, epilogueAOE };
        epiloguePhase.minHealth = -1f;
        

        phases = new Phase[2] { prologuePhase, epiloguePhase };


        fireTimer = fireInterval;
        ResetDrainGraphic();
        movement_script.enabled = false;
    }

    public void SetRotSpeed(float speedConstant){
        animator.SetFloat("RotationSpeedConstant", speedConstant);
        animator.SetBool("Rotating", speedConstant > 0.0f);
    }
    public void SetRuneVisibility(bool visible){
        foreach (PrologueRuneScript rune in runeList){
            if(visible){ rune.Appear(); }
            else{ rune.Dissapear(); }
        }
    }

    public PrologueRuneProjectile GetNewRuneProjectile()
    {
        // PrologueRuneProjectile obj;
        // foreach (PrologueRuneProjectile item in runeExplosionPool)
        // {
        //     if (item.state == PrologueRuneProjectile.State.INACTIVE)
        //     {
        //         return item;
        //     }
        // }

        GameObject newObj = Instantiate(runeProjectile, transform);
        newObj.transform.SetParent(null);

        PrologueRuneProjectile newProjectile = newObj.GetComponent<PrologueRuneProjectile>();
        runeExplosionPool.Add(newProjectile);
        return newProjectile;
    }

    public void ResetDrainGraphic() // Bool returns if the drain graphic is hitting the player
    {
        foreach(var graphic in drainGraphics) graphic.color = Color.clear;
        drainTransform.localScale = Vector3.one;
        drainTransform.rotation = transform.rotation;
    }


    public bool SetDrainGraphic(DebuffType debuffType) // Bool returns if the drain graphic is hitting the player
    {
        Vector2 spawn = runeList[(int)debuffType].transform.position;
        Vector2 target;
        bool hit_player;
        
        RaycastHit2D[] rays = Physics2D.RaycastAll(spawn, (Vector2)Player.main.tf.position - (Vector2)runeList[(int)debuffType].transform.position, Mathf.Infinity, LayerMask.GetMask("Obstacles", "Player"));
        raysss = rays;
        RaycastHit2D hit = rays.Length > 0 ? rays[0] : new RaycastHit2D();
        float dist = rays.Length > 0 ? Vector2.Distance(hit.point, spawn) : Mathf.Infinity;
        foreach (var ray in rays)
        {
            if (!hit || Vector2.Distance(hit.point, spawn) > dist)
            {
                hit = ray;
                dist = Vector2.Distance(hit.point, spawn);
            }
        }

        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            // The ray has hit a wall
            target = hit.point;
            hit_player = false;
        }
        else
        {
            // The ray has not hit the wall
            target = Player.main.tf.position;
            hit_player = true;
        }

        drainTransform.position = spawn;
        drainTransform.localScale = new Vector3(
            1,
            Vector2.Distance(target, spawn),
            1
        );
        foreach(var graphic in drainGraphics) graphic.color = debuffColor[(int)debuffType];

        drainTransform.rotation = Quaternion.LookRotation(Vector3.forward, target - spawn);
        hitEffectTransform.rotation = Quaternion.LookRotation(Vector3.forward, spawn - target);

        hitEffectTransform.position = target;
        // hitEffectTransform.rotation = drainTransform.rotation;
        // drainTransform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle((Vector2)transform.position, target));

        drainedThisFrame = true;
        return hit_player;
    }

    void TransformToEpilogue()
    {
        BossBarManager.Instance.RemoveFromQueue(gameObject);
        BossBarManager.Instance.AddTimerToQueue(epilogueTimer, name, displayColor, displaySprite, out linkedDisplay, 50);
        animator.SetTrigger("transform");

        foreach (PrologueRuneScript prologueRune in runeList)
        {
            prologueRune.TransformToSpecial();
        }
        
        // runesRotator.SetRotationRate(360);
    }

    public AOEAttackRune CreateAOERune()
    {
        return Instantiate(AOEPrefab, transform);
    }

    public GameObject GetInstantiate(GameObject prefab, Transform transform){
        return Instantiate(prefab, transform);
    }

    protected override void OnUpdate()
    {
        if (fireTimer <= 0)
        {
            // Shoot rune
            PrologueRuneProjectile projectile = GetNewRuneProjectile();
            Vector2 target = (Vector2)Player.main.tf.position + (Vector2)Player.main.rb.velocity * projectile.seekTime;
            projectile.StartSeek(transform.position, target, DebuffType.SPEED);

            fireTimer = fireInterval;
        }

        rb_self.velocity = (Player.main.tf.position - transform.position).normalized * (maxSpeed * 1 - (health.CurrentHealth / health.maxHealth)) * Time.deltaTime;
        fireTimer -= Time.deltaTime;
        
    }

    protected override void OnLateUpdate()
    {
        hitEffectTransform.gameObject.SetActive(drainedThisFrame);
        drainedThisFrame = false;
    }

    public override void DeathEvent(bool to_player = false)
    {
        health.SetImmortal(true);

        // Play transform animation
        // runesRotator.SetRotationRate(0, 1.5f);

        Invoke(nameof(TransformToEpilogue), 2f);
    }
}
