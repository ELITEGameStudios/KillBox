
using System.Collections;
using UnityEngine;

[System.Serializable]
public class QuadShooterSecondAttack : BossStateData
{

    public QuadShooter quadData;
    public float fireRate = 0.2f, gunDistance, Kp = 0.1f;
    public int bullets, currentBullets;
    public float currentFireInterval, maxSpread, timeActive;
    public float playerAngle;

    public Vector2[] positions;

    public QuadShooterSecondAttack(QuadShooter bossBase, float maxSpread = 120) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;
        this.maxSpread = maxSpread;
        gunDistance = 2;

        // this.maxSpeed = ;
        bullets = 16;
        // fireRate = bossBase.fireRateCurve.Evaluate(1 - bossBase.normalizedHealth);
        
        currentBullets = bullets;
    }

    public override void Start() // Called When the state object becomes active
    {
        // fireRate = 
        currentFireInterval = fireRate;
        currentBullets = bullets;
        positions = new Vector2[quadData.guns.Length];
        timeActive = 0;
        // if(quadData.guns[0].transform.parent == null)
        // {
            for (int i = 0; i < quadData.guns.Length; i++){
                quadData.guns[i].transform.SetParent(transform);
            }
        // }
    
    }

    public override void Update() // Called every frame while the object is active
    {

        float spreadInterval = maxSpread/positions.Length;
        // float startPoint = (float)positions.Length/-2;
        float startPoint = -45;
        
        Vector2 playerDir = ((Vector2)Player.main.tf.position - (Vector2)transform.position).normalized;
        playerAngle = Vector2.SignedAngle(Vector2.up, playerDir);

        float startAngle = playerAngle - startPoint;
        for (int i = 0; i < positions.Length; i++)
        {
            Vector2 direction = 
                new Vector2(
                    Mathf.Cos( (startAngle + spreadInterval * i )* Mathf.Deg2Rad),
                    Mathf.Sin( (startAngle + spreadInterval * i )* Mathf.Deg2Rad)
                ).normalized;

            positions[i] = (Vector2)transform.position + direction * gunDistance;

            quadData.guns[i].transform.position = Vector2.Lerp(quadData.guns[i].transform.position, positions[i], Kp);
            quadData.guns[i].transform.rotation = Quaternion.Slerp(quadData.guns[i].transform.rotation, Quaternion.LookRotation(Vector3.forward, direction), Kp);
        }


        if (currentBullets <= 0){
            End();
            return;
        }

        if(timeActive > 1) FiringUpdate();

        timeActive += Time.deltaTime;
    }
    
    void FiringUpdate() {
        if (currentFireInterval <= 0){
            
            foreach (QuadShooter.GunObject source in quadData.guns){
                source.shooterScript.Shoot();
            }
            
            currentBullets--;
            currentFireInterval = fireRate;
            return;
        }
        currentFireInterval -= Time.deltaTime;
    }

}