using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CutterBoss : BossBase
{
    public SpriteRenderer renderer;
    public Color defaultColor;

    [Header("GameObjects")]
    public GameObject laser;
    public GameObject bomb_prefab, beam_indicator_prefab, dashParticlesA, dashParticlesB;

    [Header("StateMachine")]
    [SerializeField] private Phase phase1;
    [SerializeField] private Phase phase2;
    [SerializeField] private Phase phase3;


    [SerializeField] private CutterChaseAttack chase, bombChase;
    [SerializeField] private CutterDashAction dash, agroDash, insaneDash;
    [SerializeField] private CutterLaserAttack laserAttack;
    [SerializeField] private CutterBombAttack bombAttack;


    [Header("Audio")]
    public AudioClip clip;
    public AudioSource audio, dashAudio;



    // [SerializeField] int chainCount, chainCooldown;
    // [SerializeField] public bool chaining;

    void Awake()
    {
        chase = new CutterChaseAttack(this, 6);
        bombChase = new CutterChaseAttack(this, 5, time: 5, bombIterations: 0);

        laserAttack = new CutterLaserAttack(this);
        bombAttack = new CutterBombAttack(this);

        dash = new CutterDashAction(this, 1, 0.6f, false);
        agroDash = new CutterDashAction(this, 2, 0.75f, false);
        insaneDash = new CutterDashAction(this, 3, 0.5f, true); // should be random between 2 and up to 5

        phase1.statesInPhase = new BossStateData[] { chase, bombAttack, laserAttack, bombChase, dash };
        phase1.minHealth = 0.65f;

        phase2.statesInPhase = new BossStateData[] { dash, bombAttack, laserAttack };
        phase2.minHealth = 0.4f;

        phase3.statesInPhase = new BossStateData[] { bombChase, bombAttack, insaneDash, laserAttack, agroDash };
        phase3.minHealth = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {

        defaultColor = renderer.color;

        //BossAudio.Instance.OnShardSpawn(gameObject);
        BossBarManager.Instance.AddToQueue(gameObject, name, displayColor, displaySprite);

        dashParticlesA.transform.SetParent(null);
        dashParticlesB.transform.SetParent(null);

        dashParticlesA.SetActive(false);
        dashParticlesB.SetActive(false);

    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        //Vector3 new_direction = Vector3.RotateTowards(laser.transform.forward, player.position - laser.transform.position, 6.28319f, 0.0f);
        //laser.transform.rotation = Quaternion.LookRotation(new_direction);
    }

    protected override void ChooseNextState()
    {

        
        if (phase3.minHealth == currentPhase.minHealth) //phase 3
        {
            if (currentState == laserAttack)
            {
                SetState(Random.Range(0, 2) == 0 ? laserAttack : agroDash, 0);
                return;
            }
            if (currentState == insaneDash)
            {
                SetState(Random.Range(0, 2) == 0 ? laserAttack : bombChase, 1);
                return;
            }
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

    IEnumerator BomberAttackNumerator()
    {
        yield return null;
        PickState(1);
    }
}   
    