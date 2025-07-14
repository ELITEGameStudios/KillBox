using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoss : BossBase
{
    public AIShooterScript[] shoot_sources;

    public DebugShootState shootState, shotgunState, dashState, teleportShoot, enragedShoot, enragedBurst;

    public Phase firstPhase, finalPhase;
    void Awake()
    {
        // Initialize new states.
        // DebugShootStates has this, stateTime, speed, accel, fireRate, min, max, isForce, isShotgun, canShoot, canRotate
        shootState = new DebugShootState(this, 6, 1.5f, 0, 1.2f, 5f, 6f);
        shotgunState = new DebugShootState(this, 8, 3f, 200, 1.8f, 6f, 7f, isForce : true, isShotgun : true);
        dashState = new DebugShootState(this, 1.5f, 18, 1000, 3f, 9f, 9f, isForce : true, canShoot : false, canRotate : false);

        teleportShoot = new DebugShootState(this, 0.5f, 0, 0, 0.3f, 6f, 7f, canRotate : false);
        enragedShoot = new DebugShootState(this, 7, 3f, 400, 1f, 5f, 6f, isForce : true);
        enragedBurst = new DebugShootState(this, 9, 4f, 300, 1.5f, 6.5f, 7.5f, isForce : true, isShotgun : true);

        // Adds the states and the minimum health of each phase.
        firstPhase.statesInPhase = new BossStateData[] {shootState, shotgunState, dashState, dashState};
        firstPhase.minHealth = 0.7f;

        finalPhase.statesInPhase = new BossStateData[] {teleportShoot, teleportShoot, teleportShoot,enragedShoot, enragedBurst, dashState, dashState, dashState, dashState,
        enragedBurst, enragedShoot, teleportShoot, teleportShoot, teleportShoot, teleportShoot, teleportShoot, teleportShoot, enragedBurst};
        finalPhase.minHealth = 0f;

        // Adds Boss phases
        phases = new Phase[2] {firstPhase, finalPhase};
    }
}
