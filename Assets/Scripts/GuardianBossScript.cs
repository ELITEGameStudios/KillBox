using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;
using UnityEditor.Rendering;

public class GuardianBossScript : MonoBehaviour
{
    public bool[] states;
    public bool whipIsActive {get; private set;}
    public bool isStalled {get; private set;}
    public AIPath movement_script;

    [SerializeField]
    private AIShooterScript[] shoot_sources;

    public Transform player;

    [SerializeField]
    private float current_chase_time, chase_time, clone_distance, rotationSpeed, stallTime, beamDistance;

    [SerializeField]
    private float[] Adrag, Aspeed, Rspeed;

    public Vector3 locked_rotation;
    private Rigidbody2D rb_self;
    [SerializeField] private GuardianBeam[] beams;
    [SerializeField] private Collider2D self_collider;
    [SerializeField] private Collider2D[] whipColliders;
    [SerializeField] private SpriteMask whipMask;

    [SerializeField] private Vector2 normalScale, warpedScale;
    [SerializeField] private FixedRotator rotator;

    [SerializeField]
    private string name;



    // Start is called before the first frame update
    void Start()
    {
        rb_self = gameObject.GetComponent<Rigidbody2D>();
        states = new bool[4];
        Chase();
        player = GameObject.FindWithTag("Player").transform;

        //BossAudio.Instance.OnShardSpawn(gameObject);
        BossBarManager.Instance.AddToQueue(gameObject, name);

    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

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
            // rb_self.angularVelocity = Aspeed[1] * Time.deltaTime;
        }
        else if(states[2]){
            // rb_self.angularVelocity = Aspeed[2] * Time.deltaTime;
        }
        else{
            // rb_self.angularVelocity = Aspeed[0] * Time.deltaTime;
        }

        if(isStalled){
            if(stallTime > 0){
                stallTime-= Time.deltaTime;
            }
            else{
                isStalled = false;
                if(whipIsActive){
                    rotationSpeed = Rspeed[2];
                    rotator.SetRotationRate(rotationSpeed);
                    movement_script.maxSpeed = 2.5f;
                }
            }
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
        else if (states[3])
        {
            //GunAttack();
            // Beams();
            Invoke("Beams", 0);
            Invoke("Beams", 2);
            Invoke("Beams", 4);

            if(Random.Range(0, 2) == 1) {
                Invoke("AgroChase", 6);
            }
            else{
                Invoke("GunAttack", 6);
            }
            // InvokeRepeating("Beams", 0, 5);
        }
    }

    void Chase()
    {
        rb_self.angularDrag = Adrag[0];

        rotationSpeed = Rspeed[0];
        rotator.SetRotationRate(rotationSpeed, 1);

        StopCoroutine("GunAttackNumerator");
        StopCoroutine("SplitAttackNumerator");
        movement_script.maxSpeed = 12;
        current_chase_time = chase_time;
        movement_script.maxAcceleration = 12;

        for (int i = 0; i < states.Length; i++) { states[i] = false; }
        states[0] = true;
    }

    public void HitPlayerWithWhip(){
        if(whipIsActive){
            isStalled = true;
            movement_script.maxSpeed = 1.5f;
            stallTime = 0.1f;
            rotationSpeed = -45;
            rotator.SetRotationRate(rotationSpeed);
        }
    } 

    void GunAttack()
    {
        movement_script.maxSpeed = 3f;

        movement_script.maxAcceleration = 2;

        // rb_self.angularDrag = Adrag[1];
        rotationSpeed = Random.Range(0, 2) == 0 ? Rspeed[1] : 450;
        rotator.SetRotationRate(rotationSpeed, 1);

        StartCoroutine("GunAttackNumerator");
    }

    void AgroChase()
    {
        // rb_self.angularDrag = Adrag[2];

        movement_script.maxSpeed = 7f;

        movement_script.maxAcceleration = 14;
        rotationSpeed = Rspeed[2];
        rotator.SetRotationRate(rotationSpeed, 1.5f);

        StartCoroutine("SplitAttackNumerator");
    }
    void Beams()
    {
        float newPosAngle = Random.Range(-180, 180);

        Vector2 newPos = new Vector2(Mathf.Cos(newPosAngle * Mathf.Deg2Rad), Mathf.Sin(newPosAngle * Mathf.Deg2Rad)) * beamDistance;

        transform.position = (Vector2)Player.main.tf.position + newPos;  
        rb_self.velocity = Vector2.zero;

        movement_script.maxSpeed = 2f;
        movement_script.maxAcceleration = 2;
        // rotationSpeed = Rspeed[3];
        rotationSpeed = Random.Range(0, 2) == 0 ? 40 : -40;

        rotator.SetRotationRate(rotationSpeed, 1f);
        foreach (GuardianBeam beam in beams){
            beam.gameObject.SetActive(true);
            beam.BeginSequence(2, 1f);
        }
        // StartCoroutine("SplitAttackNumerator");
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
        float 
            targetRotation = Rspeed[2],
            initialRSpeed = rotationSpeed,
            difference = targetRotation - initialRSpeed;

        whipMask.gameObject.SetActive(true);
        rotationSpeed = targetRotation; 
        rotator.SetRotationRate(rotationSpeed, 1f);
        while (whipMask.alphaCutoff > 0){
            whipMask.alphaCutoff -= 1 * Time.deltaTime;
            // rotationSpeed = initialRSpeed + difference * (1-whipMask.alphaCutoff); 
            // foreach (Collider2D col in whipColliders){ col.transform.localScale = Vector2.Lerp(warpedScale, normalScale, whipMask.alphaCutoff); }
            yield return null;  }

        whipMask.alphaCutoff = 0;
        // rotator.SetRotationRate(rotationSpeed);
        whipIsActive = true;

        foreach (Collider2D col in whipColliders){ col.enabled = true; }
        
        yield return new WaitForSeconds(4);
        // float timer = 4;

        // while (timer > 0){
        //     if()
        // }

        targetRotation = Rspeed[0];
        // rotator.SetRotationRate(targetRotation);
        initialRSpeed = rotationSpeed;
        difference = targetRotation - initialRSpeed;



        while (whipMask.alphaCutoff < 1){
            whipMask.alphaCutoff += 1 * Time.deltaTime;
            rotationSpeed = initialRSpeed + difference * whipMask.alphaCutoff; 
            // foreach (Collider2D col in whipColliders){ col.transform.localScale = Vector2.Lerp(warpedScale, normalScale, whipMask.alphaCutoff); }
            if(whipMask.alphaCutoff > 0.5f && whipIsActive)
                whipIsActive = false;
                foreach (Collider2D col in whipColliders){ col.enabled = false; }
            yield return null; }

        whipMask.alphaCutoff = 1;
        whipMask.gameObject.SetActive(false);
        rotationSpeed = targetRotation; 
        // rotator.SetRotationRate(rotationSpeed);
        Chase();
    }
}   
    