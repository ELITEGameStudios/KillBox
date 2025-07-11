
using UnityEngine;
using static PrologueBoss;

[System.Serializable]
public class PrologueDrainAttack : BossStateData
{


    PrologueBoss prologueData;
    private float drainTime, currentLaserTime;
    private bool fires;
    public AIShooterScript[] shoot_sources;
    public float fireRate;
    public float currentFireInterval;
    public DebuffType debuffType;
    



    public PrologueDrainAttack(PrologueBoss bossBase, float drainTime, DebuffType debuffType, bool fires = false) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        prologueData = bossBase;

        this.drainTime = drainTime;
        this.fires = fires;
        this.fireRate = 0.75f;

        this.debuffType = debuffType;
    }

    public override void Start() // Called When the state object becomes active
    {
        PrologueArenaSpawnSystem.SpawnEnemies(prologueData.drainSpawn, 1, drainTime, 0, Random.Range(0, 2) == 1);
        currentLaserTime = drainTime;

        // if (fires)
        // {
        //     currentFireInterval = fireRate;
        // }
    }
    public override void Update() // Called every frame while the object is active
    {

        // Drain function
        if (prologueData.SetDrainGraphic(debuffType))
        {
            // If the player is hit
            switch (debuffType)
            {
                case DebuffType.HEALTH:
                    Player.main.health.AddDebuffHealth(10 * Time.deltaTime);
                    break;

                case DebuffType.SPEED:
                    Player.main.movement.AddDebuff(1 * Time.deltaTime);
                    break;

                // case DebuffType.CAPACITY:
                //
                //     break;
                
                // case DebuffType.LIFESTEAL:
                //
                //     break;
            }

        }

        // State timer
        if (currentLaserTime > 0)
        {
            Debug.Log("drain update");
            currentLaserTime -= Time.deltaTime;
        }
        else
        {
            End();
            Debug.Log("Ended drain phase");
        }
    }
    
    // void FiringUpdate() {
    //     if (currentFireInterval <= 0)
    //     {
    //         // foreach (AIShooterScript source in prologueData.shoot_sources)
    //         // {
    //         //     source.Shoot();
    //         // }
    //         currentFireInterval = fireRate;
    //     }
    //     else
    //     {
    //         currentFireInterval -= Time.deltaTime;
    //     }
    // }
}