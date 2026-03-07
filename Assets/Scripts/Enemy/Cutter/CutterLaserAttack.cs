using System.Collections;
using UnityEngine;

[System.Serializable]
public class CutterLaserAttack : BossStateData
{
    public CutterBoss cutterBoss;
    public GameObject laser, beam_indicator_prefab;
    public Vector3 locked_rotation;
    public float lazer_size, laserWindupTime, laserDiminishTime;
    public int iterations;

    public CutterLaserAttack(CutterBoss bossBase, float lazer_size = 20, float laserWindupTime = 1.3f, int iterations = 1, float laserDiminishTime = 2f) : base(bossBase) // Always include super(bossBase) in any child class constructors 
    {
        // IMPORTANT - THIS IS NOT USED TO REINITIALIZE OR RESET THE ATTACK FOR MULTIPLE USES. USE OnReset() TO REASSIGN DEFAULT VALUES ON STARTUP! (example: timers, counters, end conditions, etc.)
        cutterBoss = bossBase;

        laser = bossBase.laser;
        beam_indicator_prefab = bossBase.beam_indicator_prefab;
        this.lazer_size = lazer_size;
        this.laserWindupTime = laserWindupTime;
        this.laserDiminishTime = laserDiminishTime;
        this.iterations = iterations;

        OnReset();
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
    }

    // Called When the state object becomes active
    public override void Start()
    {
        movement_script.maxSpeed = 0.5f;
        movement_script.enableRotation = false;
        cutterBoss.StartCoroutine(nameof(LaserAttackNumerator));

        // if(health.CurrentHealth / (float)health.maxHealth< 0.35f && !cutterBoss.chaining && cutterBoss.chainCooldown == 0){
        //     cutterBoss.chaining = true;
        //     cutterBoss.chainCount = Random.Range(1, 3);
        // }
    }

    // Called every frame while the object is active
    public override void Update()
    {
        transform.localEulerAngles = locked_rotation;
    }

    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {

        cutterBoss.StopCoroutine(nameof(LaserAttackNumerator));
        base.End();
    }

    public IEnumerator LaserAttackNumerator()
    {
        for (int i = 0; i < iterations; i++)
        {
            var dir = Player.main.tf.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            locked_rotation = transform.localEulerAngles;


            float timer = laserWindupTime;
            float normalized_timer = timer / laserWindupTime;
            Vector3 target_position = new Vector3(0, 200, 0);
            Vector3 initial_position = new Vector3(0, -200, 0);
            // Vector3 indicator_position;

            beam_indicator_prefab.SetActive(true);
            beam_indicator_prefab.transform.localPosition = new Vector3(0, 0, 0);
            beam_indicator_prefab.transform.localEulerAngles = new Vector3(0, 0, 0);


            while (timer > 0)
            {
                normalized_timer = timer / laserWindupTime;
                beam_indicator_prefab.transform.localPosition = Vector3.Lerp(initial_position, target_position, 1 - (normalized_timer));

                timer -= Time.deltaTime;
                yield return null;
            }

            beam_indicator_prefab.SetActive(false);
            PulseEffectManager.instance.AddEffect(transform.position, 0.06f, 1, 0.15f);

            // yield return new WaitForSeconds(laser_time);

            laser.SetActive(true);
            laser.transform.localScale = new Vector3(lazer_size, 100, 1);

            cutterBoss.audio.clip = cutterBoss.clip;
            cutterBoss.audio.pitch = Random.Range(0.9f, 1.1f);
            cutterBoss.audio.Play();

            while (laser.transform.localScale.x > 0)
            {
                laser.transform.localScale -= new Vector3(Time.deltaTime * (lazer_size / laserDiminishTime), 0, 0);
                yield return null;
            }

            laser.transform.localScale = new Vector3(0, 100, 0);
            laser.SetActive(false);

            cutterBoss.audio.Stop();
        }

        End();
    }
}