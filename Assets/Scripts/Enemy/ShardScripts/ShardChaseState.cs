
using UnityEngine;

[System.Serializable]
public class ShardChaseState : BossStateData {
    
    ShardBoss shardData;
    private float chaseTime, currentChaseTime;
    private float maxSpeed, maxAccel, aDrag, rSpeed;

    public ShardChaseState(ShardBoss bossBase, float chaseTime, float maxSpeed, float maxAccel, float aDrag, float rSpeed) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        shardData = bossBase;

        this.maxSpeed = maxSpeed;
        this.maxAccel = maxAccel;
        this.aDrag = aDrag;
        this.rSpeed = rSpeed;
        this.chaseTime = chaseTime;
    }

    public override void Start() // Called When the state object becomes active
    {
        shardData.rotator.SetRotationRate(shardData.Aspeed[0], 1f);
        rb_self.angularDrag = aDrag;
        movement_script.rotationSpeed = rSpeed;
        movement_script.maxSpeed = maxSpeed;
        currentChaseTime = chaseTime;
        movement_script.maxAcceleration = maxAccel;

    } 
    public override void Update() // Called every frame while the object is active
    {
        rb_self.angularVelocity = shardData.Aspeed[0] * Time.deltaTime;

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
}