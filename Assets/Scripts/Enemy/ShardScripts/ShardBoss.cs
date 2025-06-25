using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardBoss : BossBase
{


    public AIShooterScript[] shoot_sources;
    public float current_chase_time, chase_time, clone_distance;
    public float[] Adrag, Aspeed, Rspeed;
    public Vector3 locked_rotation;
    public FixedRotator rotator;

    public ShardChaseState chaseState, agroChaseState;
    public ShardSpiralAttackState spiralState;
    public ShardLeaveState leaveState;
    public ShardSphericalAttack sphereAttack;
    public ShardDashAttack dashAttack;
    
    public Phase firstPhase, lastPhase;

    public AnimationCurve enterCurve;


    // Start is called before the first frame update
    void Awake()
    {
        chaseState = new ShardChaseState(this, 12, 12, 8, Adrag[0], Rspeed[0]);
        agroChaseState = new ShardChaseState(this, 5, 3.5f, 11, Adrag[2], Rspeed[2]);
        spiralState = new ShardSpiralAttackState(this, 2, 2, Adrag[1], Rspeed[1]);
        leaveState = new ShardLeaveState(this, 250, 35);
        sphereAttack = new ShardSphericalAttack(this, 90, 20, 8, enterCurve, 2, 7, 2, 0.5f);
        dashAttack = new ShardDashAttack(this, 6, 25, 0.4f, 4);


        firstPhase.statesInPhase = new BossStateData[] { chaseState, leaveState, dashAttack, sphereAttack, chaseState, spiralState};
        firstPhase.minHealth = 0.5f;

        lastPhase.statesInPhase = new BossStateData[] { agroChaseState, spiralState };
        lastPhase.minHealth = 0f;

        phases = new Phase[2] { firstPhase, lastPhase };
        //BossAudio.Instance.OnShardSpawn(gameObject);

    }
}
