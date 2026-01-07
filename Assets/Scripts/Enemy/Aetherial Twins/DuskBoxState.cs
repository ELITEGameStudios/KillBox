using UnityEngine;

[System.Serializable]
public class DuskBoxState : BossStateData
{
    DuskBoss duskData;

    private float fireRate, currentFireInterval, distance, lerpTime, dashCooldown, dashTime;
    private Vector2 initPosition, finalPosition1, finalPosition2, staffTarget;
    private int currState;
    private bool canShoot;

    public DuskBoxState(DuskBoss bossBase, float dist, float fr, float time) : base(bossBase){
        duskData = bossBase;
        this.distance = dist;
        this.fireRate = fr;
        this.dashCooldown = time;
    }

    public override void Start(){
        duskData.rb_self.velocity = new Vector2(0, 0);
        initPosition = new Vector2(Player.main.tf.position.x - distance, Player.main.tf.position.y - distance);
        finalPosition1 = new Vector2(Player.main.tf.position.x - distance, Player.main.tf.position.y + distance);
        finalPosition2 = new Vector2(Player.main.tf.position.x + distance, Player.main.tf.position.y + distance);
        staffTarget = Player.main.tf.position;

        currentFireInterval = fireRate;
        lerpTime = 0f;
        currState = 0;
        canShoot = false;
        dashTime = 0;
    }
    public override void FixedUpdate(){
        // Staff Pathfinding
        staffTarget = Player.main.tf.position;
        duskData.staffTf.rotation = Quaternion.LookRotation(Vector3.forward, staffTarget - (Vector2)duskData.staffTf.position);
        // Body Rotation
        duskData.bodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*4.5f);
        duskData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*duskData.rb_self.velocity.x*3f);


        // Main Pathfinding.
        if (dashTime <= 0){
            switch (currState){
            case 0:
            dashUpdate(initPosition);
            break;
            case 1:
            dashUpdate(finalPosition1);
            break;
            case 2:
            dashUpdate(finalPosition2);
            break;
            case 3:
            dashUpdate(finalPosition1);
            break;
            case 4:
            End();
            break;
        }
        }
        

    }
    public override void Update(){
        if (dashTime <= 0){
            if (currState != 0){
                canShoot = true; 
            }
            lerpTime += Time.deltaTime;}
        else{canShoot = false;}
        if (canShoot){
            FiringUpdate();
        }
        dashTime -= Time.deltaTime;
    }
    void FiringUpdate(){
        if (currentFireInterval <= 0)
        {
            duskData.mainShootSources[0].Shoot();
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
    void dashUpdate(Vector2 target){
        if (Vector2.Distance((Vector2)duskData.transform.position, target) <= 0.2f){
            lerpTime = 0f;
            dashTime = dashCooldown;
            currState ++;
        }
        else{
            duskData.transform.position = Vector2.Lerp((Vector2)duskData.transform.position, target, 0.2f*lerpTime);
        }
    }
}