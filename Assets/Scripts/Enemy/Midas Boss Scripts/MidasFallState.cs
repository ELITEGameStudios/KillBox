using UnityEngine;

[System.Serializable]
public class MidasFallState : BossStateData
{
    MidasBoss midasData;
    private float stateTime, currentStateTime;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval, initFire;
    private Vector2 target;

    public MidasFallState(MidasBoss bossBase, float time, float speed, float force, float dist, float fr, float init) : base(bossBase){
        midasData = bossBase;
        this.stateTime = time;
        this.maxSpeed = speed; 
        this.accel = force;
        this.distance = dist;
        this.fireRate = fr;
        this.initFire = init;
    }
    public override void Start(){
        midasData.midasAnimate.SetTrigger("Intro");
        currentStateTime = stateTime;

        target = new Vector2(Player.main.tf.position.x, Player.main.tf.position.y + distance);

        currentFireInterval = initFire;
    }
    public override void FixedUpdate(){
        midasData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)midasData.transform.position);
        target = new Vector2(Player.main.tf.position.x, Player.main.tf.position.y + distance);

        midasData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (midasData.rb_self.velocity.magnitude > (Vector2.Distance((Vector2)midasData.transform.position, target) * maxSpeed)){
                midasData.rb_self.velocity = midasData.rb_self.velocity.normalized* (Vector2.Distance((Vector2)midasData.transform.position, target) * maxSpeed);
            }
    }
    public override void Update(){
        FiringUpdate();

        // The Timer until the next phase.
        if (currentStateTime > 0){
            currentStateTime -= Time.deltaTime;
        }
        else{
            midasData.midasAnimate.SetTrigger("Exit");
            End();
        }

    }
    void FiringUpdate(){
        if (currentFireInterval <= 0)
        {
            foreach (AIShooterScript shooter in midasData.crownFallShooter){
                shooter.Shoot();
            }
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }
    }

}