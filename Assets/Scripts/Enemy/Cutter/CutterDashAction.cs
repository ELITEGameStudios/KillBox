using System.Collections;
using UnityEngine;

[System.Serializable]
public class CutterDashAction : BossStateData
{
    public CutterBoss cutterBoss;
    [SerializeField] int iterations;
    [SerializeField] float interval;
    [SerializeField] bool instantDash;
    // [SerializeField] float predictor_coefficient;

    public CutterDashAction(CutterBoss bossBase, int iterations, float interval, bool instantDash = false) : base(bossBase) // Always include super(bossBase) in any child class constructors 
    {
        // IMPORTANT - THIS IS NOT USED TO REINITIALIZE OR RESET THE ATTACK FOR MULTIPLE USES. USE OnReset() TO REASSIGN DEFAULT VALUES ON STARTUP! (example: timers, counters, end conditions, etc.)
        cutterBoss = bossBase;

        this.iterations = iterations;
        this.interval = interval;
        this.instantDash = instantDash;


        OnReset();
    }

    void Dash()
    {
        cutterBoss.dashParticlesA.SetActive(false);
        cutterBoss.dashParticlesB.SetActive(false);
        movement_script.maxSpeed = 0.5f;


        // if(health.CurrentHealth / (float)health.maxHealth < 0.4f && !cutterBoss.chaining && chainCooldown == 0){
        //     chaining = true;
        //     chainCount = Random.Range(2, 5);
        // }

        cutterBoss.StartCoroutine(nameof(DashAttackNumerator));
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
    }

    // Called When the state object becomes active
    public override void Start()
    {

        movement_script.maxSpeed = 5f;
        movement_script.enableRotation = false;

    }

    // Called every frame while the object is active
    public override void Update(){}

    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        cutterBoss.StopCoroutine(nameof(DashAttackNumerator));
        base.End();
    }
    
    public IEnumerator DashAttackNumerator()
    {
        SpriteRenderer renderer = cutterBoss.renderer;
        GameObject dashParticlesA = cutterBoss.dashParticlesA;
        GameObject dashParticlesB = cutterBoss.dashParticlesB;
        AudioSource dashAudio = cutterBoss.dashAudio;

        for (int i = 0; i < iterations; i++)
        {

            Vector2 targetPos = DashLocationManager.cutterMap.getClosestValidPosition(false, 5).position;
            renderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            renderer.color = Color.clear;
            yield return new WaitForSeconds(0.06f);
            renderer.color = Color.white;
            yield return new WaitForSeconds(0.06f);

            float angle = Vector2.SignedAngle(transform.position, Player.main.tf.position) * Mathf.Deg2Rad;

            dashParticlesB.transform.position = transform.position;
            yield return null;

            dashParticlesA.SetActive(true);
            dashParticlesB.SetActive(true);

            dashAudio.Stop();
            dashAudio.time = 0;

            dashParticlesA.transform.LookAt(Player.main.tf.position);
            dashParticlesA.transform.Rotate(new Vector3(0, 0, angle));

            yield return null;

            dashParticlesA.transform.position = transform.position;

            dashAudio.pitch = Random.Range(0.8f, 1.2f);
            dashAudio.Play();

            dashParticlesB.transform.LookAt(Player.main.tf.position);
            dashParticlesB.transform.Rotate(new Vector3(0, 0, angle + 180));

            transform.position = targetPos;
            dashParticlesB.transform.position = transform.position;

            
            float timer = 1f;
            while (timer > 0 && !instantDash){
                timer -= Time.deltaTime;
                renderer.color = Color.Lerp(cutterBoss.defaultColor, Color.white, timer);
                yield return null;
            }

            renderer.color = cutterBoss.defaultColor;
            
            yield return new WaitForSeconds(interval);
        }
    }
}