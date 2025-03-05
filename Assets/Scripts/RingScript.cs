using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingScript : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] private float velocity_factor, angle_randomizer, start_velocity, set_velocity; 
    [SerializeField] private bool glitchy; 
    [SerializeField] private EnemyMod mod; 
    
    [SerializeField]
    private float time, current_vel, period;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.main.obj;
        rb = gameObject.GetComponent<Rigidbody2D>();

        Vector3 relative = transform.InverseTransformPoint(player.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        transform.Rotate(0,0, -angle + Random.Range(-angle_randomizer, angle_randomizer));
        rb.AddForce( transform.up * start_velocity);

        period = ( 2*Mathf.PI / Mathf.PI);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 velocity = rb.velocity;
        Vector2 set_vector = velocity;
        set_vector.Normalize();

        if(glitchy){
            if(current_vel == 0){
                current_vel = 0.5f;
            }

            if(time < period){
                time += Time.fixedDeltaTime;
            }
            else{
                time = 0;
            }
            current_vel = (set_velocity * mod.SpeedMultiplier) * Mathf.Sin( (2*Mathf.PI * time) / period) ;


            if(current_vel != 0){
                set_vector *= current_vel;
                velocity = set_vector;
                rb.velocity = velocity;
            }
            else{
                return;
            }
        }
        else{
            set_vector *= set_velocity;

            velocity = set_vector * mod.SpeedMultiplier;
            rb.velocity = velocity;
                        
            // if(velocity.magnitude > set_vector.magnitude){
            //     velocity = set_vector;
            //     rb.velocity = velocity;
            // }
            // else{
            //     rb.AddForce( velocity * velocity_factor * Time.fixedDeltaTime);
            // }
        }



    }
}
