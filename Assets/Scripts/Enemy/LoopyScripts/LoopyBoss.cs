using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopyBoss : BossBase
{
    [Header("General Info")]
    public float maxSpeed;
    public BossDisplayObj linkedDisplay;


    [Header("State Info")]
    public Phase prologuePhase;
    public Phase epiloguePhase;
    public BeginnerLoopyAttack beginnerLoopyAttack;


    // [Header("Drain Graphic")]


    [Header("Spawner prefabs")]
    public GameObject goldDelta;
    public GameObject goldPolaroid;
    public GameObject goldTriad;
    public GameObject shard;
    public SweepingIndicator sweepingIndicator;

    public IEnumerator SpawnEnemies {get { return beginnerLoopyAttack.SpawnEnemies(); }}


    // [Header("Animation Curves")]

    // [Header("Debug")]


    // Start is called before the first frame update
    void Awake()
    {
        //Initialize attacks here

        //Initialize and phases here

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
