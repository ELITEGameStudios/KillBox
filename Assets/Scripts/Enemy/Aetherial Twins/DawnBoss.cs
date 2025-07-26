using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawnBoss : BossBase
{
    [Header("Shoot Sources")]
    public AIShooterScript[] fiveArrow, fourArrow, gravArrowUp, gravArrowDown, otherShootSources;
    public Transform bowTf;
    public Transform bodyTf;
    public Transform mainBodyTf;

    [Header("Death Handler")]
    public bool isSolo;
    public bool isHalf;
    public DuskBoss duskScript;

    [Header("States")]
    public DawnMainAttackState mainAttack, halfHpAttack, enragedMain;
    public DawnGravityState gravAttack;
    public DawnOtherAttackState burstAttack, clusterAttack, trailAttack, meteorAttack, rainAttack;
    public Phase firstPhase, secondPhase, enragedPhase;
    // Start is called before the first frame update
    void Awake()
    {
        // this,  stateTime,  speed,  force, distance, fireRate, init
        mainAttack = new DawnMainAttackState(this, 8f, 5f, 1500f, 7.5f, 1.9f, 2f);
        gravAttack = new DawnGravityState(this, 12f, 4.5f, 1000f, 6f, 2.5f, 2.7f, 1.5f);
        burstAttack = new DawnOtherAttackState(this, 10f, 5f, 1500f, 8f, 3f, 1f, 0);
        clusterAttack = new DawnOtherAttackState(this, 8f, 3f, 1000f, 7.5f, 1.5f, 1.3f, 1);
        trailAttack = new DawnOtherAttackState(this, 10f, 3f, 1500f, 8f, 2.2f, 1f, 2);

        halfHpAttack = new DawnMainAttackState(this, 8f, 7f, 1500f, 8.5f, 1.65f, 2f);
        meteorAttack = new DawnOtherAttackState(this, 10f, 5f, 1500f, 8.5f, 2.5f, 1f, 3);
        rainAttack = new DawnOtherAttackState(this, 10f, 3f, 1000f, 7.5f, 0.5f, 0.9f, 4);

        enragedMain = new DawnMainAttackState(this, 8f, 9f, 1500f, 8.5f, 0.5f, 1.5f);

        firstPhase.statesInPhase = new BossStateData[] {mainAttack, gravAttack, mainAttack, burstAttack, clusterAttack, trailAttack};
        firstPhase.minHealth = 0.5f;

        secondPhase.statesInPhase = new BossStateData[] {meteorAttack, halfHpAttack, rainAttack, gravAttack, burstAttack, halfHpAttack, trailAttack, clusterAttack};
        secondPhase.minHealth = -0.5f;

        enragedPhase.statesInPhase = new BossStateData[] {enragedMain};
        enragedPhase.minHealth = -1f;



        phases = new Phase[3]{firstPhase, secondPhase, enragedPhase};
        
    }
    public override void DeathEvent(bool to_player = false){
        if (!isSolo){
            duskScript.isSolo = true;
            duskScript.SetPhase(duskScript.enragedPhase);
        }
    }
    protected override void OnSetPhase(){
        if (!isSolo && isHalf){
            duskScript.SetPhase(duskScript.secondPhase);
        }
        isHalf = true;
    }

}
