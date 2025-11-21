
using UnityEngine;

[System.Serializable]
public class QuadShooterMainAttack : BossStateData
{

    QuadShooter quadData;
    private float time, currentTime;
    private float maxSpeed;
    public float fireRate;
    public int bullets;
    public float currentFireInterval;

    public QuadShooterMainAttack(QuadShooter bossBase, int bullets = 16, float fireRate = 0.1f, float maxSpeed = 2) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;

        this.maxSpeed = maxSpeed;
        this.time = time;
        this.bullets = bullets;
        this.fireRate = fireRate;
    }

    public override void Start() // Called When the state object becomes active
    {
        currentTime = time;
        currentFireInterval = fireRate;
    }
    public override void Update() // Called every frame while the object is active
    {
        if (bullets <= 0){
            End();
            return;
        }

        FiringUpdate();

        foreach (AIShooterScript source in quadData.guns)
        {
            source.transform.LookAt(Player.main.tf);
            yield return new WaitForSeconds(fireRate/4);
        }
    }
    
    void FiringUpdate() {
        if (currentFireInterval <= 0){
            StartCoroutine(FireCoroutine());
            currentFireInterval = fireRate;
        }
        else
        {
            currentFireInterval -= Time.deltaTime;
        }
    }

    IENumerator FireCoroutine(){
        foreach (AIShooterScript source in quadData.guns)
        {
            source.shooterScript.Shoot();
            yield return new WaitForSeconds(fireRate/4);
        }
        bullets--;
    }
}