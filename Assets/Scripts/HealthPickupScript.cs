using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{

    public PlayerHealth manager;
    
    void Awake()
    {
        manager = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();

    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            if(manager.GetMaxHealth() + 150 < 2500);
            {
                manager.SetMaxHealth(manager.GetMaxHealth() + 150,"JWBVIHEWBCV*&T^&237236fg3gv38fvr3v3v6)*&", false);
            }
            Destroy(gameObject);
        }
    }
}
