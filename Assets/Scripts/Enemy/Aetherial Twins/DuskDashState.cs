using UnityEngine;

[System.Serializable]
public class DuskDashState : BossStateData
{
    DuskBoss duskData;
    private float maxSpeed, accel, returnSpeed, returnAccel, distance, returnDist, forceEnd;
    private Vector2 startPos, playerPos;
    public bool atPlayer, atStart;

    public DuskDashState(DuskBoss bossBase, float speed, float force, float retSpeed, float retAccel, float dist, float returnDist) : base(bossBase){
        duskData = bossBase;
        this.maxSpeed = speed; 
        this.accel = force;
        this.returnSpeed = retSpeed;
        this.returnAccel = retAccel;
        this.distance = dist;
        this.returnDist = returnDist;
    }

    public override void Start(){
        // Sets the start Position.
        forceEnd = 0;
        playerPos = duskData.dawnScript.transform.position;
        duskData.rb_self.velocity = new Vector2(0, 0);

        // Bool Set
        atPlayer = false;
        atStart = false;
    }
    public override void FixedUpdate(){
        if (!atPlayer){
            // Body Rotation
            duskData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*5.5f);
            DashUpdate();
        }
        else if (!atStart){
            // Body Rotation
            duskData.bodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*4.5f);
            duskData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*3f);
            ReturnUpdate();
        }

    }
    public override void Update(){
        forceEnd += Time.deltaTime;
        if (forceEnd >= 5f){
            End();
        }
    }
    void DashUpdate(){
        duskData.transform.rotation = Quaternion.LookRotation(Vector3.forward, playerPos - (Vector2)duskData.transform.position);

        duskData.rb_self.AddForce(transform.up * accel * Time.fixedDeltaTime);
        if (duskData.rb_self.velocity.magnitude > maxSpeed){
            duskData.rb_self.velocity = duskData.rb_self.velocity.normalized*maxSpeed;
        }
        if (Vector2.Distance((Vector2)duskData.transform.position, playerPos) <= distance){
            duskData.rb_self.velocity = new Vector2(0, 0);
            atPlayer = true;
        }
    }
    void ReturnUpdate(){
        startPos = new Vector2(Player.main.tf.position.x - 8f, Player.main.tf.position.y);

        duskData.transform.rotation = Quaternion.LookRotation(Vector3.forward, startPos - (Vector2)duskData.transform.position);

        duskData.rb_self.AddForce(transform.up * returnAccel * Time.fixedDeltaTime);
        if (duskData.rb_self.velocity.magnitude > returnSpeed){
            duskData.rb_self.velocity = duskData.rb_self.velocity.normalized*maxSpeed;
        }
        if (Vector2.Distance((Vector2)duskData.transform.position, startPos) <= returnDist){
            End();
        }
    }
}