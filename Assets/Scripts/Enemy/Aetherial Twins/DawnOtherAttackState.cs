using UnityEngine;

[System.Serializable]
public class DawnOtherAttackState : BossStateData
{
    DawnBoss dawnData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval, initFire;
    private Vector2 bodyTarget, bowTarget;
    private int index;

    public DawnOtherAttackState(DawnBoss bossBase, float time, float speed, float force, float dist, float fr, float init, int otherIndex) : base(bossBase){
        dawnData = bossBase;
        this.stateTime = time;
        this.maxSpeed = speed; 
        this.accel = force;
        this.distance = dist;
        this.fireRate = fr;
        this.initFire = init;
        this.index = otherIndex;
    }

    public override void Start(){
        currentStateTime = stateTime;

        bodyTarget = new Vector2(Player.main.tf.position.x + distance, Player.main.tf.position.y);
        bowTarget = Player.main.tf.position;

        currentFireInterval = initFire;
    }
    public override void FixedUpdate(){
        // Staff Pathfinding
        bowTarget = Player.main.tf.position;
        dawnData.bowTf.rotation = Quaternion.LookRotation(Vector3.forward, bowTarget - (Vector2)dawnData.bowTf.position);
        // Body Rotation
        dawnData.bodyTf.eulerAngles = new Vector3(0, 0, -1*dawnData.rb_self.velocity.x*4.5f);
        dawnData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*dawnData.rb_self.velocity.x*3f);


        // Main Pathfinding.
        bodyTarget = new Vector2(Player.main.tf.position.x + distance, Player.main.tf.position.y);
        dawnData.transform.rotation = Quaternion.LookRotation(Vector3.forward, bodyTarget - (Vector2)dawnData.transform.position);

        dawnData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (dawnData.rb_self.velocity.magnitude > maxSpeed){
                dawnData.rb_self.velocity = dawnData.rb_self.velocity.normalized*maxSpeed;
            }

    }
    public override void Update(){
        FiringUpdate();

        // The Timer until the next phase.
        if (currentStateTime > 0){
            currentStateTime -= Time.deltaTime;
        }
        else{
            End();
        }

    }
    void FiringUpdate(){
        if (currentFireInterval <= 0)
        {
            dawnData.otherShootSources[index].Shoot();
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
}