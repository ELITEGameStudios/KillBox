using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [SerializeField]
    private int body_damage;

    [SerializeField]
    private bool can_attack = true, destruct_on_harm;
    public float _hit_cooldown = 1, _attack_cooldown = 1, _immunity_time;
    private float _current_hit_cooldown;
    void UpdateCooldowns(){
        if(_current_hit_cooldown > 0){
            _current_hit_cooldown -= Time.deltaTime;
            can_attack = false;
        }
        else{
            can_attack = true;
        }
    }

    void Update(){
        UpdateCooldowns();
        EnemyUpdate();
    }

    protected abstract void EnemyUpdate();

    void TryAttack(Entity entity){

        if(can_attack)
        {
            if(!entity.immune_to_attacks){
                entity.Damage(body_damage);
                can_attack = false;
                _current_hit_cooldown = _hit_cooldown;
            }
            //else if(ignore_immunity)
            //{
            //    ph.TakeImmediateDmg(Damage, immunity_time);
            //    can_attack = false;
            //    current_hit_cooldown = hit_cooldown;
            //}

            if (destruct_on_harm)
            {
                Die();
            }
        }
    }

    protected override void EntityOnCollisionEnter(Collision2D collision){ AttemptBodyAttack(collision); }
    protected override void EntityOnCollisionStay(Collision2D collision){ AttemptBodyAttack(collision); }
    protected override void EntityOnCollisionExit(Collision2D collision){ AttemptBodyAttack(collision); }
    protected override void EntityOnTriggerEnter(Collider2D collider){ AttemptBodyAttack(collider: collider); }
    protected override void EntityOnTriggerStay(Collider2D collider){ AttemptBodyAttack(collider: collider); }
    protected override void EntityOnTriggerExit(Collider2D collider){ can_attack = true; }

    void AttemptBodyAttack(Collision2D collision = null, Collider2D collider = null){
        Entity entity;
        try
        {
            if(collision != null) { entity = collision.gameObject.GetComponent<Entity>(); } 
            else { entity = collider.gameObject.GetComponent<Entity>(); }

            if(entity != null){
                OnEntityDetected(entity);
                TryAttack(entity);
            }
        }
        catch (System.NullReferenceException)
        {
            return;
        }
    }

    protected abstract void OnEntityDetected(Entity entity);
}
