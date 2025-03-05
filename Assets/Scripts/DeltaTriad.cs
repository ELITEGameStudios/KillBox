using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaTriad : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private float timeElapsed, max_time, velocity_factor, angle_randomizer; 
    private float start_velocity;
    [SerializeField] private TrailRenderer trail; 
    [SerializeField] private Vector3 scale_mod; 
    [SerializeField] private bool out_of_time;
    [SerializeField] private EnemyMod mod;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        Vector3 relative = transform.InverseTransformPoint(Player.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        transform.Rotate(0,0, -angle + Random.Range(-angle_randomizer, angle_randomizer));
        
        gameObject.GetComponent<Rigidbody2D>().AddForce( transform.up * (start_velocity));
    }

    // Update is called once per frame
    void Update()
    {
        if (!out_of_time){
            timeElapsed += Time.deltaTime;
            gameObject.GetComponent<Rigidbody2D>().AddForce( transform.up * (velocity_factor) * mod.SpeedMultiplier * Time.deltaTime);
            transform.localScale += scale_mod * Time.deltaTime;


            //Time to attack
            if (timeElapsed > max_time){
                timeElapsed = 0;
                out_of_time = true;
            }
            // Get rid of my trail before I vanish
            else if(timeElapsed > max_time - 0.25f){
                trail.emitting = false;
            }
        }

        else{
            // Kills the entity once time is out
            gameObject.GetComponent<EnemyHealth>().Die(to_player: false);
        }
    }
}
