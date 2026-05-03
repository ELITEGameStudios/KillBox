using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    public string name { get; protected set; }
    public int damage { get; protected set; }
    public int penetration { get; protected set; }
    [SerializeField]
    private Collider2D col;

    [SerializeField] private bool excludePenetration;
    private int integrity;
    public bool wall_penetration { get; protected set; }
    public bool isTrigger { get {return col.isTrigger;} }

    [SerializeField]
    private bool misc_bullet, lock_trigger, inWall;

    public float knockbackForce, knockbackTime, startingVel;
    public float range { get; private set; }
    public int slowdownVel { get; protected set; } // only set for penetration bullets
    public BulletDestroy bulletDestroy;


    //public BulletClass(string name_input, int dmg, int penetration_input = -1, bool wall_penetration_int = false)
    //{
    //    name = name_input;
    //    damage = dmg;
    //    col = gameObject.GetComponent<Collider2D>();
    //
    //    if (penetration_input == -1)
    //    {
    //        col.isTrigger = false;
    //    }
    //    else
    //    {
    //        col.isTrigger = true;
    //        integrity = penetration;
    //    }
    //
    //    wall_penetration = wall_penetration_int;
    //}

    public void SetBullet(string name_input, int dmg, int penetration_input = 0, bool wall_penetration_int = false, float _range = 1, float knockbackForce = 1, float knockbackTime = 0.33f, float startingVel = 0)
    {
        name = name_input;
        damage = dmg;
        col = gameObject.GetComponent<Collider2D>();
        range = _range;

        if (penetration_input == 0 || excludePenetration)
        {
            if (!lock_trigger)
            {
                col.isTrigger = false;
            }
        }
        else
        {
            col.isTrigger = true;
            integrity = penetration_input;
        }

        inWall = false;
        wall_penetration = wall_penetration_int;
        this.knockbackForce = knockbackForce;
        this.knockbackTime = knockbackTime;
        this.startingVel = startingVel;

        if(bulletDestroy!= null) bulletDestroy.NewTimer(range);
    }

    void Awake()
    {
        col = gameObject.GetComponent<Collider2D>();
    }
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); 
        if (inWall){
            rb.AddForce(-rb.velocity * Time.fixedDeltaTime * 75 / integrity);// / (100 * integrity));
        }
    }

    public void SetName(string nameInput)
    {
        name = nameInput;
        gameObject.name = nameInput;
    }

    public void SetDmg(int dmg)
    {
        damage = dmg;
    }

    void PenetrationEnter(Collider2D hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            // integrity--;
            inWall = true;
        }
        else if (hit.tag == "Enemy")
        {
            integrity--;
        }
        else if (hit.GetComponent<EnemyHealth>() != null)
        {
            integrity--;
        }

        if(integrity == 0)
        {
            if(!lock_trigger){
                col.isTrigger = false;
            }
        }
    }
    void PenetrationExit(Collider2D hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            inWall = false;
        }
        
    }

    protected virtual void TriggerEnter(Collider2D hit){ }
    protected virtual void TriggerExit(Collider2D hit){ }
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (!excludePenetration) { PenetrationEnter(hit); }
        TriggerEnter(hit);
    }
    void OnTriggerExit2D(Collider2D hit)
    {
        if (!excludePenetration) { PenetrationExit(hit); }
        TriggerExit(hit);
    }

}
