using UnityEngine;

[System.Serializable]
public class DuskCannonState : BossStateData
{
    DuskBoss duskData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval, initFire, multiply, cap;
    private Vector2 bodyTarget, staffTarget;

    public DuskCannonState(DuskBoss bossBase, float time, float speed, float force, float dist, float fr, float init, float mult, float cap) : base(bossBase){
        duskData = bossBase;
        this.stateTime = time;
        this.maxSpeed = speed; 
        this.accel = force;
        this.distance = dist;
        this.fireRate = fr;
        this.initFire = init;
        this.multiply = mult;
        this.cap = cap;
    }

    public override void Start(){
        currentStateTime = stateTime;

        bodyTarget = new Vector2(Player.main.tf.position.x - distance, Player.main.tf.position.y);
        staffTarget = Player.main.tf.position;

        fireRate = initFire;
        currentFireInterval = fireRate;
    }
    public override void FixedUpdate(){
        // Staff Pathfinding
        staffTarget = Player.main.tf.position;
        duskData.staffTf.rotation = Quaternion.LookRotation(Vector3.forward, staffTarget - (Vector2)duskData.staffTf.position);
        // Body Rotation
        duskData.bodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*4.5f);
        duskData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*3f);


        // Main Pathfinding.
        bodyTarget = new Vector2(Player.main.tf.position.x - distance, Player.main.tf.position.y);
        duskData.transform.rotation = Quaternion.LookRotation(Vector3.forward, bodyTarget - (Vector2)duskData.transform.position);

        duskData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (duskData.rb_self.velocity.magnitude > maxSpeed){
                duskData.rb_self.velocity = duskData.rb_self.velocity.normalized*maxSpeed;
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
            duskData.mainShootSources[5].Shoot();
            fireRate = fireRate * multiply;
            if (fireRate <= cap){
                fireRate = cap;
            }
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
}