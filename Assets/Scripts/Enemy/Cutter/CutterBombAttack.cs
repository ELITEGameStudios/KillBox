using System.Collections;
using UnityEngine;

[System.Serializable]
public class CutterBombAttack : BossStateData
{
    public CutterBoss cutterBoss;
    public GameObject laser, beam_indicator_prefab;
    public float radius;
    public float time, timer;
    public int quantity;

    public CutterBombAttack(CutterBoss bossBase, float time = 0.7f, float radius = 7, int quantity = 12) : base(bossBase) // Always include super(bossBase) in any child class constructors 
    {
        // IMPORTANT - THIS IS NOT USED TO REINITIALIZE OR RESET THE ATTACK FOR MULTIPLE USES. USE OnReset() TO REASSIGN DEFAULT VALUES ON STARTUP! (example: timers, counters, end conditions, etc.)
        cutterBoss = bossBase;
        this.time = time;
        this.radius = radius;
        this.quantity = quantity;

        OnReset();
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
        timer = time;
    }

    // Called When the state object becomes active
    public override void Start()
    {
        movement_script.maxSpeed = 0.5f;

        Vector3[] bomb_coordinates = new Vector3[quantity];
        GameObject[] bombs = new GameObject[quantity];

        for(int i = 0; i < bomb_coordinates.Length; i++)
        {
            bomb_coordinates[i] = Player.main.tf.transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0);
            bombs[i] = Object.Instantiate(cutterBoss.bomb_prefab, bomb_coordinates[i], transform.rotation);
            bombs[i].SetActive(true);
        }
    }

    // Called every frame while the object is active
    public override void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0){ End(); }
    }

    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        base.End();
    }
}