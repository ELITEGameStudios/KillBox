using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuskBoss : BossBase
{
    [Header("Shoot Sources")]
    public Transform staffTf;
    public Transform bodyTf;
    public Transform mainBodyTf;
    public FixedRotator rotator;
    public AIShooterScript[] mainShootSources;

    [Header("Death Handler")]
    public bool isSolo;
    public bool isHalf;
    public DawnBoss dawnScript;

    [Header("States")]
    public DuskMainAttackState mainAttack, halfHpAttack, enragedMain;
    public DuskOtherAttackState retargetAttack, barrageAttack, cometAttack;
    public DuskDashState dashAttack;
    public DuskCannonState cannonAttack;
    public DuskWaveState waveAttack;
    public Phase firstPhase, secondPhase, enragedPhase;
    // Start is called before the first frame update
    void Awake()
    {
        // this,  stateTime,  speed,  force, distance, fireRate, init
        mainAttack = new DuskMainAttackState(this, 8f, 5f, 1000f, 7.5f, 1.6f, 1.8f);
        retargetAttack = new DuskOtherAttackState(this, 9f, 3f, 1000f, 8f, 1.8f, 2f, 2);
        barrageAttack = new DuskOtherAttackState(this, 3f, 5f, 1000f, 7.5f, 2f, 2f, 3);
        cometAttack = new DuskOtherAttackState(this, 10f, 7f, 1500f, 8.5f, 2f, 1.5f, 4);
        dashAttack = new DuskDashState(this, 14f, 2000f, 8f, 8000f, 0.2f, 1f);

        halfHpAttack = new DuskMainAttackState(this, 8f, 7f, 1500f, 8.5f, 1.4f, 1.8f);
        cannonAttack = new DuskCannonState(this, 12f, 3f, 1000f, 9f, 1.8f, 1.8f, 0.86f, 0.35f);
        waveAttack = new DuskWaveState(this, 2f, 8f, 3000f, 9f, 0.08f);

        enragedMain = new DuskMainAttackState(this, 8f, 9f, 1500f, 8.5f, 0.4f, 1.2f);

        firstPhase.statesInPhase = new BossStateData[] {mainAttack, dashAttack, dashAttack, retargetAttack, cometAttack, barrageAttack};
        firstPhase.minHealth = 0.5f;

        secondPhase.statesInPhase = new BossStateData[] {cannonAttack, halfHpAttack, waveAttack, waveAttack, retargetAttack,  barrageAttack, cometAttack, 
        dashAttack, dashAttack, dashAttack};
        secondPhase.minHealth = -0.5f;

        enragedPhase.statesInPhase = new BossStateData[] {enragedMain};
        enragedPhase.minHealth = -1f;



        phases = new Phase[3]{firstPhase, secondPhase, enragedPhase};
    }
    public override void DeathEvent(bool to_player = false){
        if (!isSolo){
            dawnScript.isSolo = true;
            dawnScript.SetPhase(dawnScript.enragedPhase);
        }
    }

    protected override void OnSetPhase(){
        if (!isSolo && isHalf){
            dawnScript.SetPhase(dawnScript.secondPhase);
        }
        isHalf = true;
    }

}
