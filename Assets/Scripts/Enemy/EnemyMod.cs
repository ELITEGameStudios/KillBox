using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMod : MonoBehaviour
{   
    [SerializeField]
    private AIPath movement_script;

    [SerializeField]
    private TriggerColliderTracker colliderTracker;

    private float def_speed, speed_multiplier;
    private bool slowed;

    public float SpeedMultiplier {get => speed_multiplier; private set => speed_multiplier = value;}

    // Start is called before the first frame update
    void Awake()
    {
        movement_script = gameObject.GetComponent<AIPath>();
        colliderTracker = gameObject.GetComponent<TriggerColliderTracker>();
        if(movement_script != null){
            def_speed = movement_script.maxSpeed;
        }
        speed_multiplier = 1;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        List<Collider2D> list = colliderTracker.GetColliders();

        slowed = false;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetComponent<EnemySlowZone>() != null)
            {
                slowed = true;

                speed_multiplier = list[i].GetComponent<EnemySlowZone>().Multiplier;

                break;
            }
        }

        if (slowed)
        {
            if(movement_script != null){
                movement_script.maxSpeed = def_speed * speed_multiplier;
            }
        }
        else
        {
            if(movement_script != null){
                movement_script.maxSpeed = def_speed;
            }
            speed_multiplier = 1;
        }
    }
}
