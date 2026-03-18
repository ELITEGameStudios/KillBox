using UnityEngine;

[System.Serializable]
public class GuardianMainState : GuardianStateData
{
    // float currentFireInterval;
    float timeElapsedInState, targetTime, startShootTime, rotationSpeed, movementSpeed, acceleration;
    bool shooting, whip;
    AIShooterScript[] shoot_sources;
    
    public GuardianMainState(GuardianBoss bossBase, string stateTag, float movementSpeed = 3f, float acceleration = 2f, float targetTime = 4, float rotationSpeed = 180f, float startShootingTime = 1.2f, bool whip = false, int maxEntities = -1) : base(bossBase, stateTag, maxEntities)
    {
        bossData = bossBase;
        this.targetTime = targetTime;
        this.rotationSpeed = rotationSpeed;
        this.movementSpeed =movementSpeed;
        this.acceleration =acceleration;
        this.whip = whip;
        shoot_sources = bossData.shoot_sources;
        startShootTime = startShootingTime;
    }

    public override void Start() // Called When the state object becomes active
    {
        bossData.movement_script.maxSpeed = movementSpeed;
        bossData.movement_script.maxAcceleration = acceleration;

        // bossData.rotationSpeed = Random.Range(0, 2) == 0 ? rotationSpeed : 450;
        bossData.rotator.SetRotationRate(rotationSpeed, 1);
        
        if (whip && targetTime > 2){
            bossData.StartCoroutine(bossData.WhipAttack(targetTime - 2));
        }

        timeElapsedInState = 0;
    }
    public override void Update() // Called every frame while the object is active
    {
        timeElapsedInState += Time.deltaTime;
        if (!shooting)
        {
            if(timeElapsedInState > startShootTime)
            {
                ToggleFiring(true);
            }
        }
        else if (timeElapsedInState > targetTime - 1)
        {
            ToggleFiring(false);
        }

        if (timeElapsedInState > targetTime)
        {
            End();
        }
    }
    
    void ToggleFiring(bool fire) {
        for (int i = 0; i < shoot_sources.Length; i++)
        {
            shoot_sources[i].enabled = fire;
            shoot_sources[i].CanShoot = fire;
        }
        shooting = fire;
    }

    public override void End(bool interrupted = false)
    {
        ToggleFiring(false);
        base.End(interrupted);
    }
}