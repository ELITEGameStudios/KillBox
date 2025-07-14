using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEditor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float speed, accel, rotate, rotateTime, currentRotateTime, angle;
    [SerializeField]
    private bool useMaxSpeed, useForce, forceRight, forceLeft, useRotate, canSeePlayer;
    private Vector2 target;
    [SerializeField]
    private TrailRenderer trail;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentRotateTime = rotateTime;
        canSeePlayer = false;
        trail.emitting = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (useForce){
            rb.AddForce(transform.up * accel * Time.fixedDeltaTime);
        }
        else if (forceRight){
            rb.AddForce(new Vector2(1, 0) * accel * Time.fixedDeltaTime);
        }
        else if (forceLeft){
            rb.AddForce(new Vector2(-1, 0) * accel * Time.fixedDeltaTime);
        }
        else if (useRotate){
            if (currentRotateTime > 0){
                currentRotateTime -= Time.deltaTime;
                transform.eulerAngles += new Vector3(0, 0, rotate  * Time.fixedDeltaTime);
            }
            else{
                if (!canSeePlayer){
                    target = Player.main.tf.position;
                    transform.eulerAngles += new Vector3(0, 0, rotate * Time.fixedDeltaTime);
                    angle =  Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position).eulerAngles.z;
                    if(Mathf.Abs(transform.eulerAngles.z - angle) <= 20){
                        canSeePlayer = true;
                    }
                }
                else{
                    trail.emitting = true;
                    rb.AddForce(transform.up * accel * Time.fixedDeltaTime);
                }
                
            }
        }
        
    }
}
