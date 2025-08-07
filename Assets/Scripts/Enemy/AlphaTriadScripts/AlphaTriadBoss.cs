using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaTriadBoss : BossBase
{
    [Header("Shoot Sources")]
    public AIShooterScript[] shoot_sources;
    public AIShooterScript[] wallShootSources;
    public AIShooterScript[] deltaSources, otherShootSources, triangleSources;

    [Header("States & Phases")]
    public AlphaTriadBurst burstState, burstState2, burstState3;
    public AlphaTriadWall wallState;
    public AlphaTriadDelta deltaState, deltaState2, deltaState3;
    public AlphaTriadOther spiralState, lineState;
    public AlphaTriadTriangle triangleState, triangleState2, triangleState3;
    public Phase firstPhase, secondPhase, thirdPhase;

    [Header("Enrage Information")]
    public int currentPhaseInt;
    public GameObject wingTriad, wingTriadTwo, shooter;
    public GameObject[] wingTriadArray, wingTriadArrayTwo;
    public FixedRotator rotator;
    // Start is called before the first frame update
    void Awake(){

    // this,  stateTime,  speed,  force,  fireRate
    burstState = new AlphaTriadBurst(this, 8f, 1.2f, 100, 1.5f);
    wallState = new AlphaTriadWall(this, 10f, 10f, 2000, 7f, 3f, 0.8f);
    deltaState = new AlphaTriadDelta(this, 6f, 1f, 100, 0.5f);
    spiralState = new AlphaTriadOther(this, 10f, 0f, 0f, 0.7f, 1.2f, 0);
    triangleState = new AlphaTriadTriangle(this, 10f, 0.5f, 100, 1.8f, 1f);
    lineState = new AlphaTriadOther(this, 9f, 0f, 100f, 2f, 1.5f, 1);

    burstState2 = new AlphaTriadBurst(this, 8f, 1.8f, 1000, 1.3f);
    deltaState2 = new AlphaTriadDelta(this, 6f, 1.5f, 1000, 0.4f);
    triangleState2 = new AlphaTriadTriangle(this, 10f, 0.6f, 1000, 1.4f, 0.8f);

    burstState3 = new AlphaTriadBurst(this, 8f, 3f, 1000, 1.1f);
    deltaState3 = new AlphaTriadDelta(this, 6f, 2.5f, 1000, 0.3f);
    triangleState3 = new AlphaTriadTriangle(this, 10f, 1.5f, 1000, 1.25f, 0.6f);

    firstPhase.statesInPhase = new BossStateData[] {burstState, wallState, deltaState, lineState, triangleState, spiralState};
    firstPhase.minHealth = 0.75f;

    secondPhase.statesInPhase = new BossStateData[] {burstState2, lineState, spiralState, deltaState2, wallState, triangleState2};
    secondPhase.minHealth = 0.4f;

    thirdPhase.statesInPhase = new BossStateData[] {burstState3, deltaState3, wallState, burstState3, lineState, triangleState3, spiralState};
    thirdPhase.minHealth = 0f;

    phases = new Phase[3]{firstPhase, secondPhase, thirdPhase};
    }

    protected override void OnSetPhase(){
        currentPhaseInt ++;
        switch (currentPhaseInt)
        {
            case 2:
                foreach (GameObject gb in wingTriadArray)
                    {
                        shooter = Instantiate(wingTriad, gb.transform);
                        shooter.GetComponent<Rigidbody2D>().AddForce(gb.transform.up * 800);
                        shooter.transform.SetParent(null);
                        gb.SetActive(false);
                    }
                rotator.speed = 250;
                break;
            case 3:
                foreach (GameObject gb in wingTriadArrayTwo)
                    {
                        shooter = Instantiate(wingTriadTwo, gb.transform);
                        shooter.GetComponent<Rigidbody2D>().AddForce(gb.transform.up * 800);
                        shooter.transform.SetParent(null);
                        gb.SetActive(false);
                    }
                rotator.speed = 500;
                break;
        }
    }
}
