using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDamage : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerHealth playerHealthScript;

    [SerializeField]
    private Rigidbody2D playerRigidBody;
    public int damage, _ignite_intensity;
    public float hitKb;
    public float HitCooldown = 1, AtCooldown = 1, immunity_time;
    private float currentHitCooldown;

    [SerializeField]
    private float _ignite_time;
    public string tag;
    public bool CanAttack = true, EndOfContact = false, ignore_immunity, _ignites, triggerEvent;

    [SerializeField]
    private EnemyHealth eh;
    
    [SerializeField]
    private UnityEvent onDamage;

    [SerializeField]
    private bool destruct_on_harm;
    //private float HitTimer, AttackCDTimer;
    
    [SerializeField]
    private EnemyProfile profile; 

    void Awake()
    {
        eh = gameObject.GetComponent<EnemyHealth>();
        if (profile != null) {damage = profile.Damage;} 
        playerHealthScript = Player.main.health;
    }
    
    // Update is called once per frame
    void Update(){

        if(playerHealthScript == null){
            playerHealthScript = Player.main.health;
            playerRigidBody = Player.main.rb;
        }

        // phrb = ph.gameObject.GetComponent<Rigidbody2D>();

        if(currentHitCooldown > 0){
            currentHitCooldown -= Time.deltaTime;
            CanAttack = false;
        }
        else{
            CanAttack = true;
        }
        
    }
    void doDamage(bool immediate){
        
        if(immediate) { playerHealthScript.TakeImmediateDmg(damage, immunity_time); }
        else{ playerHealthScript.TakeDmg(damage, immunity_time); }
        if (hitKb != 0)
        {
            Player.main.movement.AddCustomForce(new((Player.main.tf.transform.position - transform.position).normalized * hitKb, 0.33f));
        }

        if (_ignites)
        {
            playerHealthScript.gameObject.GetComponent<PlayerFireDamage>().Ignite(_ignite_time, _ignite_intensity);
        }

        if(triggerEvent){
            onDamage.Invoke();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(tag) && CanAttack)
        {
            if(!playerHealthScript.immune && !ignore_immunity){
                doDamage(false);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }
            else if(ignore_immunity)
            {
                doDamage(true);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }

            //var explosionDir = phrb.position - new Vector2(transform.position.x, transform.position.y);
            //var explosionDistance = (explosionDir.magnitude / 1);
            //var explosionForce = 3000;
            // Normalize without computing magnitude again
            //explosionDir /= explosionDistance;
            //phrb.AddForce(explosionDir * explosionForce, ForceMode2D.Force);
            //Debug.Log("ABOFVEBG");

            if (destruct_on_harm)
            {
                eh.Die(false);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //HitTimer -= 1 * Time.deltaTime;
        if(collision.gameObject.CompareTag(tag) && CanAttack)
        {
            if (!playerHealthScript.immune && !ignore_immunity)
            {
                doDamage(false);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }
            else if (ignore_immunity)
            {
                doDamage(true);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        CanAttack = true;
        //currentHitCooldown = AtCooldown;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(tag) && CanAttack)
        {
            if (!playerHealthScript.immune && !ignore_immunity)
            {
                doDamage(false);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }
            else if (ignore_immunity)
            {
                doDamage(true);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }

            if (destruct_on_harm)
            {
                eh.Die();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        //HitTimer -= 1 * Time.deltaTime;
        if (collider.gameObject.CompareTag(tag) && CanAttack)
        {
            if (!playerHealthScript.immune && !ignore_immunity)
            {
                doDamage(false);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }
            else if (ignore_immunity)
            {
                doDamage(true);
                CanAttack = false;
                currentHitCooldown = HitCooldown;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        CanAttack = true;
        //currentHitCooldown = AtCooldown;
    }
}