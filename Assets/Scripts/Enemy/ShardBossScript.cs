using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class ShardBossScript : MonoBehaviour
{
    public bool[] states;
    public AIPath movement_script;

    [SerializeField]
    private AIShooterScript[] shoot_sources;

    public Transform player;

    [SerializeField]
    private float current_chase_time, chase_time, clone_distance;

    [SerializeField]
    private float[] Adrag, Aspeed, Rspeed;

    public Vector3 locked_rotation;
    private Rigidbody2D rb_self;

    [SerializeField]
    private Collider2D self_collider;

    [SerializeField]
    private string name;

    public Sprite displaySprite;



    // Start is called before the first frame update
    void Start()
    {
        rb_self = gameObject.GetComponent<Rigidbody2D>();
        states = new bool[3];
        Chase();
        player = GameObject.FindWithTag("Player").transform;

        //BossAudio.Instance.OnShardSpawn(gameObject);
        BossBarManager.Instance.AddToQueue(gameObject, name, Color.yellow, displaySprite);

    }

    // Update is called once per frame
    void Update()
    {
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
            rb_self.angularVelocity = Aspeed[1] * Time.deltaTime;
        }
        else if(states[2]){
            rb_self.angularVelocity = Aspeed[2] * Time.deltaTime;
        }
        else{
            rb_self.angularVelocity = Aspeed[0] * Time.deltaTime;
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

        states[Random.Range(1, states.Length)] = true;

        if (states[1])
        {
            GunAttack();
        }
        else if (states[2])
        {
            //GunAttack();
            AgroChase();
        }
    }

    void Chase()
    {
        rb_self.angularDrag = Adrag[0];

        movement_script.rotationSpeed = Rspeed[0];

        StopCoroutine("GunAttackNumerator");
        StopCoroutine("SplitAttackNumerator");
        movement_script.maxSpeed = 12;
        current_chase_time = chase_time;
        movement_script.maxAcceleration = 8;

        for (int i = 0; i < states.Length; i++)
        {
            states[i] = false;
        }

        states[0] = true;
    }

    void GunAttack()
    {
        movement_script.maxSpeed = 2;

        movement_script.maxAcceleration = 2;

        rb_self.angularDrag = Adrag[1];
        movement_script.rotationSpeed = Rspeed[1];

        
        

        //var dir = player.position - transform.position;
        //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //locked_rotation = transform.localEulerAngles;

        StartCoroutine("GunAttackNumerator");
    }

    void AgroChase()
    {
        rb_self.angularDrag = Adrag[2];

        movement_script.maxSpeed = 3.5f;
        movement_script.maxSpeed = 3.5f;

        movement_script.maxAcceleration = 11;

        movement_script.rotationSpeed = Rspeed[2];

        StartCoroutine("SplitAttackNumerator");
    }

    IEnumerator GunAttackNumerator()
    {

        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < shoot_sources.Length; i++)
        {
            shoot_sources[i].enabled = true;
            shoot_sources[i].CanShoot = true;
        }

        yield return new WaitForSeconds(4);

        for (int i = 0; i < shoot_sources.Length; i++)
        {
            shoot_sources[i].enabled = false;
            shoot_sources[i].CanShoot = false;
        }

        yield return new WaitForSeconds(1);

        Chase();

    }

    IEnumerator SplitAttackNumerator()
    {

        yield return new WaitForSeconds(4);

        Chase();

    }
}   
    