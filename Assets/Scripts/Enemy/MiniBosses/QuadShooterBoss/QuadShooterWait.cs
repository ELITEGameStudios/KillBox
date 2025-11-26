
using UnityEngine;

[System.Serializable]
public class QuadShooterWait : BossStateData
{

    public QuadShooter quadData;
    private float time, currentTime;
    private float maxSpeed, Kp = 0.01f;

    public QuadShooterWait(QuadShooter bossBase, float time, float maxSpeed = 2) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        quadData = bossBase;

        this.maxSpeed = maxSpeed;
        this.time = time;
    }

    public override void Start() // Called When the state object becomes active
    {
        currentTime = time;
        
        if(quadData.guns[0].transform.parent == null){
            for (int i = 0; i < quadData.guns.Length; i++){
                quadData.guns[i].transform.SetParent(quadData.mainGunPositions[i]);
            }
        }
    }
    
    public override void Update() // Called every frame while the object is active
    {   
        for (int i = 0; i < quadData.guns.Length; i++){
            if(quadData.guns[i].transform.localPosition.magnitude > 0.05f)
            {
                quadData.guns[i].transform.localPosition = Vector2.Lerp(quadData.guns[i].transform.localPosition, Vector2.zero, Kp);
            }
        }
        
        foreach (QuadShooter.GunObject source in quadData.guns){
            source.transform.LookAt(Player.main.tf);
            Vector2 target = (Player.main.tf.position - source.transform.position).normalized;
            source.transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
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