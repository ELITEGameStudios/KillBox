using UnityEngine;

[System.Serializable]
public class DuskWaveState : BossStateData
{
    DuskBoss duskData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval;
    private Vector2 bodyTarget;
    private bool atTarget;

    public DuskWaveState(DuskBoss bossBase, float time, float speed, float force, float dist, float fr) : base(bossBase){
        duskData = bossBase;
        this.stateTime = time;
        this.maxSpeed = speed; 
        this.accel = force;
        this.distance = dist;
        this.fireRate = fr;
    }

    public override void Start(){
        duskData.rotator.speed = 300;
        currentStateTime = stateTime;
        atTarget = false;

        bodyTarget = new Vector2(Player.main.tf.position.x - distance, Player.main.tf.position.y);

        currentFireInterval = fireRate;
    }
    public override void FixedUpdate(){
        // Body Rotation
        duskData.bodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*4.5f);
        duskData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*3f);


        // Main Pathfinding.
        if (!atTarget){
            duskData.transform.rotation = Quaternion.LookRotation(Vector3.forward, bodyTarget - (Vector2)duskData.transform.position);

            duskData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (duskData.rb_self.velocity.magnitude > maxSpeed){
                duskData.rb_self.velocity = duskData.rb_self.velocity.normalized*maxSpeed;
            }
            if (Vector2.Distance((Vector2)duskData.transform.position, bodyTarget) <= 0.5f){
            duskData.rb_self.velocity = new Vector2(0, 0);
            atTarget = true;
        }
        }

    }
    public override void Update(){
        if (atTarget){
            FiringUpdate();

            // The Timer until the next phase.
            if (currentStateTime > 0){
                currentStateTime -= Time.deltaTime;
            }
            else{
                duskData.rotator.speed = 0;
                End();
            }
        }

    }
    void FiringUpdate(){
        if (currentFireInterval <= 0)
        {
            duskData.mainShootSources[6].Shoot();
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
}