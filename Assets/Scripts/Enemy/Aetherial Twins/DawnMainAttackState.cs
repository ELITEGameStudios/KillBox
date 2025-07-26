using UnityEngine;

[System.Serializable]
public class DawnMainAttackState : BossStateData
{
    DawnBoss dawnData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval, initFire;
    private Vector2 bodyTarget, bowTarget;
    private int shootBullet;

    public DawnMainAttackState(DawnBoss bossBase, float time, float speed, float force, float dist, float fr, float init) : base(bossBase){
        dawnData = bossBase;
        this.stateTime = time;
        this.maxSpeed = speed; 
        this.accel = force;
        this.distance = dist;
        this.fireRate = fr;
        this.initFire = init;
    }

    public override void Start(){
        currentStateTime = stateTime;

        bodyTarget = new Vector2(Player.main.tf.position.x + distance, Player.main.tf.position.y);
        bowTarget = Player.main.tf.position;

        currentFireInterval = initFire;

        shootBullet = 0;

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
            if (shootBullet == 0){
                foreach (AIShooterScript source in dawnData.fiveArrow)
                {
                    source.Shoot();
                }
                shootBullet = 1;
            }
            else{
                foreach (AIShooterScript source in dawnData.fourArrow)
                {
                    source.Shoot();
                }
                shootBullet = 0;
            }
            
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
}