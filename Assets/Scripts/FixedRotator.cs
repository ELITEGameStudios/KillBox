using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotator : MonoBehaviour
{

    public float speed;

    [SerializeField] private float initialSpeed, targetSpeed, timer, targetTime;
    [SerializeField] private bool inTransition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inTransition){
            if(timer > 0){
                timer -= Time.deltaTime;
                speed = Mathf.Lerp(targetSpeed, initialSpeed, timer/targetTime);
            }
            else{
                speed = targetSpeed;
                inTransition = false;
            }
        }

        
        gameObject.transform.localEulerAngles += new Vector3(0, 0, speed * Time.deltaTime);

        if(gameObject.transform.localEulerAngles.z == 360)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);

    }

    public void SetRotationRate(float newSpeed, float time = 0){
        if(time != 0){
            timer = time;
            targetTime = time;

            initialSpeed = speed;
            targetSpeed = newSpeed;

            inTransition = true;
        } 
        else{
            speed = newSpeed;
            inTransition = false;
        }
    }
}
