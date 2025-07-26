using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBarrage : MonoBehaviour
{
    [SerializeField]
    private AIShooterScript shooter;
    void OnEnable(){
        shooter.CanShoot = true;
    }
}