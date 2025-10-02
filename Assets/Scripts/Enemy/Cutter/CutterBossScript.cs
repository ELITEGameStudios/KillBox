using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CutterBossScript : MonoBehaviour
{
    public bool[] states;
    public AIPath movement_script;
    public GameObject laser, bomb_prefab, beam_indicator_prefab, dashParticlesA, dashParticlesB;

    [SerializeField]
    private GameObject[] bombs;

    public Transform player;

    [SerializeField]
    private float current_chase_time, chase_time, bomb_radius, lazer_size, laser_time, bombs_time_offset, quick_bombs_radius, predictor_coefficient;

    [SerializeField]
    private int bombs_int, quick_bombs_int, bombs_iterations, dashesIterations, chainCount, chainCooldown;
    private int bombs_iterations_counter;

    public Vector3 locked_rotation;
    public AudioSource audio, dashAudio;
    public AudioClip clip;

    [SerializeField]
    private Vector3[] bomb_coordinates;

    [SerializeField]
    private string name;

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Color defaultColor;
    [SerializeField] private bool instantDashes, chaining;
    [SerializeField] private EnemyHealth health;

    public Sprite displaySprite;
    public Color displayColor;


    // Start is called before the first frame update
    void Start()
    {
        states = new bool[5];
        Chase();
        player = GameObject.FindWithTag("Player").transform;

        defaultColor = renderer.color;

        //BossAudio.Instance.OnShardSpawn(gameObject);
        BossBarManager.Instance.AddToQueue(gameObject, name, displayColor, displaySprite);

        dashParticlesA.transform.SetParent(null);
        dashParticlesB.transform.SetParent(null);

        dashParticlesA.SetActive(false);
        dashParticlesB.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(current_chase_time > 0 && states[0])
        { current_chase_time -= Time.deltaTime; }

        else if(current_chase_time < 0 && states[0])
        { PickState(3); }

        if (states[1])
        { transform.localEulerAngles = locked_rotation; }

        if (states[3] && bombs_iterations_counter <= 0)
        { CancelInvoke(); PickState(); }
        //Vector3 new_direction = Vector3.RotateTowards(laser.transform.forward, player.position - laser.transform.position, 6.28319f, 0.0f);
        //laser.transform.rotation = Quaternion.LookRotation(new_direction);
    }

    void PickState(int state = -1)
    {
        for(int i = 0; i < states.Length; i++) { states[i] = false; }
        if(state == -1){ states[Random.Range(1, 5)] = true; }
        else{ states[state] = true; }

        if(health.CurrentHealth / (float)health.maxHealth < 0.4f && !instantDashes){
            instantDashes = true;
        }

        if (states[1])
        { LaserAttack(); }

        else if (states[2])
        { BomberAttack(); }
        
        else if (states[3])
        { ChaseWithBombs(); }

        else if (states[4])
        { Dash(); }
        
    }

    void Chase()
    {
        StopCoroutine("LaserAttackNumerator");
        movement_script.maxSpeed = 6f;
        movement_script.enableRotation = false;
        current_chase_time = chase_time;

        for (int i = 0; i < states.Length; i++)
        {
            states[i] = false;
        }

        states[0] = true;
    }
    void ChaseWithBombs()
    {
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = false;
        }

        states[3] = true;

        movement_script.maxSpeed = 5f;
        movement_script.enableRotation = false;
        bombs_iterations_counter = bombs_iterations;

        InvokeRepeating("QuickBomb", 0.6f, 0.6f);
    }

    void Dash()
    {
        dashParticlesA.SetActive(false);
        dashParticlesB.SetActive(false);
        // movement_script.maxSpeed = 0.5f;
        if(health.CurrentHealth / (float)health.maxHealth < 0.4f && !chaining && chainCooldown == 0){
            chaining = true;
            chainCount = Random.Range(2, 5);
        }

        StartCoroutine("DashAttackNumerator");
        
    }

    void LaserAttack()
    {
        movement_script.maxSpeed = 0.5f;
        movement_script.maxSpeed = 0.5f;
        movement_script.enableRotation = false;
        StartCoroutine("LaserAttackNumerator");

        if(health.CurrentHealth / (float)health.maxHealth< 0.35f && !chaining && chainCooldown == 0){
            chaining = true;
            chainCount = Random.Range(1, 3);
        }

        var dir = player.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        locked_rotation = transform.localEulerAngles;
    }

    void QuickBomb(){
        bomb_coordinates = new Vector3[quick_bombs_int];
        bombs = new GameObject[quick_bombs_int];

        Vector3 target = Player.main.tf.position + (Vector3)((Player.main.rb.velocity/Time.fixedDeltaTime) *predictor_coefficient );

        for(int i = 0; i < bomb_coordinates.Length; i++)
        {
            bomb_coordinates[i] = target + new Vector3(Random.Range(-quick_bombs_radius, quick_bombs_radius), Random.Range(-quick_bombs_radius, quick_bombs_radius), 0);
            bombs[i] = Instantiate(bomb_prefab, bomb_coordinates[i], transform.rotation);
            bombs[i].SetActive(true);
        }

        bombs_iterations_counter--;
    }

    bool CheckChain(){
        if(chaining){
            if(chainCount > 0){
                chainCount--;
                return true;
            }
            chaining = false;
            chainCooldown = Random.Range(1, 3);
        }
        if(chainCooldown > 0){
            chainCooldown--;
        }
        return false;
    }

    void BomberAttack()
    {
        movement_script.maxSpeed = 0.5f;

        bomb_coordinates = new Vector3[bombs_int];
        bombs = new GameObject[bombs_int];

        for(int i = 0; i < bomb_coordinates.Length; i++)
        {
            bomb_coordinates[i] = player.transform.position + new Vector3(Random.Range(-bomb_radius, bomb_radius), Random.Range(-bomb_radius, bomb_radius), 0);
            bombs[i] = Instantiate(bomb_prefab, bomb_coordinates[i], transform.rotation);
            bombs[i].SetActive(true);
        }

        StartCoroutine("BomberAttackNumerator");
    }

    IEnumerator LaserAttackNumerator()
    {
        float timer = laser_time;
        float normalized_timer = timer/laser_time;
        Vector3 target_position = new Vector3(0, 200, 0);
        Vector3 initial_position = new Vector3(0, -200, 0);
        Vector3 indicator_position;

        beam_indicator_prefab.SetActive(true);
        beam_indicator_prefab.transform.localPosition =  new Vector3(0, 0, 0); 
        beam_indicator_prefab.transform.localEulerAngles = new Vector3(0, 0, 0);

        while (timer > 0){
            normalized_timer = timer/laser_time;
            beam_indicator_prefab.transform.localPosition = Vector3.Lerp(initial_position, target_position, 1-(normalized_timer));
            
            timer -= Time.deltaTime;
            yield return null;
        }

        beam_indicator_prefab.SetActive(false);

        //yield return new WaitForSeconds(laser_time);
        
        laser.SetActive(true);
        laser.transform.localScale = new Vector3(lazer_size, 100, 1);

        audio.clip = clip;
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.Play();

        while (laser.transform.localScale.x > 0)
        {
            laser.transform.localScale -= new Vector3(Time.deltaTime * (lazer_size/2), 0, 0);
            yield return null;
        }

        laser.transform.localScale = new Vector3 (0, 100, 0);
        laser.SetActive(false);

        audio.Stop();
        
        if(CheckChain()){
            PickState(1);
        }
        else{
            if(health.CurrentHealth / (float)health.maxHealth < 0.4f){

                PickState(Random.Range(3, 5));
            }
            else{

                PickState(health.CurrentHealth / (float)health.maxHealth < 0.65f ? 4 : 0);
            }
            // int newState = Random.Range(1, 5);
            // PickState(newState == 0 ? 1 : 3);
        }
        // int newState = Random.Range(0, 2);

    }

    IEnumerator DashAttackNumerator()
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
        while (timer > 0 && !instantDashes){
            timer -= Time.deltaTime;
            renderer.color = Color.Lerp(defaultColor, Color.white, timer);
            yield return null;
        }

        renderer.color = defaultColor;

        
        if(CheckChain()){
            PickState(4);
        }
        else{
            if(health.CurrentHealth / (float)health.maxHealth < 0.4f){

                PickState(Random.Range(1, 5));
            }
            else{

                PickState(health.CurrentHealth / (float)health.maxHealth < 0.65f ? 2 : 3);
            }
            // int newState = Random.Range(1, 5);
            // PickState(newState == 0 ? 1 : 3);
            // PickState(newState);
        }


    }

    IEnumerator BomberAttackNumerator()
    {


        yield return new WaitForSeconds(0.7f);

        //laser.SetActive(true);
        //laser.transform.localScale = new Vector3(15, 100, 1);
        //
        //audio.pitch = Random.Range(0.9f, 1.1f);
        //audio.Play();
        //
        //while (laser.transform.localScale.x > 0)
        //{
        //    laser.transform.localScale -= new Vector3(Time.deltaTime * 7.5f, 0, 0);
        //    yield return null;
        //}
        //
        //laser.transform.localScale = new Vector3(0, 100, 0);
        //laser.SetActive(false);
        PickState(1);

    }
}   
    