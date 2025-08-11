using UnityEngine;

[System.Serializable]
public class MidasRamState : BossStateData
{
    MidasBoss midasData;
    private float maxSpeed, accel, distance, dashSpeed, dashAccel;
    private Vector2 playerTarget, downTarget;
    private bool atPlayer, atDown, atUp;

    public MidasRamState(MidasBoss bossBase, float speed, float force, float dist) : base(bossBase){
        midasData = bossBase;
        this.maxSpeed = speed;
        this.accel = force;
        this.distance = dist;
    }
    public override void Start(){
        playerTarget = new Vector2(Player.main.tf.position.x + (Player.main.rb.velocity.x/Time.fixedDeltaTime)* 0.5f, Player.main.tf.position.y + 5.5f);
        downTarget = new Vector2(Player.main.tf.position.x + (Player.main.rb.velocity.x/Time.fixedDeltaTime) * 0.5f, Player.main.tf.position.y - 5.5f);
        midasData.rb_self.velocity = new Vector2(0, 0);

        atPlayer = false; atDown = false; atUp = false;
    }
    public override void FixedUpdate(){
        if (!atPlayer){ToTarget(playerTarget, 1);}
        else if (!atDown) {ToTarget(downTarget, 2);}
        else if (!atUp) {ToTarget(playerTarget, 3);}
    }
    public override void Update(){

    }
    void ToTarget(Vector2 target, int index){
        midasData.transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)midasData.transform.position);
            midasData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
            if (midasData.rb_self.velocity.magnitude > maxSpeed){
                midasData.rb_self.velocity = midasData.rb_self.velocity.normalized*maxSpeed;
            }
            if (Vector2.Distance((Vector2)midasData.transform.position, target) <= distance){
                midasData.rb_self.velocity = new Vector2(0, 0);
                switch(index){
                    case 1: atPlayer = true; midasData.ramParticles.Play(); break;
                    case 2: atDown = true; midasData.ramFinished.Play(); break;
                    case 3: atUp = true; midasData.ramParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting); End(); break;
                }
            }
    }
}