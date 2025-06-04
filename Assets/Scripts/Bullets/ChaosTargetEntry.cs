using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Pathfinding;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ChaosTargetEntry{

    public GameObject obj {get; private set;}
    public Transform host {get; private set;}
    public float time {get; private set;}
    public float maxTime {get; private set;}
    // public float startingDistance {get; private set;}
    public Vector2 startingPos {get; private set;}
    public Vector3 startingScale {get; private set;}

    public ChaosTargetEntry(GameObject _obj, Transform _host){
        obj = _obj;
        host = _host;
        startingPos = obj.transform.position;
        startingScale = obj.transform.localScale;
        obj.GetComponent<Collider2D>().enabled = false;
        obj.GetComponent<AIPath>().enabled = false;
        time = 0;
        maxTime = 0.5f;

        if(obj.GetComponent<RingScript>() != null) {obj.GetComponent<RingScript>().enabled = false;}
        if(obj.GetComponent<EnemyDart>() != null) {obj.GetComponent<EnemyDart>().enabled = false;}
    }

    public void Feed(float added_time){
        time += added_time;
        float normmlizedTime = time/maxTime;
        obj.transform.position = Vector2.Lerp(startingPos, host.position, Mathf.Pow(2, 3* (normmlizedTime - 1)) - 0.125f );

        obj.transform.localScale = new Vector3(
            startingScale.x - 0.8f * startingScale.x * normmlizedTime,
            startingScale.y - 0.8f * startingScale.y * normmlizedTime,
            startingScale.z - 0.8f * startingScale.z * normmlizedTime
        );

        TimeCheck();
    }

    public void TimeCheck(){
        if(time >= maxTime){
            obj.GetComponent<EnemyHealth>().Die(true);
            Debug.Log("DED");
        }
    }
}