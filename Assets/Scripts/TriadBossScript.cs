using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class TriadBossScript : MonoBehaviour
{
    public bool[] states;
    public AIPath movement_script;
    
    public GameObject minion_prefab, delta_prefab;

    [SerializeField]
    private GameObject[] clones;
    
    public List<GameObject> minions;
    [SerializeField]
    private Vector3[] clone_positions;

    public Transform player;

    [SerializeField]
    private float current_chase_time, chase_time, clone_distance;

    public Vector3 locked_rotation;
    public AudioSource audio;

    [SerializeField]
    private Vector3[] bomb_coordinates;

    private Rigidbody2D rb_self;

    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private Collider2D self_collider;

    [SerializeField]
    private float delta_random_pos, delta_distance, delta_time_between_bursts, delta_iteration_time, 
        culminative_delta_burst_time, culminative_delta_attack_time;

    [SerializeField]
    private int delta_burst_count, delta_burst_iterations, last_state;

    private float max_hp; 
    private float health;

    [SerializeField]
    private float health_ratio;

    private Color32 main_color, transparent_col;



    // Start is called before the first frame update
    void Start()
    {
        rb_self = gameObject.GetComponent<Rigidbody2D>();
        states = new bool[4];
        Chase();
        player = Player.main.tf;

        //BossAudio.Instance.OnShardSpawn(gameObject);
        BossBarManager.Instance.AddToQueue(gameObject, "ALPHA TRIAD");


        ConstraintSource constraint = new ConstraintSource();
        constraint.sourceTransform = player;
        constraint.weight = 1;

        gameObject.GetComponent<LookAtConstraint>().AddSource(constraint);

        max_hp = gameObject.GetComponent<EnemyHealth>().maxHealth; 
        health = gameObject.GetComponent<EnemyHealth>().CurrentHealth;
        health_ratio = health / max_hp;

        main_color = renderer.color;
        transparent_col = new Color32(
            main_color.r,
            main_color.g,
            main_color.b,
            55);

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.GetComponent<LookAtConstraint>() != null)
                transform.GetChild(i).gameObject.GetComponent<LookAtConstraint>().AddSource(constraint);
        }


    }

    // Update is called once per frame
    void Update()
    {
        max_hp = gameObject.GetComponent<EnemyHealth>().maxHealth; 
        health = gameObject.GetComponent<EnemyHealth>().CurrentHealth;
        health_ratio = health / max_hp;

        if(current_chase_time > 0 && states[0])
        {
            current_chase_time -= Time.deltaTime;

        }
        else if(current_chase_time < 0 && states[0])
        {
            PickState();
        }

        if (states[1])
        {
            transform.localEulerAngles = locked_rotation;
        }
        //Vector3 new_direction = Vector3.RotateTowards(laser.transform.forward, player.position - laser.transform.position, 6.28319f, 0.0f);
        //laser.transform.rotation = Quaternion.LookRotation(new_direction);
    }

    void PickState()
    {
        for(int i = 0; i < states.Length; i++)
        {
            states[i] = false;
        }

        // states[3] = true;
        if(health_ratio > 0.1f){
            int next_state = Random.Range(1, 4);
            if(last_state == next_state){ next_state = Random.Range(1, 4); }

            states[next_state] = true;
            last_state = next_state;
        }
            
        else{
            states[3] = true;
        }

        if (states[1])
        {
            GunAttack();
        }
        else if (states[2])
        {
            //GunAttack();
            SplitAttack();
        }
        else if (states[3])
        {
            //GunAttack();
            DeltaAttack();
        }
    }

    void Chase()
    {
        StopCoroutine("GunAttackNumerator");
        movement_script.maxSpeed = 3;
        current_chase_time = chase_time;

        for (int i = 0; i < states.Length; i++)
        {
            states[i] = false;
        }

        states[0] = true;
    }

    void GunAttack()
    {
        movement_script.maxSpeed = 0;
        movement_script.maxSpeed = 0;


        clone_positions = new Vector3[] {
            transform.position + new Vector3(clone_distance, clone_distance, 0),
            transform.position + new Vector3(-clone_distance, clone_distance, 0),
            transform.position + new Vector3(-clone_distance, -clone_distance, 0),
            transform.position + new Vector3(clone_distance, -clone_distance, 0)
        };

        for(int i = 0; i < clones.Length; i++)
        {
            clones[i].SetActive(true);
            clones[i].transform.SetParent(null);
        }

        //var dir = player.position - transform.position;
        //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //locked_rotation = transform.localEulerAngles;

        renderer.enabled = false;
        self_collider.enabled = false;

        StartCoroutine("GunAttackNumerator");
    }

    void SplitAttack()
    {
        movement_script.maxSpeed = 0.5f;

        minions = new List<GameObject>();

        for(int i = 0; i < 20; i++)
        {
            minions.Add(Instantiate(minion_prefab, transform.position, transform.rotation));
            minions[i].transform.SetParent(null);

            minions[i].GetComponent<AIPath>().enabled = false;
            minions[i].GetComponent<EnemyHealth>().no_drops = true;
            minions[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(1400, -1400), Random.Range(1400, -1400)));
            minions[i].GetComponent<AIPath>().speed = 7;
        }

        renderer.enabled = false;
        self_collider.enabled = false;

        StartCoroutine("SplitAttackNumerator");
    }


    void DeltaAttack(){

        // Delta setup
        int delta_choice;
        
        if(health_ratio > 0.75){ delta_choice = Random.Range(0, 3); }
        else if (health_ratio > 0.25){ delta_choice = Random.Range(2, 4); }
        else { delta_choice = 4; }

        if(delta_choice == 0){TwoBurstDeltaSetup();}
        else if(delta_choice == 1){ FourBurstDeltaSetup();}
        else if(delta_choice == 2){ ThreeSpreadDeltaSetup();}
        else if(delta_choice == 3){ FiveSpreadDeltaSetup();}
        else if(delta_choice == 4){ PanicDeltaSetup();}

        // PanicDeltaSetup();

        // Alpha setup
        movement_script.maxSpeed = 0f;
        renderer.color = transparent_col;
        
        StartCoroutine("DeltaAttackNumerator");
        
    }

    // ----DELTA ATTACKS----

    void PanicDeltaSetup(){
        delta_burst_iterations = 20;
        delta_burst_count = 3;
        delta_iteration_time = 0.1f;

        culminative_delta_burst_time = delta_burst_iterations * delta_iteration_time;
        delta_time_between_bursts = culminative_delta_burst_time-1;

        culminative_delta_attack_time = culminative_delta_burst_time * delta_time_between_bursts;
        
        delta_distance = 10;
        delta_random_pos = 0;

    }
    
    void FourBurstDeltaSetup(){
        delta_burst_iterations = 4;
        delta_burst_count = 5;
        delta_iteration_time = 0.1f;

        culminative_delta_burst_time = delta_burst_iterations * delta_iteration_time;
        
        delta_time_between_bursts = culminative_delta_burst_time+1;
        culminative_delta_attack_time = culminative_delta_burst_time * delta_time_between_bursts;
        
        delta_distance = 10;
        delta_random_pos = 1.5f;

    }
    void TwoBurstDeltaSetup(){
        delta_burst_iterations = 2;
        delta_burst_count = 8;
        delta_iteration_time = 0.2f;

        culminative_delta_burst_time = delta_burst_iterations * delta_iteration_time;
        
        delta_time_between_bursts = culminative_delta_burst_time + delta_iteration_time;
        culminative_delta_attack_time = culminative_delta_burst_time * delta_time_between_bursts;
        
        delta_distance = 10;
        delta_random_pos = 1.5f;

    }
    void FiveSpreadDeltaSetup(){
        delta_burst_iterations = 5;
        delta_burst_count = 5;
        delta_iteration_time = 0;

        culminative_delta_burst_time = delta_burst_iterations * delta_iteration_time;
        
        delta_time_between_bursts = culminative_delta_burst_time + 1;
        culminative_delta_attack_time = culminative_delta_burst_time * delta_time_between_bursts;
        
        delta_distance = 10;
        delta_random_pos = 3f;
    }
    void ThreeSpreadDeltaSetup(){
        delta_burst_iterations = 3;
        delta_burst_count = 6;
        delta_iteration_time = 0;

        culminative_delta_burst_time = delta_burst_iterations * delta_iteration_time;
        
        delta_time_between_bursts = culminative_delta_burst_time + 0.5f;
        culminative_delta_attack_time = culminative_delta_burst_time * delta_time_between_bursts;
        
        delta_distance = 10;
        delta_random_pos = 1;
    }



    // ----ENUMERATORS----

    IEnumerator GunAttackNumerator()
    {
        float lerp_value = 0;

        rb_self.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        while (lerp_value < 1)
        {
            lerp_value += 8 * Time.deltaTime;
            if(lerp_value > 1)
            {
                lerp_value = 1;
            }

            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = Vector3.Lerp(transform.position, clone_positions[i], lerp_value);
            }
            yield return null;
        }



        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < clones.Length; i++)
        {
            clones[i].GetComponent<AIShooterScript>().enabled = true;
            clones[i].GetComponent<AIShooterScript>().CanShoot = true;
        }

        yield return new WaitForSeconds(4);

        for (int i = 0; i < clones.Length; i++)
        {
            clones[i].GetComponent<AIShooterScript>().enabled = false;
        }

        while (lerp_value > 0)
        {
            lerp_value -= 8 * Time.deltaTime;
            if (lerp_value < 0)
            {
                lerp_value = 0;
            }

            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = Vector3.Lerp(transform.position, clone_positions[i], lerp_value);
            }
            yield return null;
        }

        for (int i = 0; i < clones.Length; i++)
        {
            clones[i].transform.SetParent(gameObject.transform);
            clones[i].SetActive(false);
        }

        renderer.enabled = true;
        self_collider.enabled = true;

        yield return new WaitForSeconds(1);

        rb_self.constraints = RigidbodyConstraints2D.None;

        Chase();

    }

    IEnumerator SplitAttackNumerator()
    {
        float timer = 5;

        yield return new WaitForSeconds(0.5f);

        timer -= 0.5f;

        for (int i = 0; i < minions.Count; i++)
        {
            if(minions[i] != null)
            {
                minions[i].GetComponent<AIPath>().enabled = true;
            }
            else
            {
                minions.RemoveAt(i);
            }
        }

        while (timer > 0)
        {
            if(minions.Count < 10)
            {
                
                if(minions.Count == 0)
                {
                    renderer.enabled = true;
                    self_collider.enabled = true;
                    Chase();
                    break;
                }

                //transform.position = minions[0].transform.position;
                renderer.enabled = true;
                self_collider.enabled = true;

                //for (int i = 0; i < minions.Count; i++)
                //{
                //
                //    Destroy(minions[i]);
                //    minions.RemoveAt(i);
                //}

                Chase();
            }

            for(int i = 0; i < minions.Count; i++)
            {
                if(minions[i] == null)
                {
                    minions.RemoveAt(i);
                    timer -= 1;
                }
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        //if (minions.Count > 0)
        //{
        //    for (int i = 0; i < minions.Count; i++)
        //    {
        //        if (minions[i] != null)
        //        {
        //            minions[i].GetComponent<EnemyHealth>().Die();
        //            minions.RemoveAt(i);
        //        }
        //        else
        //        {
        //            minions.RemoveAt(i);
        //        }
        //    }
        //}
        
        renderer.enabled = true;
        self_collider.enabled = true;

        Chase();

    }

    IEnumerator DeltaAttackNumerator(){
        // Triggers delta bursts ever x amount of time and for x bursts
        for(int i = 0; i < delta_burst_count; i++)
        {
            StartCoroutine("DeltaBurst");
            yield return new WaitForSeconds(delta_time_between_bursts);
        }    

        renderer.color = main_color;
        Chase();
    }

    IEnumerator DeltaBurst(){

        // Sets up initial positions and list
        minions = new List<GameObject>();

        float delta_angle = Random.Range(0, 2*Mathf.PI);
        Vector2 random_pos_factor = new Vector2( 
            Random.Range(-delta_random_pos, delta_random_pos),
            Random.Range(-delta_random_pos, delta_random_pos));
        
        Vector2 position = new Vector2(Mathf.Cos(delta_angle), Mathf.Sin(delta_angle)) * delta_distance;
        //position += random_pos_factor;

        // Bursts delta spawns
        for(int i = 0; i < delta_burst_iterations; i++)
        {

            // Generates a random offset from standard position
            random_pos_factor = new Vector2( 
                Random.Range(-delta_random_pos, delta_random_pos),
                Random.Range(-delta_random_pos, delta_random_pos));
        
            // Setting new related random positions
            position = new Vector2(Mathf.Cos(delta_angle), Mathf.Sin(delta_angle)) * delta_distance;
            //position += random_pos_factor;

            // Creates next delta
            minions.Add(Instantiate(delta_prefab, player.transform.position + new Vector3(position.x, position.y, 0), transform.rotation));
            minions[i].transform.SetParent(null);

            // Removing dead deltas from list
            yield return new WaitForSeconds(delta_iteration_time);
        }
    }
}   
    