using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    [Header("Basic References")]
    [SerializeField]
    private EnemyHealth healthScript;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer mSprite1, mSprite2;
    [Header("Activated Mode Settings")]
    [SerializeField]
    private bool activated;
    [SerializeField]
    private float speed, postSpeed, maxSpeed, rampUpForce, rampUpSpeed;
    [SerializeField]
    private int healthLoss;
    [SerializeField]
    private Color enragedColor;
    private Vector2 target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthScript.CurrentHealth < healthScript.maxHealth){
            Enrage();
        }
        target = Player.main.tf.position;
    }
    void FixedUpdate(){
        transform.rotation = Quaternion.LookRotation(Vector3.forward, target - (Vector2)transform.position);
        if (!activated){
            rb.velocity = (transform.position - Player.main.tf.position).normalized*speed*-1;
        }
        else{
            rb.AddForce(transform.up * postSpeed * Time.fixedDeltaTime);
            if (rb.velocity.magnitude > rampUpSpeed){
                rb.velocity = rb.velocity.normalized*rampUpSpeed;
            }
            if (rampUpSpeed < maxSpeed){
                rampUpSpeed += rampUpForce;
            }
            //healthScript.CurrentHealth -= (1);
        }
    }
    void Enrage(){
        activated = true;
        mSprite1.color = enragedColor;
        mSprite2.color = enragedColor;
    }
}
