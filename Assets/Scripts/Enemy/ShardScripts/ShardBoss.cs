using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardBoss : BossBase
{
    public static List<ShardBoss> bosses;
    public bool multipleBosses {get { return bosses.Count > 1; }}


    public AIShooterScript[] shoot_sources;
    public float current_chase_time, chase_time, clone_distance;
    public float[] Adrag, Aspeed, Rspeed;
    public Vector3 locked_rotation;
    public FixedRotator rotator;


    public ShardChaseState chaseState, agroChaseState;
    public ShardSpiralAttackState spiralState;
    public ShardLeaveState leaveState, agroLeaveState;
    public ShardSphericalAttack sphereAttack, agroSphereAttack;
    public ShardDashAttack dashAttack, agroDashAttack;

    public Phase firstPhase, lastPhase;

    public AnimationCurve enterCurve;


    // Start is called before the first frame update
    void Awake()
    {
        if(bosses == null){ bosses = new List<ShardBoss>(); }
        chaseState = new ShardChaseState(this, 12, 12, 8, Adrag[0], Rspeed[0]);
        agroChaseState = new ShardChaseState(this, 8, 3.5f, 7, Adrag[2], Rspeed[2], fires: true);

        spiralState = new ShardSpiralAttackState(this, 2, 2, Adrag[1], Rspeed[1]);

        leaveState = new ShardLeaveState(this, 250, 35);
        agroLeaveState = new ShardLeaveState(this, 750, 35);

        sphereAttack = new ShardSphericalAttack(this, 90, 20, 8, enterCurve, 2, 7, 2, 0.5f);
        agroSphereAttack = new ShardSphericalAttack(this, 110, 20, 9, enterCurve, 2, 7, 2, 0.4f);

        dashAttack = new ShardDashAttack(this, 6, 25, 0.4f, 4);
        agroDashAttack = new ShardDashAttack(this, 5, 25, 0.3f, 4);



        firstPhase.statesInPhase = new BossStateData[] { chaseState, leaveState, dashAttack, chaseState, spiralState };
        firstPhase.minHealth = 0.5f;

        lastPhase.statesInPhase = new BossStateData[] { agroChaseState, spiralState, agroLeaveState, agroDashAttack };
        lastPhase.minHealth = 0f;

        phases = new Phase[2] { firstPhase, lastPhase };
        SetPhase(phases[0]);

        //BossAudio.Instance.OnShardSpawn(gameObject);

        bosses.Add(this);
    }

    void Start(){
        ChooseNextState();
        health.SetDeathHandler(this);
    }

    protected override void ChooseNextState()
    {
        if (!multipleBosses) { base.ChooseNextState(); return; }

        // bool chaserExists = false;
        bool dasherExists = false;
        bool spiralExists = false;

        for (int i = 0; i < bosses.Count; i++)
        {
            ShardBoss boss = bosses[i];
            if (boss == this) { continue; }

            if (boss.currentState ==  boss.dashAttack || boss.currentState == boss.agroDashAttack) { dasherExists = true; }
            if (boss.currentState == boss.spiralState) { spiralExists = true; }
        }

        if (!spiralExists) { SetState(spiralState, 0); return; }
        if (!dasherExists) { SetState(currentPhase.statesInPhase == firstPhase.statesInPhase ? dashAttack : agroDashAttack, 0); return; }
        SetState(currentPhase.statesInPhase == firstPhase.statesInPhase ? chaseState : agroChaseState, 0);
        // SetState(chaseState, 0);
    }

    public override void DeathEvent(bool to_player = false)
    {
        bosses.Remove(this);
        Debug.Log("Shard Removed");
        base.DeathEvent(to_player);
    }
}
