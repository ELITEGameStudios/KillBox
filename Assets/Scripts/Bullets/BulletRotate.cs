using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotate : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float accel, rotate, rotateTime, currentRotateTime, angle, minAngle;
    [SerializeField]
    private bool canSeePlayer, hasTrail, bulletFreeze;
    private Vector2 target;
    [SerializeField]
    private TrailRenderer trail;

    // Start is called before the first frame update
    void OnEnable()
    {
        bulletFreeze = true;
        currentRotateTime = rotateTime;
        canSeePlayer = false;
        if (hasTrail){
            trail.emitting = false;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentRotateTime > 0){
            currentRotateTime -= Time.deltaTime;
            transform.eulerAngles += new Vector3(0, 0, rotate  * Time.fixedDeltaTime);
        }
        else{
            if (!canSeePlayer){
                target = Player.main.tf.position;
                transform.eulerAngles += new Vector3(0, 0, rotate * Time.fixedDeltaTime);
                angle =  Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position).eulerAngles.z;
                if(Mathf.Abs(transform.eulerAngles.z - angle) <= minAngle){
                    canSeePlayer = true;
                }
            }
            else{
                if (hasTrail){
                    trail.emitting = true;
                }
                if (bulletFreeze){
                    rb.velocity = new Vector2(0, 0);
                    bulletFreeze = false;
                }
                rb.AddForce(transform.up * accel * Time.fixedDeltaTime);
            }
            
        }
        }
        
    }
