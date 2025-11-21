
using UnityEngine;

[System.Serializable]
public class QuadShooterWait : BossStateData
{

    QuadShooter quadData;
    private float time, currentTime;
    private float maxSpeed;

    public QuadShooterWait(QuadShooter bossBase, float time, float maxSpeed = 2) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;

        this.maxSpeed = maxSpeed;
        this.time = time;
    }

    public override void Start() // Called When the state object becomes active
    {
        currentTime = time;
    }
    
    public override void Update() // Called every frame while the object is active
    {
        if (bullets <= 0){
            End();
            return;
        }
        

        if (currentTime > 0)
        {
            Debug.Log("chase phase update");
            currentTime -= Time.deltaTime;
        }
        else
        {
            End();
            Debug.Log("Ended chase phase");
        }
    }
}