
using UnityEngine;

[System.Serializable]
public class PrologueMainAttack : BossStateData
{

    ShardBoss shardData;
    private float chaseTime, currentChaseTime;
    private float maxSpeed, maxAccel, aDrag, rSpeed;
    private bool fires;
    public AIShooterScript[] shoot_sources;
    public float fireRate;
    public float currentFireInterval;

    public PrologueMainAttack(ShardBoss bossBase, float chaseTime, float maxSpeed, float maxAccel, float aDrag, float rSpeed, bool fires = false) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        shardData = bossBase;

        this.maxSpeed = maxSpeed;
        this.maxAccel = maxAccel;
        this.aDrag = aDrag;
        this.rSpeed = rSpeed;
        this.chaseTime = chaseTime;
        this.fires = fires;
        this.fireRate = 0.75f;
    }

    public override void Start() // Called When the state object becomes active
    {
        shardData.rotator.SetRotationRate(shardData.Aspeed[0], 1f);
        rb_self.angularDrag = aDrag;
        movement_script.rotationSpeed = rSpeed;
        movement_script.enabled = true;
        movement_script.updatePosition = true;
        movement_script.maxSpeed = maxSpeed;

        currentChaseTime = chaseTime;
        movement_script.maxAcceleration = maxAccel;

        if (fires)
        {
            currentFireInterval = fireRate;
        }
    }
    public override void Update() // Called every frame while the object is active
    {
        rb_self.angularVelocity = shardData.Aspeed[0] * Time.deltaTime;
        if(fires){ FiringUpdate(); }

        if (currentChaseTime > 0)
        {
            Debug.Log("chase phase update");
            currentChaseTime -= Time.deltaTime;
        }
        else
        {
            End();
            Debug.Log("Ended chase phase");
        }
    }
    
    void FiringUpdate() {
        if (currentFireInterval <= 0)
        {
            foreach (AIShooterScript source in shardData.shoot_sources)
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