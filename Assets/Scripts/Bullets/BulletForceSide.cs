using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForceSide : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed, accel;
    [SerializeField]
    private bool forceRight, forceLeft;

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (forceRight){
            rb.AddForce(new Vector2(1, 0) * accel * Time.fixedDeltaTime);
        }
        else if (forceLeft){
            rb.AddForce(new Vector2(-1, 0) * accel * Time.fixedDeltaTime);
        }
        
    }
}
