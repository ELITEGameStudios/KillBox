using UnityEngine;

[System.Serializable]
public class DebugShootState : BossStateData
{
    DebugBoss debugData;
    private float stateTime, currentStateTime;
    private float maxSpeed, acceleration, min, max;
    private bool isForce, isShotgun, canShoot, canRotate;
    public AIShooterScript[] shoot_sources;
    public float fireRate;
    public float currentFireInterval;
    public float trig;
    private Vector2 target;

    public DebugShootState(DebugBoss bossBase, float chaseTime, float speed, float accel, float fireRate, float min, float max, 
    bool isForce = false, bool isShotgun = false, bool canShoot = true, bool canRotate = true) : base(bossBase)
    {
        debugData = bossBase;
        this.stateTime = chaseTime;
        this.maxSpeed = speed;
        this.acceleration = accel;
        this.fireRate = fireRate;
        this.min = min;
        this.max = max;
        this.isForce = isForce;
        this.isShotgun = isShotgun;
        this.canShoot = canShoot;
        this.canRotate = canRotate;

    }
    public override void Start() // Called When the state object becomes active
    {
        currentStateTime = stateTime;
        // Teleports Debug to a position near the player.
        trig = Random.Range(0f, 6.28f);
        target = Player.main.tf.position;
        debugData.transform.position = new Vector2(target.x + Random.Range(min, max)*Mathf.Cos(trig), target.y + Random.Range(min, max)*Mathf.Sin(trig));
        debugData.rb_self.velocity = new Vector2(0, 0);

        // Looks towards the player.
        debugData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)debugData.transform.position);

        if (canShoot){
            currentFireInterval = fireRate;
        }
    }
    
    public override void FixedUpdate(){
        // Sets the target for the pathfinder.
        target = Player.main.tf.position;
        // Constantly looks towards the player if can Rotate
        if (canRotate){
            debugData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)debugData.transform.position);
        }
        // Accelerates towards the player until it hits the max speed. Only if isForce.
        if (isForce){
            debugData.rb_self.AddForce(transform.up * acceleration * Time.fixedDeltaTime);
            if (debugData.rb_self.velocity.magnitude > maxSpeed){
                debugData.rb_self.velocity = debugData.rb_self.velocity.normalized*maxSpeed;
            }
        }
        // Moves towards the player at constant velocity.
        else{
                debugData.rb_self.velocity = ((Vector2)debugData.transform.position - target).normalized*maxSpeed*-1;
            }

    }
    public override void Update()
    {
        // If canShoot, then runs the firing update, which allows Debug to fire. 
        if (canShoot){
            FiringUpdate();
        }

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
            // If isShotgun is true, then shoot at every point in the shoot sources.
            if (isShotgun){
                foreach (AIShooterScript source in debugData.shoot_sources)
                {
                    source.Shoot();
                }
            }
            else{
                debugData.shoot_sources[0].Shoot();
            }
            currentFireInterval = fireRate;
        }
        else
        {
            currentFireInterval -= Time.deltaTime;
        }
    }

}