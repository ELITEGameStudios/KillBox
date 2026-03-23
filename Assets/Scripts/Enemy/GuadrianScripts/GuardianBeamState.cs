using UnityEngine;

[System.Serializable]
public class GuardianBeamState : GuardianStateData
{
    float targetTime, currentTime, rotationSpeed, beamDistance, width, lastAngle;
    Vector2 targetPos, startLerpPos;
    int iterations, currentIterations;
    
    

    public GuardianBeamState(GuardianBoss bossBase, string stateTag, int iterations = 3, float targetTime = 4, float rotationSpeed = 40f, float beamDistance = 10f, float width = 2, int maxEntities = 1) : base(bossBase, stateTag, maxEntities)
    {
        bossData = bossBase;
        this.targetTime = targetTime;
        this.iterations = iterations;
        this.beamDistance =beamDistance;
        this.rotationSpeed = rotationSpeed;
        this.width = width;
    }

    public override void Start() // Called When the state object becomes active
    {
        currentIterations = 0;
        currentTime = 0;
        lastAngle = 0;
        NewBeam();
    }
    public override void Update() // Called every frame while the object is active
    {
        currentTime += Time.deltaTime;
        if(currentTime > targetTime)
        {
            currentTime = 0;
            if(currentIterations >= iterations) {End();}
            else { NewBeam(); }
        }
        else
        {
            transform.position = Vector2.Lerp(startLerpPos, targetPos, bossData.beamPositionLerpAnimCurve.Evaluate(currentTime*2 / targetTime));
        }
    }
    
    void NewBeam()
    {
        float newPosAngle = (lastAngle + Random.Range(-150, 150)) % 360;

        Vector2 newPos = new Vector2(Mathf.Cos(newPosAngle * Mathf.Deg2Rad), Mathf.Sin(newPosAngle * Mathf.Deg2Rad)) * beamDistance;

        targetPos = (Vector2)Player.main.tf.position + newPos;  
        startLerpPos = transform.position;  
        rb_self.velocity = Vector2.zero;

        movement_script.maxSpeed = 1;
        movement_script.maxAcceleration = 1;

        bossData.rotator.SetRotationRate(Random.Range(0, 2) == 0 ? rotationSpeed : -rotationSpeed, 1f);
        foreach (GuardianBeam beam in bossData.beams){
            beam.gameObject.SetActive(true);
            beam.BeginSequence(width, targetTime/2);
        }
        
        lastAngle = newPosAngle;
        currentIterations++;
    }

    public override void End(bool interrupted = false)
    {
        base.End(interrupted);
    }
}