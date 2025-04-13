using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected int max_health, health;

    protected bool alive = true;
    protected bool destroyed = true;
    public bool immune_to_attacks = false; 

    public bool is_alive{
        get {return alive;}
        set {}
    }

    void Awake(){
        ResetHealth();
    }
    void ResetHealth(){
        health = max_health;
        OnMaxHealth();
    }

    //void Update(){}

    public int GetHealth(){
        return health;
    }

    public int GetMaxHealth(){
        return max_health;
    }

    public void Damage(int Damage){
        health -= Damage;

        if (health <= 0){
            Die();
        }
        else{
            OnDamaged();
        }
    }

    public void Heal(int value){
        health += value;
        if (health >= max_health){ health = max_health; OnMaxHealth();}

        OnHealed();
    }

    public void Die(){
        alive = false;
        OnDeath();
        
        if(destroyed){
            Destroy(gameObject);
        }
        else{
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EntityOnCollisionEnter(collision: collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        EntityOnCollisionStay(collision: collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        EntityOnCollisionExit(collision: collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        EntityOnTriggerEnter(collider: collider);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        EntityOnTriggerStay(collider: collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        EntityOnTriggerExit(collider: collider);
    }

    protected abstract void EntityOnCollisionEnter(Collision2D collision);
    protected abstract void EntityOnCollisionStay(Collision2D collision);
    protected abstract void EntityOnCollisionExit(Collision2D collision);
    protected abstract void EntityOnTriggerEnter(Collider2D collider);
    protected abstract void EntityOnTriggerStay(Collider2D collider);
    protected abstract void EntityOnTriggerExit(Collider2D collider);
        

    public abstract void OnDeath();
    public abstract void OnDamaged();
    public abstract void OnHealed();
    public abstract void OnMaxHealth();
    public abstract void OnCollisionEnter2D();
    //public abstract void Update();
}
