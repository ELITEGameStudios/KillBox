using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRerender : MonoBehaviour
{
    [SerializeField]
    private float waitTime, currentWaitTime;

    [SerializeField]
    private TrailRenderer trail;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentWaitTime = waitTime;
        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaitTime <= 0){
            trail.emitting = true;
        }
        else{
            currentWaitTime -= Time.deltaTime;
        }
    }
        
    }
