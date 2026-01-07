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
    public int phaseInt;
    public DawnBoss dawnScript;

    [Header("Enrage Handler")]
    public SpriteRenderer[] spriteArray;
    public SpriteRenderer[] glowArray;
    public Color startColor, endColor, startGlow, endGlow;
    public float timeElapsed, colorChange;

    [Header("States")]
    public DuskMainAttackState mainAttack, halfHpAttack, enragedMain;
    public DuskOtherAttackState retargetAttack, barrageAttack, cometAttack, enragedRetarget;
    public DuskDashState dashAttack;
    public DuskCannonState cannonAttack, cannonAttackLong;
    public DuskWaveState waveAttack;
    public DuskBoxState boxAttack;
    public DuskChaseState chaseAttack;
    public Phase firstPhase, secondPhase, enragedPhase;
    // Start is called before the first frame update
    void Awake()
    {
        bossType = BossRoundManager.BossType.TWINS;

        // this,  stateTime,  speed,  force, distance, fireRate, init
        mainAttack = new DuskMainAttackState(this, 8f, 5f, 1000f, 7.5f, 1.55f, 1.8f);
        retargetAttack = new DuskOtherAttackState(this, 9f, 3f, 1000f, 8f, 1.7f, 2f, 2);
        boxAttack = new DuskBoxState(this, 9f, 0.1f, 1.2f);
        waveAttack = new DuskWaveState(this, 10f, 12f, 5000f, 4f, 0.15f);

        halfHpAttack = new DuskMainAttackState(this, 8f, 7f, 1500f, 8.5f, 1.3f, 1.8f);
        cannonAttack = new DuskCannonState(this, 6f, 3f, 1000f, 9f, 1.8f, 1.8f, 0.95f, 0.45f);
        cannonAttackLong = new DuskCannonState(this, 12f, 3f, 1000f, 9f, 1.8f, 1.8f, 0.95f, 0.45f);
        chaseAttack = new DuskChaseState(this, 10f, 8f, 4000f);

        enragedMain = new DuskMainAttackState(this, 8f, 9f, 1500f, 8.5f, 0.2f, 0.15f);
        enragedRetarget = new DuskOtherAttackState(this, 10f, 9f, 1500f, 8f, 0.35f, 0.1f, 2);

        firstPhase.statesInPhase = new BossStateData[] {mainAttack, boxAttack, retargetAttack, waveAttack};
        firstPhase.minHealth = 0.5f;

        secondPhase.statesInPhase = new BossStateData[] {cannonAttack, chaseAttack, halfHpAttack, waveAttack, cannonAttackLong, boxAttack, retargetAttack};
        secondPhase.minHealth = -0.5f;

        enragedPhase.statesInPhase = new BossStateData[] {enragedMain, enragedRetarget};
        enragedPhase.minHealth = -1f;



        phases = new Phase[3]{firstPhase, secondPhase, enragedPhase};
    }
    public override void DeathEvent(bool to_player = false){
        if (!isSolo){
            dawnScript.isSolo = true;
            dawnScript.SetPhase(dawnScript.enragedPhase);
        }

        base.DeathEvent(to_player);
    }

    protected override void OnSetPhase(){
        phaseInt ++;
        if (!isSolo && phaseInt == 2){
            dawnScript.SetPhase(dawnScript.secondPhase);
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
