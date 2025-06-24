
using UnityEngine;

[System.Serializable]
public class ShardLeaveState : BossStateData {
    
    ShardBoss shardData;
    private float magnitude, targetDistance;

    public ShardLeaveState(ShardBoss bossBase, float magnitude, float targetDistance) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        shardData = bossBase;

        this.targetDistance = targetDistance;
        this.magnitude = magnitude;
    }

    public override void Start() // Called When the state object becomes active
    {
        shardData.rotator.SetRotationRate(shardData.Aspeed[2], 2f);
        movement_script.updatePosition = false;
        movement_script.enabled = false;

    } 
    public override void Update() // Called every frame while the object is active
    {

        if (targetDistance < Vector2.Distance(shardData.transform.position, Player.main.tf.position))
        {
            End();
        }    
    }

    public override void FixedUpdate() // Called every frame while the object is active
    {

        // if (targetDistance < Vector2.Distance(shardData.transform.position, Player.main.tf.position))
        // {
        //     if (!finished) End();
        // }
        // else
        // {
        Vector2 force = (Player.main.tf.position - shardData.transform.position).normalized * -1 * magnitude;
        shardData.rb_self.AddForce((Player.main.tf.position - shardData.transform.position).normalized * -1 * magnitude * Time.fixedDeltaTime, ForceMode2D.Force);
        // Debug.Log(force.x + " | " + force.y);
        // }
    }

    // public override void End(bool interrupted = false) // Called every frame while the object is active
    // {
    //     // movement_script.enabled = true;

    // }
}