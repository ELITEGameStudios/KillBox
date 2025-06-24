
using UnityEngine;

[System.Serializable]
public class ShardSphericalAttack : BossStateData
{
    ShardBoss shardData;
    public AIShooterScript[] shoot_sources;


    [Header("Angular movement data")]
    public float angularSpeed;
    public float maxDist, minDist;
    public float distance;
    private float angleDegrees;


    [Header("Enter data")]
    public AnimationCurve enterCurve;
    public float enterTime;


    [Header("Fire data")]
    public float fireTime;
    public float fireInterval, currentFireInterval;


    [Header("Exit data")]
    public float exitTime;


    [Header("State machine data")]
    public SubState state;
    public float timeInState;



    public enum SubState {
        ENTERING,
        FIRING,
        EXITING
    }


    public ShardSphericalAttack(ShardBoss bossBase, float angularSpeed, float maxDist, float minDist, AnimationCurve enterCurve, float enterTime, float fireTime, float exitTime, float fireInterval) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        shardData = bossBase;

        this.angularSpeed = angularSpeed;
        this.maxDist = maxDist;
        this.minDist = minDist;

        this.enterCurve = enterCurve;

        this.enterTime = enterTime;
        this.fireTime = fireTime;
        this.exitTime = exitTime;
        this.fireInterval = fireInterval;
        
        shoot_sources = shardData.shoot_sources;
    }

    public override void Start() // Called When the state object becomes active
    {
        SwitchSubState(SubState.ENTERING);
        shardData.rotator.SetRotationRate(shardData.Aspeed[0], 1.5f);

        angleDegrees = Vector2.Angle(Player.main.tf.position, transform.position);
        distance = maxDist;
        rb_self.velocity = Vector2.zero;

        transform.position = new Vector2(
            Mathf.Cos(angleDegrees * Mathf.Deg2Rad),
            Mathf.Sin(angleDegrees * Mathf.Deg2Rad)
        ) * distance;

    }
    public override void Update() // Called every frame while the object is active
    {
        if (state == SubState.FIRING)
        {
            FiringUpdate();
        }
    }

    public override void FixedUpdate() // Called every frame while the object is active
    {


        switch (state)
        {
            case SubState.ENTERING:
                EnteringUpdate();
                break;
            case SubState.EXITING:
                ExitingUpdate();
                break;
        }


        // Rotation update
        angleDegrees += angularSpeed * Time.fixedDeltaTime;
        // angleDegrees %= 360;
        
        transform.position = new Vector2(
            Mathf.Cos(angleDegrees * Mathf.Deg2Rad),
            Mathf.Sin(angleDegrees * Mathf.Deg2Rad)
            ) * distance;
    }

    void SwitchSubState(SubState state)
    {
        this.state = state;
        timeInState = 0;
        currentFireInterval = fireInterval;
    }


    void EnteringUpdate() {

        if (timeInState > enterTime)
        {
            SwitchSubState(SubState.FIRING);
            return;   
        }

        distance = Mathf.Lerp(minDist, maxDist, enterCurve.Evaluate(timeInState/enterTime));
        timeInState += Time.fixedDeltaTime;
    }

    void FiringUpdate() {
        if (timeInState > fireTime)
        {
            SwitchSubState(SubState.EXITING);
            return;
        }
        timeInState += Time.deltaTime;

        if (currentFireInterval <= 0)
        {
            foreach (AIShooterScript source in shoot_sources)
            {
                source.Shoot();
            }
            currentFireInterval = fireInterval - fireInterval / 2 * (timeInState / fireTime);
        }
        else
        {
            currentFireInterval -= Time.deltaTime;
        }
    }
    void ExitingUpdate() {
        if (timeInState > exitTime)
        {
            End();
            movement_script.enabled = true;
            movement_script.updatePosition = true;

            return;   
        }

        distance = Mathf.Lerp(maxDist, minDist, enterCurve.Evaluate(timeInState/exitTime));
        timeInState += Time.deltaTime;
    }
}