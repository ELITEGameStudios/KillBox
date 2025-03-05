using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiExplosionClass : MonoBehaviour
{


    [SerializeField]
    private int damage;

    [SerializeField]
    private float timer, current_time;

    void OnEnable(){
        current_time = timer;
    }

    void Update(){

        if(current_time <= 0){
            gameObject.SetActive(false);
        }

        current_time -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            if(collider.gameObject.GetComponent<EnemyHealth>() !=null){
                collider.gameObject.GetComponent<EnemyHealth>().TakeDmg(damage);
            }
            else
            {
                if(collider.transform.parent.gameObject.GetComponent<EnemyHealth>() != null)
                    collider.transform.parent.gameObject.GetComponent<EnemyHealth>().TakeDmg(damage);
            }
        }
    }
}
