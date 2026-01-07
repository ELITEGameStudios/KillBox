using UnityEngine;

[System.Serializable]
public class DawnGravityState : BossStateData
{
    DawnBoss dawnData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance, offset;
    private float fireRate, currentFireInterval, initFire;
    private Vector2 bodyTarget, bowTarget;

    public DawnGravityState(DawnBoss bossBase, float time, float speed, float force, float offset, float fr, float init) : base(bossBase){
        dawnData = bossBase;
        this.stateTime = time;
        this.maxSpeed = speed; 
        this.accel = force;
        this.offset = offset;
        this.fireRate = fr;
        this.initFire = init;
    }

    public override void Start(){
        currentStateTime = stateTime;

        dawnData.bowTf.eulerAngles = new Vector3(0, 0, 0);
        bowTarget = Player.main.tf.position;

        currentFireInterval = initFire;
    }
    public override void FixedUpdate(){
        // Body Rotation
        dawnData.bodyTf.eulerAngles = new Vector3(0, 0, -1*dawnData.rb_self.velocity.x*4.5f);
        dawnData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*dawnData.rb_self.velocity.x*3f);


        // Main Pathfinding.
        bodyTarget = new Vector2(Player.main.tf.position.x, Player.main.tf.position.y + offset);
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
            foreach (AIShooterScript source in dawnData.gravArrowDown)
            {
                source.Shoot();
            }   
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
}