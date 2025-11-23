
using System.Collections;
using UnityEngine;

[System.Serializable]
public class QuadShooterMainAttack : BossStateData
{

    public QuadShooter quadData;
    public float fireRate;
    public int bullets, currentBullets;
    public float currentFireInterval;

    public QuadShooterMainAttack(QuadShooter bossBase) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;

        // this.maxSpeed = ;
        bullets = 16;
        fireRate = bossBase.fireRateCurve.Evaluate(1 - bossBase.normalizedHealth);
        
        currentBullets = bullets;
    }

    public override void Start() // Called When the state object becomes active
    {
        fireRate = quadData.fireRateCurve.Evaluate(1 - quadData.normalizedHealth);
        currentFireInterval = fireRate;
        currentBullets = bullets;
    }

    public override void Update() // Called every frame while the object is active
    {
        if (currentBullets <= 0){
            End();
            return;
        }

        FiringUpdate();

        foreach (QuadShooter.GunObject source in quadData.guns){
            source.transform.LookAt(Player.main.tf);
        }
    }
    
    void FiringUpdate() {
        if (currentFireInterval <= 0){
            quadData.StartCoroutine(FireCoroutine());
            currentFireInterval = fireRate;
        }
        else
        {
            currentFireInterval -= Time.deltaTime;
        }
    }

    IEnumerator FireCoroutine(){
        foreach (QuadShooter.GunObject source in quadData.guns)
        {
            source.shooterScript.Shoot();
            yield return new WaitForSeconds(fireRate/4);
        }
        currentBullets--;
    }
}