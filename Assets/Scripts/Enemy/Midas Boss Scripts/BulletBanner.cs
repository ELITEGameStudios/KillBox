using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBanner : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed, accel, getVelo, ogVelo;

    void OnEnable(){
        getVelo = ogVelo;
    }
    void Update(){
        if (getVelo > 0){
            getVelo -= Time.deltaTime;
            speed = rb.velocity.x;
        }
    }
    void FixedUpdate(){   
        if (speed > 0){rb.AddForce(transform.right * accel * Time.fixedDeltaTime);}
        else {rb.AddForce(transform.right * accel * Time.fixedDeltaTime * -1);}
    }
}