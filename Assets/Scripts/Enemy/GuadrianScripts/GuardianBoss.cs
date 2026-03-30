using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GuardianBoss : BossBase
{

    public bool whipIsActive {get; private set;}
    public bool isStalled {get; private set;}

    [Header("Ported guardian variables")]
    public AIShooterScript[] shoot_sources;
    public float chase_time;
    public GuardianBeam[] beams;
    public Collider2D[] whipColliders;
    public SpriteMask whipMask;
    public FixedRotator rotator;

    // public Transform player;
    // public Vector3 locked_rotation;
    // public Vector2 normalScale, warpedScale;

    [Header("State Variables")]

    public GuardianBeamState beamState, angryBeamState;
    public GuardianMainState spiralShoot, chaosShoot, chaseState, whipState, angryWhipState;
    public GuardianStateData[] firstPhaseStates, secondPhaseStates, finalPhaseStates;
    public AnimationCurve beamPositionLerpAnimCurve;

    public static List<GuardianBoss> bosses;
    public bool multipleBosses {get { return bosses.Count > 1; }}

    // Start is called before the first frame update
    void Awake()
    {
        bossType = BossRoundManager.BossType.GUARDIANS;

        if(bosses == null){ bosses = new List<GuardianBoss>(); }

        spiralShoot = new GuardianMainState(this, "SPIRAL", 1, 2, targetTime: 4, rotationSpeed: 45, maxEntities: 1);
        chaosShoot = new GuardianMainState(this, "CHAOS", 3, 2, targetTime: 4, rotationSpeed: -320, maxEntities: 1);
        chaseState = new GuardianMainState(this, "CHASE", 12, 12, targetTime: chase_time, rotationSpeed: 70, startShootingTime: 5, maxEntities: -1);
        whipState = new GuardianMainState(this, "WHIP", 25, 8, targetTime: 7, rotationSpeed: -320, startShootingTime: 7, whip: true, maxEntities: 1);
        beamState = new GuardianBeamState(this, "BEAM", iterations: 4, targetTime: 2, rotationSpeed: 15, width:4, beamDistance: 10, maxEntities: 1);
        angryBeamState = new GuardianBeamState(this, "ANGRYBEAM", iterations: 8, targetTime: 1.4f, rotationSpeed: 15, width:2.5f, beamDistance: 7, maxEntities: 2);

        angryWhipState = new GuardianMainState(this, "WHIP", 8, 8, targetTime: 7, rotationSpeed: -320, startShootingTime: 1, whip: true, maxEntities: 2);
        firstPhaseStates = new GuardianStateData[]{ chaseState, spiralShoot, beamState, whipState };
        secondPhaseStates = new GuardianStateData[]{ chaseState, spiralShoot, chaosShoot, beamState, whipState };
        finalPhaseStates = new GuardianStateData[]{ spiralShoot, angryWhipState, angryBeamState};
        fallbackState = chaseState;

        // BossAudio.Instance.OnShardSpawn(gameObject);

        bosses.Add(this);
    }

    protected override void OnStart(){
        ChooseNextState();
        health.SetDeathHandler(this);
    }

    protected override void ChooseNextState()
    {
        // if (!multipleBosses) { base.ChooseNextState(); return; }
        GuardianStateData candidateState;
        GuardianStateData[] phaseStates;
        
        // Choose main state list
        if(!BossRoundManager.main.spawnSystem.allBossesHaveSpawned || bosses.Count == 3){ phaseStates = firstPhaseStates; }
        else if(bosses.Count == 2){ phaseStates = secondPhaseStates; }
        else { phaseStates = finalPhaseStates; }

        // handling special transition cases from prior states
        if(currentState == beamState) { candidateState = Random.Range(0, 2) == 1 ? chaseState : spiralShoot; }
        else if(currentState == whipState && bosses.Count < 2)  { candidateState = chaseState; }

        // Choose randomly from the current state list
        else { candidateState = phaseStates[Random.Range(1, phaseStates.Length)]; }

        while (true)
        {
            if(bosses.Count < 2  || candidateState.maxEntitiesSharingState == -1){
                SetState(candidateState);
                return;
            }
            else
            {
                int bossesSharingCandadite = 0;
                bool hasBeam = false;
                bool hasSpiral = false;

                foreach (GuardianBoss boss in bosses)
                {
                    if(boss == this){continue;}
                    if(!boss.startedAttacks) continue;
                    GuardianStateData bossStateData = boss.currentState as GuardianStateData;
                    if(bossStateData.stateTag == candidateState.stateTag){
                        bossesSharingCandadite++;
                    }

                    if(bossStateData.stateTag == "BEAM"){hasBeam = true;}
                    if(bossStateData.stateTag == "SPIRAL"){hasSpiral = true;}
                }
                if(bossesSharingCandadite >= candidateState.maxEntitiesSharingState)
                {
                    switch (candidateState.stateTag)
                    {
                        case "WHIP":
                            candidateState = chaseState;
                            break;
                        case "SPIRAL":
                            candidateState = whipState;
                            break;
                        case "BEAM":
                            candidateState = spiralShoot;
                            break;
                        default:
                            candidateState = chaseState;
                            break;

                        // Loops again
                        // Chase state has maxEntitiesSharingRate at -1 and therefore will break the loop no matter what
                    }
                }
                else
                {
                    if(( hasBeam && candidateState == spiralShoot) || (hasSpiral && candidateState == beamState))
                    {
                        candidateState = whipState;
                        continue;
                    }
                    SetState(candidateState);
                    return;
                }
            }
        }
    }

    public override void DeathEvent(bool to_player = false)
    {
        bosses.Remove(this);
        Debug.Log("Guardian Removed");
        if(bosses.Count == 1)
        {
            if(bosses[0].health.CurrentHealth / bosses[0].health.maxHealth < 0.5f)
            bosses[0].health.CurrentHealth = (int)(bosses[0].health.maxHealth / 2f);
        }
        base.DeathEvent(to_player);
    }
    public void HitPlayerWithWhip(){
        if(whipIsActive){
            // isStalled = true;
            // movement_script.maxSpeed = 1.5f;
            // stallTime = 0.1f;
            // float currentRotation = rotator.speed;
            // rotator.SetRotationRate(-45);
            // rotator.SetRotationRate(currentRotation, 0.1f);
        }
    } 


    public IEnumerator WhipAttack(float time)
    {
        whipMask.gameObject.SetActive(true);

        while (whipMask.alphaCutoff > 0){
            whipMask.alphaCutoff -= Time.deltaTime;
            yield return null;  }

        whipMask.alphaCutoff = 0;
        whipIsActive = true;

        foreach (Collider2D col in whipColliders){ col.enabled = true; }
        yield return new WaitForSeconds(time);
        

        while (whipMask.alphaCutoff < 1){
            whipMask.alphaCutoff += Time.deltaTime; 
            if(whipMask.alphaCutoff > 0.5f && whipIsActive)
                whipIsActive = false;
                foreach (Collider2D col in whipColliders){ col.enabled = false; }
            yield return null; }

        whipMask.alphaCutoff = 1;
        whipMask.gameObject.SetActive(false);
    }
}
