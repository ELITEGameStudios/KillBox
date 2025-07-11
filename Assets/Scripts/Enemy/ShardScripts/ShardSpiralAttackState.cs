
using UnityEngine;

[System.Serializable]
public class ShardSpiralAttackState : BossStateData
{
    ShardBoss shardData;
    public AIShooterScript[] shoot_sources;
    private float spiralTime, currentSpiralTime, warmupTime, currentWarmupTime;
    private float maxSpeed, maxAccel, aDrag, rSpeed;
    private bool warmingUp;

    public ShardSpiralAttackState(ShardBoss bossBase, float maxSpeed, float maxAccel, float aDrag, float rSpeed) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        shardData = bossBase;

        this.maxSpeed = maxSpeed;
        this.maxAccel = maxAccel;
        this.aDrag = aDrag;
        this.rSpeed = rSpeed;

    }

    public override void Start() // Called When the state object becomes active
    {

        movement_script.maxSpeed = maxSpeed;
        movement_script.maxAcceleration = maxAccel;
        movement_script.rotationSpeed = rSpeed;
        rb_self.angularDrag = aDrag;
        currentSpiralTime = 0;
        spiralTime = 3;

        // warmup setup
        warmupTime = 1.5f;
        currentWarmupTime = warmupTime;
        shoot_sources = shardData.shoot_sources;
        warmingUp = true;

        shardData.rotator.SetRotationRate(shardData.Aspeed[1], 1.5f);
    }
    public override void Update() // Called every frame while the object is active
    {
        // rb_self.angularVelocity = shardData.Aspeed[1] * Time.deltaTime;

        // Warming up
        if (warmingUp)
        {
            if (currentWarmupTime <= 0)
            {
                warmingUp = false;
                for (int i = 0; i < shoot_sources.Length; i++)
                {
                    shoot_sources[i].enabled = true;
                    shoot_sources[i].CanShoot = true;
                }
                return;
            }
            else
            {
                currentWarmupTime -= Time.deltaTime;
            }

            return;
        }

        // Active
        if (currentSpiralTime < spiralTime)
        {
            currentSpiralTime += Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < shoot_sources.Length; i++)
            {
                shoot_sources[i].enabled = false;
                shoot_sources[i].CanShoot = false;
            }

            End();
        }
    }
}