using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawnMeteor : MonoBehaviour
{
    [SerializeField]
    private AIShooterScript[] shootSources;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Rigidbody2D rb;
    private Vector2 target;
    void OnEnable(){

    }
    void Update(){
        target = Player.main.tf.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position);
        rb.velocity = ((Vector2)transform.position - target).normalized*speed*-1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MeteorTrigger")){
            foreach (AIShooterScript source in shootSources)
        {
            source.Shoot();
        }
        bullet.SetActive(false);
        }
    }
}