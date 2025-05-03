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

    private int integrity;
    public bool wall_penetration { get; protected set; }
    public bool isTrigger { get {return col.isTrigger;} }

    [SerializeField]
    private bool misc_bullet, lock_trigger;

    public float range { get; private set; }

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

    public void SetBullet(string name_input, int dmg, int penetration_input = 0, bool wall_penetration_int = false, float _range = 1)
    {
        name = name_input;
        damage = dmg;
        col = gameObject.GetComponent<Collider2D>();
        range = _range;

        if (penetration_input == 0)
        {
            if(!lock_trigger){
                col.isTrigger = false;
            }
        }
        else
        {
            col.isTrigger = true;
            integrity = penetration_input;
        }

        wall_penetration = wall_penetration_int;
    }

    void Awake()
    {
        col = gameObject.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (wall_penetration)
        {
            integrity--;
        }
        else if(hit.tag == "Enemy")
        {
            integrity--;
        }
        else if(hit.GetComponent<EnemyHealth>() != null)
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

    public void SetName(string nameInput){
        name = nameInput;
        gameObject.name = nameInput;
    }

    //public void SetPenetration(int input)//BuffsManager manager)
    //{
    //
    //    penetration = input;
    //    //int chance = Random.Range(0, 101);
    //    //if(chance <= manager.buff_strength[4] * 4)
    //    //{
    //    //    col.isTrigger = true;
    //    //}
    //    //else
    //    //{
    //    //    col.isTrigger = false;
    //    //}
    //}

    public void SetDmg(int dmg)
    {
        damage = dmg;
    }
}
