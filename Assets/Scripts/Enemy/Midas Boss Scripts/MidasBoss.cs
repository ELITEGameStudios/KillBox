using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasBoss : BossBase
{
    public AIShooterScript[] crownShooter, crownFallShooter, otherShooter, alterShooter;
    public ParticleSystem ramParticles, ramFinished;
    public Animator midasAnimate;
    public GameObject corridorSummon, corridor;

    [Header("States")]
    public MidasShootState crownShoot, nerfedShoot;
    public MidasRamState ramAttack;
    public MidasFallState crownFall;
    public MidasOtherState wallAttack;
    public MidasCorridorState corridorAttack;
    public MidasAlterState bannerAttack, nerfedBanner;
    public MidasGuillotineState guillotineAttack;

    public Phase firstPhase, secondPhase, thirdPhase, finalPhase;

    void Awake(){
        
        bossType = BossRoundManager.BossType.MIDAS;

        // this,  stateTime,  speed,  force, distance, fireRate, init
        crownShoot = new MidasShootState(this, 7f, 3f, 3000f, 5f, 1f, 1.5f);
        nerfedShoot = new MidasShootState(this, 10f, 2f, 3000f, 5f, 1.4f, 2f);
        ramAttack = new MidasRamState(this, 14f, 1700f, 0.6f);
        crownFall = new MidasFallState(this, 6f, 2f, 3000f, 6f, 0.8f, 1.5f);
        wallAttack = new MidasOtherState(this, 8.5f, 2f, 3000f, 5f, 1.3f, 1.5f, 0);
        corridorAttack = new MidasCorridorState(this);
        bannerAttack = new MidasAlterState(this, 7f, 2f, 3000f, 5f, 1.4f, 1.5f, 0);
        nerfedBanner = new MidasAlterState(this, 10f, 1.5f, 3000f, 5f, 1.9f, 1.8f, 0);
        guillotineAttack = new MidasGuillotineState(this, 4f, 3000f, 5.5f, 12f, 1);

        firstPhase.statesInPhase = new BossStateData[] {crownShoot, ramAttack, ramAttack, ramAttack, crownFall};
        firstPhase.minHealth = 0.85f;

        secondPhase.statesInPhase = new BossStateData[] {wallAttack, ramAttack, ramAttack, crownFall, corridorAttack, nerfedShoot, crownFall};
        secondPhase.minHealth = 0.55f;

        thirdPhase.statesInPhase = new BossStateData[] {bannerAttack, crownFall, ramAttack, ramAttack, ramAttack, corridorAttack, nerfedBanner, ramAttack, ramAttack, wallAttack};
        thirdPhase.minHealth = 0.2f;

        finalPhase.statesInPhase = new BossStateData[] {guillotineAttack};
        finalPhase.minHealth = 0f;

        phases = new Phase[4]{firstPhase, secondPhase, thirdPhase, finalPhase};

    }
}