
using UnityEngine;

[System.Serializable]
public class ShardDashAttack : BossStateData
{
    ShardBoss shardData;
    public AIShooterScript[] shoot_sources;


    [Header("lerp movement data")]
    public Vector2 startPos, targetPos;
    public float dashTime;
    public float distance;
    public int iterations, currentIterations;
    public bool vertical;


    [Header("Fire data")]
    
    public float fireRate;
    public float currentFireInterval;


    [Header("State machine data")]
    public float timeInDash;


    public ShardDashAttack(ShardBoss bossBase, float dashTime, float distance, float fireRate, int iterations) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        shardData = bossBase;

        this.dashTime = dashTime;
        this.distance = distance;
        this.iterations = iterations;
        this.fireRate = fireRate;
        
        shoot_sources = shardData.shoot_sources;
    }

    public override void Start() // Called When the state object becomes active
    {
        shardData.rotator.SetRotationRate(shardData.Aspeed[0], 1.5f);
        rb_self.velocity = Vector2.zero;

        movement_script.updatePosition = false;
        movement_script.enabled = false;

        currentIterations = iterations;

        SetupNewDash();
    }

    void SetupNewDash()
    {
        // vertical = Random.Range(0, 2) == 1;
        vertical = false;
        float invert = Random.Range(0, 2) == 1 ? 1 : -1;
        startPos = new Vector2(
            vertical ? Player.main.tf.position.x : Player.main.tf.position.x + (distance * invert),
            !vertical ? Player.main.tf.position.y : Player.main.tf.position.y + (distance * invert)
        );

        targetPos = new Vector2(
            vertical ? Player.main.tf.position.x : Player.main.tf.position.x + (distance * invert * -1),
            !vertical ? Player.main.tf.position.y : Player.main.tf.position.y + (distance * invert * -1)
        );

        currentIterations--;
        transform.position = startPos;
        timeInDash = 0;
         
    }

    public override void Update() // Called every frame while the object is active
    {
        FiringUpdate();
    }

    public override void FixedUpdate() // Called every frame while the object is active
    {
        if (timeInDash >= dashTime)
        {
            if (currentIterations > 0){
                SetupNewDash();
            }
            else
            {
                movement_script.updatePosition = false;
                movement_script.enabled = false;
                End();
            }
        }
        else
        {
            transform.position = Vector2.Lerp(startPos, targetPos, timeInDash / dashTime);
            timeInDash += Time.fixedDeltaTime;
        }
    }

    void FiringUpdate() {

        if (currentFireInterval <= 0)
        {
            foreach (AIShooterScript source in shoot_sources)
            {
                source.Shoot();
            }
            currentFireInterval = fireRate;
        }
        else
        {
            currentFireInterval -= Time.deltaTime;
        }
    }
}