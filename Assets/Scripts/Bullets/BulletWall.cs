using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed, scaleFactor, distance, timeElapsed;
    [SerializeField] private int index;

    // Update is called once per frame
    void OnEnable(){
        timeElapsed = 0;
        transform.localScale = new Vector2(0, 0.6f);
        index = Random.Range(0, 4) * 90;
        transform.eulerAngles = new Vector3(0, 0, index);
        transform.position = new Vector2(Player.main.tf.position.x + distance*Mathf.Cos(Mathf.Deg2Rad * (index - 90)), 
        Player.main.tf.position.y + distance*Mathf.Sin(Mathf.Deg2Rad * (index - 90)));

        rb.AddForce(transform.up * speed);
    }
    void Update()
    {
        if (transform.localScale.x < 4){
            timeElapsed += Time.deltaTime;
            transform.localScale = new Vector2(timeElapsed * scaleFactor, 0.6f);
        }
    }
}
