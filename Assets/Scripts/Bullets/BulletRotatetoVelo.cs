using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotatetoVelo : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Transform tf;
    void OnEnable(){
        rb.velocity = new Vector2(0.01f, 0.01f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tf.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
    }
}
