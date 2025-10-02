using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CutterBoss : BossBase
{
    public Transform player;
    public bool[] states;
    public GameObject laser, bomb_prefab, beam_indicator_prefab, dashParticlesA, dashParticlesB;


    [SerializeField] public GameObject[] bombs; 


    [SerializeField] public float current_chase_time, chase_time, bomb_radius, lazer_size, laser_time, bombs_time_offset, quick_bombs_radius, predictor_coefficient;

    [SerializeField]
    public int bombs_int, quick_bombs_int, bombs_iterations, dashesIterations, chainCount, chainCooldown;
    public int bombs_iterations_counter;

    public Vector3 locked_rotation;
    public AudioSource audio, dashAudio;
    public AudioClip clip;

    [SerializeField]
    public Vector3[] bomb_coordinates;


    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Color defaultColor;
    [SerializeField] public bool instantDashes, chaining;


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

    public bool CheckChain(){
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

    // public IEnumerator LaserAttackNumerator()
    // {
    //     // // if(currentState is CutterLaserAttack)
    //     // {
    //     //     yield return (currentState as CutterLaserAttack).LaserAttackNumerator();
    //     // }
    // }

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
    