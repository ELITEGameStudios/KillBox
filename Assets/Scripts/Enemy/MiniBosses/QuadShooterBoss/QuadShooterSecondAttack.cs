
using System.Collections;
using UnityEngine;

[System.Serializable]
public class QuadShooterSecondAttack : BossStateData
{

    public QuadShooter quadData;
    public float fireRate = 1f, gunDistance, Kp = 0.01f;
    public int bullets, currentBullets;
    public float currentFireInterval, maxSpread, timeActive;
    private bool inCoroutine;

    public Vector2[] positions;

    public QuadShooterSecondAttack(QuadShooter bossBase, float maxSpread = 120) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;
        this.maxSpread = maxSpread;

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
        float startPoint = -(float)positions.Length/2;
        
        Vector2 playerDir = (Player.main.tf.position - transform.position).normalized;
        float playerAngle = Vector2.SignedAngle(Vector2.up, playerDir);

        float startAngle = playerAngle - startPoint;

        for (int i = 0; i < positions.Length; i++)
        {
            Vector2 direction = 
                new Vector2(
                    Mathf.Cos((startAngle + spreadInterval * i )* Mathf.Rad2Deg),
                    Mathf.Sin((startAngle + spreadInterval * i )* Mathf.Rad2Deg)
                );
            positions[i] = (Vector2)transform.position + direction * gunDistance;

            quadData.guns[i].transform.position = Vector2.Lerp(quadData.guns[i].transform.position, positions[i], Kp);
            quadData.guns[i].transform.rotation = Quaternion.Slerp(quadData.guns[i].transform.rotation, Quaternion.LookRotation(direction, Vector3.forward), Kp);
        }


        if (currentBullets <= 0){
            End();
            return;
        }

        if(timeActive > 1) FiringUpdate();

        foreach (QuadShooter.GunObject source in quadData.guns){
            source.transform.LookAt(Player.main.tf);
            Vector2 target = (Player.main.tf.position - source.transform.position).normalized;
            source.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
        }

        timeActive += Time.deltaTime;
    }
    
    void FiringUpdate() {
        if (currentFireInterval <= 0 && !inCoroutine){
            quadData.StartCoroutine(FireCoroutine());
            currentFireInterval = fireRate;
            inCoroutine = true;
            
            return;
        }
        currentFireInterval -= Time.deltaTime;
    }

    IEnumerator FireCoroutine(){
        foreach (QuadShooter.GunObject source in quadData.guns)
        {
            source.shooterScript.Shoot();
            Debug.Log("Yeieileding");
            yield return new WaitForSeconds(fireRate/4);
        }
        currentBullets--;
        inCoroutine = false;
    }
}