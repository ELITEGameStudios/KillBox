using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueBoss : BossBase
{


    [Header("Rune Info")]
    public FixedRotator runesRotator;
    public GameObject[] runeList;
    public Transform[] runeParentTf;


    [Header("State Info")]
    public Phase prologuePhase;
    public Phase epiloguePhase;
    public PrologueDrainAttack healthDrain, speedDrain;
    public PrologueShootAttack shootAttack;
    public PrologueRuneLaserAttack runeLaserAttack;

    [Header("Drain Graphic")]
    public SpriteRenderer mainSquare;
    public Transform drainTransform;
    public Color[] debuffColor;
    public GameObject beamObject;


    [Header("Spawner prefabs")]
    public GameObject drainSpawn;
    public GameObject runeProjectile;
    public SweepingIndicator sweepingIndicator;
    public List<PrologueRuneProjectile> runeExplosionPool;

    [Header("Fire Rate Timers")]
    public float fireTimer;
    public float fireInterval;
    
    [Header("Animation Curves")]
    public AnimationCurve fireLerpCurve;
    public AnimationCurve beamWidthCurve;

    public enum DebuffType
    {
        HEALTH,
        SPEED,
        CAPACITY,
        LIFESTEAL
    } 

    // Start is called before the first frame update
    void Awake()
    {
        healthDrain = new PrologueDrainAttack(this, 9, DebuffType.HEALTH);
        speedDrain = new PrologueDrainAttack(this, 9, DebuffType.SPEED);
        shootAttack= new PrologueShootAttack(this, 10, 0.2f, 3, 12);
        runeLaserAttack= new PrologueRuneLaserAttack(this, 4, 2, 1, 9);

        prologuePhase.statesInPhase = new BossStateData[] { healthDrain, runeLaserAttack, shootAttack, speedDrain };
        prologuePhase.minHealth = 0.5f;

        epiloguePhase.statesInPhase = new BossStateData[] { };
        epiloguePhase.minHealth = 0f;

        phases = new Phase[2] { prologuePhase, epiloguePhase };


        fireTimer = fireInterval;
        ResetDrainGraphic();
        movement_script.enabled = false;
    }


    public PrologueRuneProjectile GetNewRuneProjectile()
    {
        PrologueRuneProjectile obj;
        foreach (PrologueRuneProjectile item in runeExplosionPool)
        {
            if (item.state == PrologueRuneProjectile.State.INACTIVE)
            {
                return item;
            }
        }

        GameObject newObj = Instantiate(runeProjectile, transform);
        newObj.transform.SetParent(null);

        PrologueRuneProjectile newProjectile = newObj.GetComponent<PrologueRuneProjectile>();
        runeExplosionPool.Add(newProjectile);
        return newProjectile;
    }

    public void ResetDrainGraphic() // Bool returns if the drain graphic is hitting the player
    {
        mainSquare.color = Color.clear;
        drainTransform.localScale = Vector3.one;
        drainTransform.rotation = transform.rotation;
    }


    public bool SetDrainGraphic(DebuffType debuffType) // Bool returns if the drain graphic is hitting the player
    {
        Vector2 target;
        bool hit_player;
        mainSquare.color = debuffColor[(int)debuffType];
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (Vector2)Player.main.tf.position - (Vector2)transform.position);
        if (ray.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            // The ray has hit a wall
            target = ray.point;
            hit_player = false;
        }
        else
        {
            // The ray has not hit the wall
            target = Player.main.tf.position;
            hit_player = true;
        }

        drainTransform.localScale = new Vector3(
            1,
            Vector2.Distance(target, transform.position),
            1
        );

        drainTransform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position);
        // drainTransform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle((Vector2)transform.position, target));
        return true;
    }


    protected override void OnUpdate()
    {
        if (fireTimer <= 0)
        {
            // Shoot rune

            fireTimer = fireInterval;
        }
        fireTimer -= Time.deltaTime;
    }
}
