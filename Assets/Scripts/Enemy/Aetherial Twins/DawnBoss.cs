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
    public int phaseInt;
    public DuskBoss duskScript;

    [Header("Enrage Handler")]
    public SpriteRenderer[] spriteArray;
    public SpriteRenderer[] glowArray;
    public Color startColor, endColor, startGlow, endGlow;
    public float timeElapsed, colorChange;

    [Header("States")]
    public DawnMainAttackState mainAttack, halfHpAttack, enragedMain;
    public DawnGravityState gravAttack, enragedGravity;
    public DawnOtherAttackState burstAttack, clusterAttack, trailAttack, meteorAttack, rainAttack, meteorAttackLong;
    public DawnBoxState boxAttack;
    public Phase firstPhase, secondPhase, enragedPhase;
    // Start is called before the first frame update
    void Awake()
    {
        bossType = BossRoundManager.BossType.DAWN;

        // this,  stateTime,  speed,  force, distance, fireRate, init
        mainAttack = new DawnMainAttackState(this, 8f, 5f, 1500f, 7.5f, 1.8f, 2f);
        gravAttack = new DawnGravityState(this, 10f, 5f, 2000f, 6f, 1.2f, 1.5f);
        clusterAttack = new DawnOtherAttackState(this, 9f, 3f, 1000f, 7.5f, 1.4f, 1.1f, 0);
        trailAttack = new DawnOtherAttackState(this, 10f, 3f, 1500f, 8f, 2.2f, 1f, 1);

        boxAttack = new DawnBoxState(this, 9f, 0.17f, 1.2f);

        halfHpAttack = new DawnMainAttackState(this, 8f, 7f, 1500f, 8.5f, 1.55f, 2f);
        meteorAttack = new DawnOtherAttackState(this, 6f, 5f, 1500f, 8.5f, 2.5f, 1f, 2);
        meteorAttackLong = new DawnOtherAttackState(this, 12f, 5f, 1500f, 8.5f, 2.5f, 1f, 2);

        enragedMain = new DawnMainAttackState(this, 8f, 9f, 1500f, 8.5f, 0.3f, 0.1f);
        enragedGravity = new DawnGravityState(this, 10f, 5f, 2500f, 6f, 0.5f, 0.1f);

        firstPhase.statesInPhase = new BossStateData[] {mainAttack, boxAttack, clusterAttack, gravAttack};
        firstPhase.minHealth = 0.5f;

        secondPhase.statesInPhase = new BossStateData[] {meteorAttack, trailAttack, halfHpAttack, gravAttack, meteorAttackLong, boxAttack, clusterAttack};
        secondPhase.minHealth = -0.5f;

        enragedPhase.statesInPhase = new BossStateData[] {enragedMain, enragedGravity};
        enragedPhase.minHealth = -1f;



        phases = new Phase[3]{firstPhase, secondPhase, enragedPhase};
        
    }
    public override void DeathEvent(bool to_player = false){
        if (!isSolo){
            duskScript.isSolo = true;
            duskScript.SetPhase(duskScript.enragedPhase);
        }

        base.DeathEvent(to_player);
    }
    protected override void OnSetPhase(){
        phaseInt ++;
        if (!isSolo && phaseInt == 2){
            duskScript.SetPhase(duskScript.secondPhase);
        }
    }
    protected override void OnUpdate(){
        if (phaseInt >= 2){
            timeElapsed += Time.deltaTime;
            foreach (SpriteRenderer body in spriteArray){
                body.color = Color.Lerp(startColor, endColor, timeElapsed * colorChange);
            }
            foreach (SpriteRenderer glow in glowArray){
                glow.color = Color.Lerp(startGlow, endGlow, timeElapsed * colorChange);
            }
        }
    }

}
