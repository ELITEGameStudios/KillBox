using UnityEngine;

[System.Serializable]
public class MidasGuillotineState : BossStateData
{
    MidasBoss midasData;
    private float maxSpeed, accel, distance;
    private float fireRate, currentFireInterval;
    private int index;
    private Vector2 target;

    public MidasGuillotineState(MidasBoss bossBase, float speed, float force, float dist, float fr, int index) : base(bossBase){
        midasData = bossBase;
        this.maxSpeed = speed; 
        this.accel = force;
        this.distance = dist;
        this.fireRate = fr;
        this.index = index;
    }
    public override void Start(){

        target = new Vector2(Player.main.tf.position.x, Player.main.tf.position.y + distance);

        currentFireInterval = fireRate * midasData.normalizedHealth;
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
    }
    void FiringUpdate(){
        if (currentFireInterval <= 0)
        {
            midasData.alterShooter[index].Velocity *= -1;
            midasData.alterShooter[index].Shoot();
            currentFireInterval = fireRate * midasData.normalizedHealth;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }
    }

}