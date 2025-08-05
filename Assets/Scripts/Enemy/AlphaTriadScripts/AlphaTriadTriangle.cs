using UnityEngine;

[System.Serializable]
public class AlphaTriadTriangle : BossStateData
{
    AlphaTriadBoss triadData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, predictor_coefficient;
    private float fireRate, currentFireInterval;
    private Vector2 target;

    public AlphaTriadTriangle(AlphaTriadBoss bossBase, float stateTime, float speed, float force, float fireRate, float predict) : base(bossBase){
        triadData = bossBase;
        this.stateTime = stateTime;
        this.maxSpeed = speed;
        this.accel = force;
        this.fireRate = fireRate;
        this.predictor_coefficient = predict;
    }

    public override void Start(){
        currentStateTime = stateTime;
        target = Player.main.tf.position;

        triadData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)triadData.transform.position);

        currentFireInterval = fireRate;

    }
    public override void FixedUpdate(){
        // Sets the target for the pathfinder.
        target = (Vector2)Player.main.tf.position + ((Vector2)(Player.main.rb.velocity/Time.fixedDeltaTime) *predictor_coefficient );
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
            foreach(AIShooterScript source in triadData.triangleSources){
                source.Shoot();
            }
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }
    }
}