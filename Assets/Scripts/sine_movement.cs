using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sine_movement : MonoBehaviour
{
    public float factor, speed, delay, delayTimer;
    private float timef, spinFactor;
    private Vector3 RootPos;
    public bool horizontal;
    private bool expired_delay = false;

    void Start(){
        RootPos = gameObject.transform.position;
        delayTimer = delay;
    }

    void OnEnable(){
        expired_delay = false;
        delayTimer = delay;
    }

    public void SetRootPos(Vector3 position){
        RootPos = position;
    }

    public void SetRootPos(){
        RootPos = transform.position;
    }

    void Update()
    {
        if (expired_delay)
        {
            if (!horizontal)
            {
                timef += speed * Time.deltaTime;
                gameObject.transform.position = RootPos + new Vector3(0, Mathf.Sin(timef) * factor, 0);
            }
            else
            {
                timef += speed * Time.deltaTime;
                gameObject.transform.position = RootPos + new Vector3(Mathf.Sin(timef) * factor, 0, 0);
            }
        }
        else
        {
            if(delayTimer <= 0)
            {
                expired_delay = true;
            }
            else if (!expired_delay)
            {
                delayTimer -= Time.deltaTime;
            }
        }
    }

    public Vector3 ROOT
    {
        get { return RootPos; }
        set { RootPos = value; }
    }
}
