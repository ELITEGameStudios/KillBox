using UnityEngine;

[System.Serializable]
public class AlphaTriadOther : BossStateData
{
    AlphaTriadBoss triadData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel;
    private float fireRate, currentFireInterval, init;
    private int bulletIndex;
    private Vector2 target;

    public AlphaTriadOther(AlphaTriadBoss bossBase, float stateTime, float speed, float force, float fireRate, float init, int index) : base(bossBase){
        triadData = bossBase;
        this.stateTime = stateTime;
        this.maxSpeed = speed;
        this.accel = force;
        this.fireRate = fireRate;
        this.init = init;
        this.bulletIndex = index;
    }

    public override void Start(){
        currentStateTime = stateTime;
        target = Player.main.tf.position;

        triadData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)triadData.transform.position);

        currentFireInterval = init;

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
            triadData.otherShootSources[bulletIndex].Shoot();
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }
    }
}