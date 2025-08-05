using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaTriadBoss : BossBase
{
    public AIShooterScript[] shoot_sources;
    public AIShooterScript[] wallShootSources;
    public AIShooterScript[] deltaSources;
    public AlphaTriadBurst burstState;
    public AlphaTriadWall wallState;
    public AlphaTriadDelta deltaState;
    public Phase firstPhase;
    // Start is called before the first frame update
    void Awake(){

    // this,  stateTime,  speed,  force,  fireRate
    burstState = new AlphaTriadBurst(this, 8f, 1.2f, 100, 1.5f);
    wallState = new AlphaTriadWall(this, 10f, 10f, 2000, 5.5f, 3f, 0.8f);
    deltaState = new AlphaTriadDelta(this, 6f, 1f, 100, 0.3f);

    firstPhase.statesInPhase = new BossStateData[] {deltaState, burstState, wallState};
    firstPhase.minHealth = 0f;

    phases = new Phase[1]{firstPhase};
    }
}
