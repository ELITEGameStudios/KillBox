using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForceLocalSide : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed, accel;

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {   
        rb.AddForce(transform.right * accel * Time.fixedDeltaTime);
        
    }
}
