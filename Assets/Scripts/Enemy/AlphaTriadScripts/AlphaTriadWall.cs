using UnityEngine;

[System.Serializable]
public class AlphaTriadWall : BossStateData
{
    AlphaTriadBoss triadData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval, initCooldown;
    private Vector2 targetUp, targetDown, target;
    private bool atTarget;

    public AlphaTriadWall(AlphaTriadBoss bossBase, float stateTime, float speed, float force, float distance, float fireRate, float cool) : base(bossBase){
        triadData = bossBase;
        this.stateTime = stateTime;
        this.maxSpeed = speed;
        this.accel = force;
        this.distance = distance;
        this.fireRate = fireRate;
        this.initCooldown = cool;
    }
    public override void Start(){
        currentStateTime = stateTime;
        atTarget = false;

        // Gets the target.
        targetUp = new Vector2(Player.main.tf.position.x, Player.main.tf.position.y + distance);
        targetDown = new Vector2(Player.main.tf.position.x, Player.main.tf.position.y - distance);
        if (Vector2.Distance(targetUp, (Vector2)triadData.transform.position) < Vector2.Distance(targetDown, (Vector2)triadData.transform.position)){
            target = targetUp;
        }
        else{
            target = targetDown;
        }
        triadData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)triadData.transform.position);

        currentFireInterval = initCooldown;

    }
    public override void FixedUpdate(){
        if (!atTarget){
            if (Vector2.Distance((Vector2)triadData.transform.position, target) <= 0.5f){
                atTarget = true;
            }
            triadData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)triadData.transform.position);
            triadData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (triadData.rb_self.velocity.magnitude > maxSpeed){
                triadData.rb_self.velocity = triadData.rb_self.velocity.normalized*maxSpeed;
            }
        }
        else{
            triadData.rb_self.velocity = new Vector2(0, 0);
        }

    }
    public override void Update(){
        if (atTarget){
            FiringUpdate();
        }
        if (currentStateTime > 0){
            currentStateTime -= Time.deltaTime;
        }
        else{
            End();
        }
    }
    void FiringUpdate(){
        if (currentFireInterval <= 0){
            foreach(AIShooterScript source in triadData.wallShootSources){
                source.Shoot();
            }
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }
    }
}