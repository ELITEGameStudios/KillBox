using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionClass : MonoBehaviour
{
    GameObject player;
    private float shrink_rate;

    private int damage;
    private float diameter;
    private int player_damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameObject.transform.localScale = new Vector3(diameter, diameter, 1);
        StartCoroutine(Explosion());
    }

    public void SetExplosion(int _damage, float _shrink_rate, float _diameter, int _player_damage){
        damage = _damage;
        shrink_rate = _shrink_rate;
        diameter = _diameter;
        player_damage = _player_damage;
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

        if(collider.gameObject == player){
            player.GetComponent<PlayerHealth>().TakeDmg(player_damage, 0);
        }
    }

    IEnumerator Explosion(){
        while (gameObject.transform.localScale.x >= 0)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - shrink_rate * Time.deltaTime, gameObject.transform.localScale.y - shrink_rate * Time.deltaTime, gameObject.transform.localScale.z);
            yield return null;
        }
        Destroy(gameObject);


    }
}
