using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomForce
{
    public Vector2 startingForce, force;
    public float time, startingTime;
    public bool finished { get { return force == Vector2.zero; } }



    public Vector2 normalized { get { return startingForce.normalized; } }
    public float magnitude { get { return force.magnitude; } }
    public float startMagnitude { get { return startingForce.magnitude; } }

    public CustomForce(Vector2 force, float time = -1)
    {
        startingForce = force;
        this.force = force;
        startingTime = time;
        this.time = startingTime;
        // if (stoppingForce == -1 && time != -1) { stoppingForce = startingForce.magnitude / time; }
        // else if (stoppingForce != -1 && time == -1) { time = force.magnitude / stoppingForce; }
        // else if (stoppingForce != force.magnitude / time) { stoppingForce = force.magnitude; time = 1; }

        startingTime = time;
    }

    public void Update() // should be called every fixed update
    {
        if (time <= 0) { force = Vector2.zero; return; }
        force = Vector2.Lerp(startingForce, Vector2.zero, 1 - (time / startingTime));
        time -= Time.fixedDeltaTime;
    }
}
