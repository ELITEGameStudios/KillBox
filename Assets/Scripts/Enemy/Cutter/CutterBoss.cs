using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CutterBoss : BossBase
{

    public static List<CutterBoss> bosses;
    public bool multipleBosses {get { return bosses.Count > 1; }}
    public bool forMultiple;


    public SpriteRenderer renderer;
    public Color defaultColor;

    [Header("GameObjects")]
    public GameObject laser;
    public GameObject bomb_prefab, beam_indicator_prefab, dashParticlesA, dashParticlesB;

    [Header("StateMachine")]
    [SerializeField] private Phase phase1;
    [SerializeField] private Phase phase2;
    [SerializeField] private Phase phase3;
    [SerializeField] private Phase multiplayerPhase;


    [SerializeField] private CutterChaseAttack chase, bombChase;
    [SerializeField] private CutterDashAction dash, agroDash, insaneDash;
    [SerializeField] private CutterLaserAttack laserAttack;
    [SerializeField] private CutterLaserAttack insaneLaserAttack;
    [SerializeField] private CutterBombAttack bombAttack;

    [SerializeField] private CutterLaserAttack multiplayerLaserAttack;
    [SerializeField] private CutterChaseAttack multiplayerChaseAttack;


    [Header("Audio")]
    public AudioClip clip;
    public AudioSource audio, dashAudio;



    // [SerializeField] int chainCount, chainCooldown;
    // [SerializeField] public bool chaining;

    void Awake()
    {
        if(bosses == null){ bosses = new List<CutterBoss>(); }

        bossType = BossRoundManager.BossType.CUTTER;


        chase = new CutterChaseAttack(this, 6, time: 5, bombIterations: 0);
        bombChase = new CutterChaseAttack(this, 5);

        laserAttack = new CutterLaserAttack(this);
        insaneLaserAttack = new CutterLaserAttack(this, iterations: 3, laserWindupTime: 1f, laserDiminishTime: 1f);
        bombAttack = new CutterBombAttack(this);

        dash = new CutterDashAction(this, 1, 0, instantDash: true);
        agroDash = new CutterDashAction(this, 2, 0.75f, false);
        insaneDash = new CutterDashAction(this, 3, 0.5f, true); // should be random between 2 and up to 5

        multiplayerChaseAttack = new CutterChaseAttack(this, 6, time: 6, bombIterations: 0);
        multiplayerLaserAttack = new CutterLaserAttack(this, iterations: 4, laserWindupTime: 1f, laserDiminishTime: 1f);

        phase1.statesInPhase = new BossStateData[] { chase, bombAttack, laserAttack, bombChase, dash };
        phase1.minHealth = 0.65f;

        multiplayerPhase.statesInPhase = new BossStateData[] { multiplayerChaseAttack, multiplayerLaserAttack, dash };
        multiplayerPhase.minHealth = 0f;

        phase2.statesInPhase = new BossStateData[] { dash, bombAttack, laserAttack };
        phase2.minHealth = 0.4f;

        phase3.statesInPhase = new BossStateData[] { bombChase, bombAttack, insaneDash, insaneLaserAttack, insaneDash };
        phase3.minHealth = 0f;

        if (forMultiple){
            phases = new Phase[] { multiplayerPhase };
        }
        else{
            phases = new Phase[] { phase1, phase2, phase3 };
        }

        // Taken from Start function in legacy script
        defaultColor = renderer.color;
        dashParticlesA.transform.SetParent(null);
        dashParticlesB.transform.SetParent(null);

        dashParticlesA.SetActive(false);
        dashParticlesB.SetActive(false);

        bosses.Add(this);
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        //Vector3 new_direction = Vector3.RotateTowards(laser.transform.forward, player.position - laser.transform.position, 6.28319f, 0.0f);
        //laser.transform.rotation = Quaternion.LookRotation(new_direction);
    }

    void Start()
    {
        if (phases.Length > 0)
        {
            SetPhase(phases[0]);
            PhaseCheck();
            ChooseNextState();
        }

        health.SetDeathHandler(this);
    }

    protected override void ChooseNextState()
    {
        if (!forMultiple)
        {
            // if (phase3.minHealth == currentPhase.minHealth) //phase 3
            // {
            // if (currentState == laserAttack)
            // {
            //     SetState(Random.Range(0, 2) == 0 ? laserAttack : agroDash, 0);
            //     return;
            // }
            // if (currentState == insaneDash)
            // {
            //     SetState(Random.Range(0, 2) == 0 ? laserAttack : bombChase, 1);
            //     return;
            // }
            // }

            base.ChooseNextState();
            return;
        }

        // Multiple boss behaviour
        if (!startedAttacks)
        {
            bool hasLaser = false;
            for (int i = 0; i < bosses.Count; i++)
            {
                if (bosses[i] == this) { continue; }
                if (bosses[i].currentState == bosses[i].multiplayerLaserAttack)
                {
                    hasLaser = true;
                    break;
                }

            }

            if (hasLaser)
            {
                SetState(multiplayerChaseAttack, 1);
            }
            else
            {
                SetState(multiplayerLaserAttack, 0);
            }
            return;
        }

        base.ChooseNextState();
        

    }


    public IEnumerator LaserAttackNumerator()
    {
        if (currentState is CutterLaserAttack) yield return (currentState as CutterLaserAttack).LaserAttackNumerator();
    }

    public IEnumerator ChaseWithBombNumerator()
    {
        if (currentState is CutterChaseAttack) yield return (currentState as CutterChaseAttack).ChaseWithBombNumerator();
    }
    
    public IEnumerator DashAttackNumerator()
    {
        if(currentState is CutterDashAction) yield return (currentState as CutterDashAction).DashAttackNumerator();
    }
    
    public override void DeathEvent(bool to_player = false)
    {
        bosses.Remove(this);
        Debug.Log("Cutter Removed");
        
        base.DeathEvent(to_player);
    }

    IEnumerator BomberAttackNumerator()
    {
        yield return new WaitForSeconds(0.6f);
        // PickState(1);
    }
}   
    