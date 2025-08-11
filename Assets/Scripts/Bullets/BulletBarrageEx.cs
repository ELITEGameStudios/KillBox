using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBarrageEx : MonoBehaviour
{
    private Vector2 target;
    void Update(){
        target = Player.main.tf.position;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position);
    }
}