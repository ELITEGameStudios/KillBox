
using System.Collections;
using UnityEngine;

[System.Serializable]
public class QuadShooterMainAttack : BossStateData
{

    public QuadShooter quadData;
    public float fireRate => 1 -quadData.fireRateCurve.Evaluate(quadData.normalizedHealth);
    public int bullets, currentBullets;
    public float currentFireInterval, Kp = 0.1f;
    private bool inCoroutine;

    public QuadShooterMainAttack(QuadShooter bossBase) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;

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
        if(quadData.guns[0].transform.parent == transform)
        {
            for (int i = 0; i < quadData.guns.Length; i++){
                quadData.guns[i].transform.SetParent(quadData.mainGunPositions[i]);
            }
        }
    }

    public override void Update() // Called every frame while the object is active
    {
        
        if (currentBullets <= 0){
            End();
            return;
        }

        for (int i = 0; i < quadData.guns.Length; i++){
            if(quadData.guns[i].transform.localPosition.magnitude > 0.05f)
            {
                quadData.guns[i].transform.localPosition = Vector2.Lerp(quadData.guns[i].transform.localPosition, Vector2.zero, Kp);
            }
        }

        FiringUpdate();

        foreach (QuadShooter.GunObject source in quadData.guns){
            source.transform.LookAt(Player.main.tf);
            Vector2 target = (Player.main.tf.position - source.transform.position).normalized;
            source.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
            // Debug.Log("uhuh");
        }
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