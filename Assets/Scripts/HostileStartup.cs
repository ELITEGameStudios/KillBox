using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HostileStartup : MonoBehaviour
{
    private float time = 0.3f;
    private bool has_started = false;

    void Update()
    { 
        if(time <= 0)
        {
            if (!has_started)
            {
                gameObject.GetComponent<Pathfinding.AIPath>().enabled = true;
                has_started = true;
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
}
