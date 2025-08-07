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
    public DuskOtherAttackState retargetAttack, barrageAttack, cometAttack;
    public DuskDashState dashAttack;
    public DuskCannonState cannonAttack, cannonAttackLong;
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
        dashAttack = new DuskDashState(this, 14f, 2000f, 8f, 8000f, 0.2f, 1.7f);

        halfHpAttack = new DuskMainAttackState(this, 8f, 7f, 1500f, 8.5f, 1.4f, 1.8f);
        cannonAttack = new DuskCannonState(this, 7f, 3f, 1000f, 9f, 1.8f, 1.8f, 0.86f, 0.35f);
        cannonAttackLong = new DuskCannonState(this, 12f, 3f, 1000f, 9f, 1.8f, 1.8f, 0.86f, 0.35f);
        waveAttack = new DuskWaveState(this, 2f, 8f, 3000f, 9f, 0.08f);

        enragedMain = new DuskMainAttackState(this, 8f, 9f, 1500f, 8.5f, 0.3f, 1.2f);

        firstPhase.statesInPhase = new BossStateData[] {mainAttack, dashAttack, dashAttack, retargetAttack, cometAttack, barrageAttack};
        firstPhase.minHealth = 0.5f;

        secondPhase.statesInPhase = new BossStateData[] {cannonAttack, halfHpAttack, waveAttack, waveAttack, retargetAttack,  barrageAttack, cometAttack, cannonAttackLong,
        dashAttack, dashAttack, dashAttack, waveAttack, waveAttack, waveAttack, retargetAttack};
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
