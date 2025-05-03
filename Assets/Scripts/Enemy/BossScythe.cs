using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossScythe : MonoBehaviour
{

    [SerializeField] private AIDestinationSetter destination_setter;
    [SerializeField] private AIPath pathing;
    [SerializeField] private FixedRotator rotator;
    [SerializeField] private Transform default_parent;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector3 normal_position;
    // Start is called before the first frame update
    void Start()
    {
        //Throw();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw(){
        destination_setter.target = Player.main.tf;
        pathing.enabled = true;
        rotator.enabled = true;
        transform.SetParent(null);
        rb.simulated = true;
    }

    public void IdleState(){
        transform.SetParent(default_parent);
        rb.simulated = false;

        destination_setter.target = Player.main.tf;
        pathing.enabled = false;
        rotator.enabled = false;
        transform.localPosition = normal_position;
        transform.localEulerAngles = Vector3.zero;
    }
}
