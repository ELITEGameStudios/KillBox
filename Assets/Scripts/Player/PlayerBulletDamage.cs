using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerHealth ph;
    public int Dmg;


    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
            ph.TakeDmg(Dmg, 0);
    }
}
