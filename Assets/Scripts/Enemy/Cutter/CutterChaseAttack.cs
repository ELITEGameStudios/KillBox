using System.Collections;
using UnityEngine;

[System.Serializable]
public class CutterChaseAttack : BossStateData
{
    public CutterBoss cutterBoss;
    [SerializeField] int bombIterations, bombsPerBurst;
    [SerializeField] float interval;
    [SerializeField] float time, timer;
    [SerializeField] float bombSpawnRadius;
    [SerializeField] float predictor_coefficient;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject bombsPrefab;
    [SerializeField] bool timeBased {get { return time > 0; }}

    public CutterChaseAttack(CutterBoss bossBase,  float moveSpeed, float time = -1, int bombIterations = 8, float bombSpawnRadius = 1, int bombsPerBurst = 1, float interval = 0.6f, float predictor_coefficient = 1) : base(bossBase) // Always include super(bossBase) in any child class constructors 
    {
        // IMPORTANT - THIS IS NOT USED TO REINITIALIZE OR RESET THE ATTACK FOR MULTIPLE USES. USE OnReset() TO REASSIGN DEFAULT VALUES ON STARTUP! (example: timers, counters, end conditions, etc.)
        cutterBoss = bossBase;
        bombsPrefab = cutterBoss.bomb_prefab;

        this.moveSpeed = moveSpeed;
        this.time = time;
        this.bombIterations = bombIterations;
        this.bombSpawnRadius = bombSpawnRadius;
        this.bombsPerBurst = bombsPerBurst;
        this.interval = interval;
        this.predictor_coefficient = predictor_coefficient;


        OnReset();
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
        timer = time;
    }

    // Called When the state object becomes active
    public override void Start()
    {
        movement_script.enabled = true;
        movement_script.maxSpeed = moveSpeed;
        movement_script.enableRotation = false;
        
        // timer = time;

        if (bombIterations > 0)
        {
            cutterBoss.StartCoroutine(nameof(ChaseWithBombNumerator));
        }
    }

    // Called every frame while the object is active
    public override void Update()
    {
        if(timeBased && timer <= 0){ End(); }
        timer -= Time.deltaTime;
    }

    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        cutterBoss.StopCoroutine(nameof(ChaseWithBombNumerator));
        base.End();
    }

    void QuickBomb(){
        Vector3[] bomb_coordinates = new Vector3[bombsPerBurst];
        GameObject[] bombs = new GameObject[bombsPerBurst];

        Vector3 target = Player.main.tf.position + (Vector3)((Player.main.rb.velocity/Time.fixedDeltaTime) * predictor_coefficient );

        for (int i = 0; i < bomb_coordinates.Length; i++)
        {
            bomb_coordinates[i] = target + new Vector3(Random.Range(-bombSpawnRadius, bombSpawnRadius), Random.Range(-bombSpawnRadius, bombSpawnRadius), 0);
            bombs[i] = Object.Instantiate(bombsPrefab, bomb_coordinates[i], transform.rotation);
            bombs[i].SetActive(true);
        }

        End();
    }
    public IEnumerator ChaseWithBombNumerator()
    {
        for (int i = 0; i < bombIterations; i++)
        {
            QuickBomb();
            yield return new WaitForSeconds(interval);
        }
        if(!timeBased){ End(); }
    }
}