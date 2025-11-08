using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopyBoss : BossBase
{
    [Header("General Info")]
    public float maxSpeed;
    public BossDisplayObj linkedDisplay;


    [Header("State Info")]
    public Phase phase1;
    public BeginnerLoopyAttack delta1;
    public BeginnerLoopyAttack delta2;
    public BeginnerLoopyAttack shard1;
    public BeginnerLoopyAttack shardDelta;
    public BeginnerLoopyAttack polaroid1;
    public BeginnerLoopyAttack dualCutter;
    // public BeginnerLoopyAttack delta3;


    // [Header("Drain Graphic")]


    [Header("Spawner prefabs")]
    public GameObject goldDelta, preAngledGoldDelta;
    public GameObject goldPolaroid;
    public GameObject goldTriad;
    public GameObject shard;
    public GameObject goldCutter;
    public SweepingIndicator sweepingIndicator;

    public IEnumerator SpawnEnemies() { if (currentState is BeginnerLoopyAttack) { yield return (currentState as BeginnerLoopyAttack).SpawnEnemies(); } }
    public void StopSpawnEnemiesCoroutine() { StopCoroutine(nameof(SpawnEnemies)); }

    // [Header("Animation Curves")]

    // [Header("Debug")]
    // public float debugFloat;

    public struct BeamMaskObjects{
        public GameObject rect;
        public GameObject triangleRight;
        public GameObject triangleLeft;
    }


    // Start is called before the first frame update
    void Awake()
    {
        //Initialize attacks here
        delta1 = new BeginnerLoopyAttack(this, new GameObject[] { goldDelta }, quantity: 12, iterationInterval: 0.3f, startDistance: 1.25f, iterations: 12);
        delta2 = new BeginnerLoopyAttack(this, new GameObject[] { goldDelta }, quantity: 24, iterationInterval: 0.2f, startDistance: 1.25f, offset: 0.25f, iterations: 24);
        shard1= new BeginnerLoopyAttack(this, new GameObject[] { shard }, quantity: 4, iterations: 1, spawningInterval: 1f, startDistance: 3.5f, maxDuration: 10, destroySpawnedEnemies: true, introWaitTime: 1);
        shardDelta = new BeginnerLoopyAttack(this, new GameObject[] { shard, preAngledGoldDelta }, quantity: 3, iterations: 100, spawningInterval: 0.1f, startDistance: 3.5f, offset: 0.25f, maxDuration: 10, destroySpawnedEnemies: true, introWaitTime: 1);
        polaroid1= new BeginnerLoopyAttack(this, new GameObject[] { goldPolaroid }, quantity: 3, iterations: 5, spawningInterval: 0f, startDistance: 20f);
        dualCutter= new BeginnerLoopyAttack(this, new GameObject[] { goldCutter }, quantity: 2, iterations: 1, spawningInterval: 0.2f, startDistance: 2f, maxDuration: 20, destroySpawnedEnemies: true, introWaitTime: 1.5f);
        
        //Initialize phases here
        phase1.statesInPhase = new BossStateData[] { delta1, shard1, polaroid1, delta2, shardDelta, polaroid1, dualCutter};

        phases = new Phase[] { phase1 };
        movement_script.enabled = false;
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnLateUpdate()
    {

    }
    
    public GameObject SpawnObject(GameObject prefab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    public override void DeathEvent(bool to_player = false)
    {
        // health.SetImmortal(true);

        // Play transform animation
        // runesRotator.SetRotationRate(0, 1.5f);

        // Invoke(nameof(TransformToEpilogue), 2f);
    }
}
