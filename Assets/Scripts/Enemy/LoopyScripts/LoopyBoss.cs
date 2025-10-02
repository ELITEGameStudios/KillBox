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
    // public BeginnerLoopyAttack delta3;


    // [Header("Drain Graphic")]


    [Header("Spawner prefabs")]
    public GameObject goldDelta, preAngledGoldDelta;
    public GameObject goldPolaroid;
    public GameObject goldTriad;
    public GameObject shard;
    public SweepingIndicator sweepingIndicator;

    public IEnumerator SpawnEnemies() { if (currentState is BeginnerLoopyAttack) { yield return (currentState as BeginnerLoopyAttack).SpawnEnemies(); } }
    public void StopSpawnEnemiesCoroutine() { StopCoroutine(nameof(SpawnEnemies)); }

    public float debugFloat;
    // [Header("Animation Curves")]

    // [Header("Debug")]


    // Start is called before the first frame update
    void Awake()
    {
        //Initialize attacks here
        delta1 = new BeginnerLoopyAttack(this, new GameObject[] { goldDelta }, quantity: 12, iterationInterval: 0.3f, startDistance: 1.25f);
        delta2 = new BeginnerLoopyAttack(this, new GameObject[] { goldDelta }, quantity: 24, iterationInterval: 0.2f, startDistance: 1.25f, offset: 0.25f);
        shard1= new BeginnerLoopyAttack(this, new GameObject[] { shard }, quantity: 6, iterations: 1, spawningInterval: 0.2f, startDistance: 5f, maxDuration: 10, destroySpawnedEnemies: true);
        shardDelta = new BeginnerLoopyAttack(this, new GameObject[] { shard, preAngledGoldDelta }, quantity: 8, iterations: 100, spawningInterval: 0.1f, startDistance: 5f, offset: 0.25f, maxDuration: 10, destroySpawnedEnemies: true);
        polaroid1= new BeginnerLoopyAttack(this, new GameObject[] { goldPolaroid }, quantity: 6, iterations: 5, spawningInterval: 0f, startDistance: 20f);

        //Initialize phases here
        phase1.statesInPhase = new BossStateData[] { delta1, shard1, polaroid1, delta2, shardDelta, polaroid1 };

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
