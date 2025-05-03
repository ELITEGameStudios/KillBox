using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySlowZone : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
    public readonly TriggerColliderTracker colliderTracker;

    [SerializeField]
    private float slow_multiplier;

    void Awake(){}

    public float Multiplier
    {
        get {return slow_multiplier;}
    }


    void LateUpdate()
    {


    }
}
