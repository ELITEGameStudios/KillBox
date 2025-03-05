using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public class EnemyDart : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private float timeElapsed, attackSpeed, playerX, playerY, teleRef, dashVelocity, init_start_distance, offset, start_distance_randomization;
    private float start_distance;
    [SerializeField] private TrailRenderer trail; 
    [SerializeField] private bool is_attack;
    [SerializeField] private int cycles, cycles_until_death;
    [SerializeField] private EnemyMod mod;
    [SerializeField] private Vector2 desiredVelocity, currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        playerX = Player.transform.position.x;
        playerY = Player.transform.position.y;
        teleRef = Random.Range(1, 5);
        Dash();
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_attack){
            timeElapsed += Time.deltaTime;

            //Time to attack
            if (timeElapsed > attackSpeed){
                timeElapsed = 0;
                teleRef = Random.Range(1, 5);
                is_attack = true;

                cycles++;
                if(cycles >= cycles_until_death){
                    gameObject.GetComponent<EnemyHealth>().Die(to_player: false);
                }
            }

            // Get rid of my trail before I vanish
            else if(timeElapsed > attackSpeed - 0.25f){
                trail.emitting = false;
            }
            //...or bring it back if its not time yet
            else{
                if(!trail.emitting){
                    trail.emitting=true;
                }
            }

            // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // gameObject.GetComponent<Rigidbody2D>().AddForce(desiredVelocity);
            
        }

        else{
            playerX = Player.transform.position.x;
            playerY = Player.transform.position.y;
            Dash();
            is_attack = false;
        }
    }
    void Dash(){
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector2 offset_pos = new Vector2(Random.Range(-offset , offset), Random.Range(-offset , offset));
        start_distance = init_start_distance + Random.Range(-start_distance_randomization, start_distance_randomization);

        switch (teleRef){
            case(1):
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.position = new Vector3(playerX+start_distance + offset_pos.x,playerY + offset_pos.y, 0);
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*(dashVelocity*-1) * mod.SpeedMultiplier);
                desiredVelocity = transform.right*(dashVelocity*-1) * mod.SpeedMultiplier;
                break;
            case(2):
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.position = new Vector3(playerX-start_distance + offset_pos.x,playerY + offset_pos.y, 0);
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*dashVelocity * mod.SpeedMultiplier);
                desiredVelocity = transform.right*(dashVelocity*-1) * mod.SpeedMultiplier;
                break;
            case(3):
                transform.eulerAngles = new Vector3(0, 0, -90);
                transform.position = new Vector3(playerX + offset_pos.x, playerY+start_distance + offset_pos.y, 0);
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*dashVelocity * mod.SpeedMultiplier);
                desiredVelocity = transform.right*(dashVelocity*-1) * mod.SpeedMultiplier;
                break;
            case (4):
                transform.eulerAngles = new Vector3(0, 0, -90);
                transform.position = new Vector3(playerX + offset_pos.x, playerY-start_distance + offset_pos.y, 0);
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*(dashVelocity*-1) * mod.SpeedMultiplier);
                desiredVelocity = transform.right*(dashVelocity*-1) * mod.SpeedMultiplier;                
                break;
        }
    }
}