using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class EnemySquare : MonoBehaviour
{

    public AIDestinationSetter dest_setter;
    public AIPath pathing;

    [SerializeField]
    private bool is_attacking;

    [SerializeField]
    private float timeElapsed, rush_time_elapsed, rush_time, chase_time, rotation, strafe;

    [SerializeField]
    private int rot_offset, max_distance;

    [SerializeField]
    private Vector3 init_pos, target, strafe_dir, strafe_constant;

    // Start is called before the first frame update
    void Start()
    {
        dest_setter.target = GameObject.Find("Player").transform;
//
        //ConstraintSource constraint = new ConstraintSource();
        //constraint.sourceTransform = dest_setter.target;
        //constraint.weight = 1;
    }

    // Update is called once per frame
    void Update()
    {


        if ( !is_attacking ){
            //gameObject.GetComponent<AIPath>().enableRotation = true;

            timeElapsed += Time.deltaTime;
            if (timeElapsed > chase_time - 0.25f ){

                if (timeElapsed > chase_time){
                    timeElapsed = 0;

                    //gameObject.GetComponent<AIPath>().enableRotation = false;
                    InitRush();
                }
                else{
                    //pathing.enabled = false;
                }
            }
            else{
                Vector3 relative = transform.InverseTransformPoint(dest_setter.target.position);
                float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
                transform.Rotate(0,0, -angle);

                transform.Translate(strafe_dir*strafe*Time.deltaTime, Space.Self);
            }
        }
        else{
            Rush();
            //gameObject.GetComponent<AIPath>().maxSpeed = 10;

        }


    }
    void Rush(){
        float normalized_time = rush_time_elapsed/rush_time;
        
        if(rush_time_elapsed >= rush_time){
            is_attacking = false;
            //pathing.enabled = true;

            int a = Random.Range(0, 2);
            if(a == 0){
                strafe_dir = Vector3.right;
            }
            else
            {
                strafe_dir = Vector3.left;   
            }

            strafe_dir += strafe_constant;
        }
        else{
            transform.position = Vector3.Lerp(init_pos, target, Mathf.Sin(normalized_time * (Mathf.PI/2)));
        }

        rush_time_elapsed += Time.deltaTime;
    }

    void InitRush(){
        Vector3 look_at_pos = dest_setter.target.position;
        look_at_pos.z = transform.position.z;

        rotation = transform.localEulerAngles.z + rot_offset;

        float distance = Vector3.Distance(transform.position, dest_setter.target.position) * 1.5f;

        if(distance > max_distance){
            distance = max_distance;
        }

        target = transform.position + new Vector3(
            Mathf.Cos(rotation * Mathf.Deg2Rad) * distance,
            Mathf.Sin(rotation* Mathf.Deg2Rad) * distance,
            0
        );

        init_pos = transform.position;
        rush_time_elapsed = 0;
        //gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up*600);
        is_attacking = true;

    }

    void ChooseStrafeDirection(){

    }
}