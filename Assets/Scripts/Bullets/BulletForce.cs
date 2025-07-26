using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed, accel;

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.up * accel * Time.fixedDeltaTime);
    }
}
