using UnityEngine;

[System.Serializable]
public class DawnBoxState : BossStateData
{
    DawnBoss dawnData;

    private float fireRate, currentFireInterval, distance, lerpTime, dashCooldown, dashTime;
    private Vector2 initPosition, finalPosition1, finalPosition2, bowTarget;
    private int currState;
    private bool canShoot;

    public DawnBoxState(DawnBoss bossBase, float dist, float fr, float time) : base(bossBase){
        dawnData = bossBase;
        this.distance = dist;
        this.fireRate = fr;
        this.dashCooldown = time;
    }

    public override void Start(){
        dawnData.rb_self.velocity = new Vector2(0, 0);
        initPosition = new Vector2(Player.main.tf.position.x + distance, Player.main.tf.position.y + distance);
        finalPosition1 = new Vector2(Player.main.tf.position.x + distance, Player.main.tf.position.y - distance);
        finalPosition2 = new Vector2(Player.main.tf.position.x - distance, Player.main.tf.position.y - distance);
        bowTarget = Player.main.tf.position;

        currentFireInterval = fireRate;
        lerpTime = 0f;
        currState = 0;
        canShoot = false;
        dashTime = 0;
    }
    public override void FixedUpdate(){
        // Staff Pathfinding
        bowTarget = Player.main.tf.position;
        dawnData.bowTf.rotation = Quaternion.LookRotation(Vector3.forward, bowTarget - (Vector2)dawnData.bowTf.position);
        // Body Rotation
        dawnData.bodyTf.eulerAngles = new Vector3(0, 0, -1*dawnData.rb_self.velocity.x*4.5f);
        dawnData.mainBodyTf.eulerAngles = new Vector3(0, 0, -1*dawnData.rb_self.velocity.x*3f);


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
            dawnData.fiveArrow[0].Shoot();
            currentFireInterval = fireRate;
        }
        else{
            currentFireInterval -= Time.deltaTime;
        }


    }
    void dashUpdate(Vector2 target){
        if (Vector2.Distance((Vector2)dawnData.transform.position, target) <= 0.2f){
            lerpTime = 0f;
            dashTime = dashCooldown;
            currState ++;
        }
        else{
            dawnData.transform.position = Vector2.Lerp((Vector2)dawnData.transform.position, target, 0.2f*lerpTime);
        }
    }
}