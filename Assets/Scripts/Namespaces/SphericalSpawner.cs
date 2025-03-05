using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SphericalSpawner : MonoBehaviour
{

    [SerializeField] private float default_distance;
    [SerializeField] private Vector2 summoned_position;
    [SerializeField] private GameObject last_summoned_object;    
    [SerializeField] private int count;

    public void SummonAtAngle(float angle, GameObject obj, float distance = -1, int count = 1){
        
        if(distance == -1){ distance = default_distance; }

        float step = 360 / count;

        for (int i = 0; i < count; i++){

            float real_angle = (step*i) + angle;

            summoned_position.x = distance * Mathf.Sin(real_angle * Mathf.Deg2Rad);
            summoned_position.y = distance * Mathf.Cos(real_angle * Mathf.Deg2Rad);

            last_summoned_object = Instantiate(obj, summoned_position, transform.rotation);
            last_summoned_object.transform.SetParent(null);
        }
    }

    public void SummonAtAngle(SphericalSpawnInstruction instruction){

        float step = 360 / count;

        for (int i = 0; i < count; i++){

            float real_angle = (step*i) + instruction.angle;

            summoned_position.x = instruction.distance * Mathf.Sin(real_angle * Mathf.Deg2Rad);
            summoned_position.y = instruction.distance * Mathf.Cos(real_angle * Mathf.Deg2Rad);

            last_summoned_object = Instantiate(instruction.obj, summoned_position, transform.rotation);
            last_summoned_object.transform.SetParent(null);
        }
    }
}
