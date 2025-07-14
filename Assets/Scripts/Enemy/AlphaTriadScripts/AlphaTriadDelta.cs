using UnityEngine;

[System.Serializable]
public class AlphaTriadDelta : BossStateData
{
    AlphaTriadBoss triadData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel;
    private float fireRate, currentFireInterval;
    private Vector2 target;
    private int cycle;

    public AlphaTriadDelta(AlphaTriadBoss bossBase, float stateTime, float speed, float force, float fireRate) : base(bossBase){
        triadData = bossBase;
        this.stateTime = stateTime;
        this.maxSpeed = speed;
        this.accel = force;
        this.fireRate = fireRate;
    }
    public override void Start(){
        currentStateTime = stateTime;
        target = Player.main.tf.position;

        triadData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)triadData.transform.position);

        currentFireInterval = fireRate;
        cycle = 0;

    }
    public override void FixedUpdate(){
        // Sets the target for the pathfinder.
        target = Player.main.tf.position;
        // Constantly looks towards the player
        triadData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)triadData.transform.position);

        //Adds a Force towards the player
        triadData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (triadData.rb_self.velocity.magnitude > maxSpeed){
                triadData.rb_self.velocity = triadData.rb_self.velocity.normalized*maxSpeed;
            }
    }
    public override void Update(){
        FiringUpdate();
        if (currentStateTime > 0){
            currentStateTime -= Time.deltaTime;
        }
        else{
            End();
        }

    }
    void FiringUpdate(){
        if (currentFireInterval <= 0){
            triadData.deltaSources[cycle].Shoot();
            cycle += 1;
            if (cycle == 3){
                cycle = 0;
            }
        currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }

    }
}