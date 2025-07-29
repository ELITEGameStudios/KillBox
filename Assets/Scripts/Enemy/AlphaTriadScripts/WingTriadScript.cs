using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingTriadScript : MonoBehaviour
{
    [SerializeField] private AIShooterScript shootSource;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fireRate, currentFireInterval, cooldown, currentCooldown, defaultTime;
    [SerializeField] private int shots, currentShots;
    [SerializeField] private bool canShoot, isEnabled;
    [Header("Movement Info")]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 target;
    void Start(){
        currentCooldown = cooldown;
        currentFireInterval = fireRate;
        currentShots = shots;
        canShoot = false;
        isEnabled = false;
    }
    void Update(){
        if (!isEnabled){
            defaultTime -= Time.deltaTime;
            if (defaultTime <= 0){
                isEnabled = true;
            }
        }
        else if (canShoot){
            // Runs the firing update if can shoot
            FiringUpdate();
        }
        else{
            // Does the cooldown in between each burst.
            if (currentCooldown > 0){
            currentCooldown -= Time.deltaTime;
            }
            else{
                currentShots = shots;
                canShoot = true;
            }
        }
    }
    void FixedUpdate(){
        if (isEnabled){
        // Sets the target and looks towards the player at all times.
        target = Player.main.tf.position;

        // Moves at a constant rate towards the player during the shoot cooldown.
        if (!canShoot){
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position);
            rb.velocity = ((Vector2)transform.position - target).normalized*speed*-1;
        }
        }
    }
    void FiringUpdate(){
        if (currentShots > 0){
            if (currentFireInterval <= 0)
            {
                currentShots -= 1;
                shootSource.Shoot();
                currentFireInterval = fireRate;
            }
            else
            {
                currentFireInterval -= Time.deltaTime;
            }
        }
        else{
            currentCooldown = cooldown;
            canShoot = false;
        }
    }
}