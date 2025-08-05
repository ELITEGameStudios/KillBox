using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRampUp : MonoBehaviour
{
    [SerializeField] private AIShooterScript[] shoot_sources;
    [SerializeField] private float baseSpeed, accel;

    // Update is called once per frame
    void OnEnable(){
        foreach (AIShooterScript shooter in shoot_sources){
            shooter.Velocity = baseSpeed;
        }
    }
    void Update()
    {
        foreach (AIShooterScript shooter in shoot_sources){
            shooter.Velocity += accel * Time.deltaTime;
        }
    }
}
