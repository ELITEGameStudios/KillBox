using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRetarget : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float accel, defaultTime, currentDefaultTime, rotateTime, currentRotateTime;
    private Vector2 target;
    [SerializeField]
    private bool finDefault, finRotate;
    // Start is called before the first frame update
    void OnEnable()
    {
        currentDefaultTime = defaultTime;
        currentRotateTime = rotateTime;
        finDefault = false;
        finRotate = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!finDefault){
            DefaultUpdate();
        }
        else if (!finRotate){
            RotateUpdate();
        }
        else{
            rb.AddForce(transform.up * accel * Time.fixedDeltaTime);
        }
        
    }
    void DefaultUpdate(){
        if (currentDefaultTime > 0){
            currentDefaultTime -= Time.deltaTime;
        }
        else{
            rb.velocity = new Vector2(0, 0);
            finDefault = true;
        }
    }
    void RotateUpdate(){
        if (currentRotateTime > 0){
            target = Player.main.tf.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position);
            currentRotateTime -= Time.deltaTime;
        }
        else{
            finRotate = true;
        }
    }
}
