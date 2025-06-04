using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneIntegration : MonoBehaviour
{
    GameObject player;
    GameObject[] guns;
    public AIShooterScript ai_gun1, ai_gun2;
    private bool player_has_misc_gun;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        guns = new GameObject[player.transform.GetChild(2).transform.childCount];

        for (int i = 0; i < player.transform.GetChild(2).transform.childCount; i++)
        {
            guns[i] = player.transform.GetChild(2).transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < guns.Length; i++)
        {
            if(guns[i].tag == "Gun" && guns[i].activeInHierarchy){
                shooterScript2D gun_script = guns[i].GetComponent<shooterScript2D>();

                //Checks gun type
                if(gun_script.misc_gun && !player_has_misc_gun){
                    player_has_misc_gun = true;
                }

                //assigns gun values
                if(!gun_script.is_dual){
                    if(gun_script.misc_gun){
                        ai_gun1.Velocity = 150f;
                        ai_gun1.spread = 30f;
                        ai_gun1.BulletsPerShot = 1;
                        ai_gun1.FR = 0.1f;
                        ai_gun1.damage_boss_field = 200;
                    }
                    else{
                        ai_gun1.Velocity = gun_script.Velocity;
                        ai_gun1.spread = gun_script.spread;
                        ai_gun1.BulletsPerShot = gun_script.bulletsPerShot;
                        ai_gun1.FR = gun_script.FR;
                        ai_gun1.damage_boss_field = gun_script.bulletDamage;
                    }
                }
                else{
                    ai_gun2.gameObject.SetActive(true);
                    if(gun_script.misc_gun){
                        ai_gun2.Velocity = 150f;
                        ai_gun2.spread = 30f;
                        ai_gun2.BulletsPerShot = 1;
                        ai_gun2.FR = 0.1f;
                        ai_gun2.damage_boss_field = 200;
                    }
                    else{
                        ai_gun2.Velocity = gun_script.Velocity;
                        ai_gun2.spread = gun_script.spread;
                        ai_gun2.BulletsPerShot = gun_script.bulletsPerShot;
                        ai_gun2.FR = gun_script.FR;
                        ai_gun2.damage_boss_field = gun_script.bulletDamage;
                    }
                }
            }
        }

        //if(!player_has_misc_gun){
        if(player.GetComponent<PlayerHealth>().GetMaxHealth() < 500){
            gameObject.GetComponent<EnemyHealth>().maxHealth = 500;
            gameObject.GetComponent<EnemyHealth>().CurrentHealth = 500;
        }
        else{
            gameObject.GetComponent<EnemyHealth>().maxHealth = (int)(player.GetComponent<PlayerHealth>().GetMaxHealth() / 2);// * 1.5;
            gameObject.GetComponent<EnemyHealth>().CurrentHealth = gameObject.GetComponent<EnemyHealth>().maxHealth;// * 1.5;
        }
        //}
        //else{
            //gameObject.GetComponent<EnemyHealth>().MaxHealth = 2000;
            //gameObject.GetComponent<EnemyHealth>().CurrentHealth = 2000;
        //}

        gameObject.GetComponent<Pathfinding.AIPath>().maxSpeed = (player.GetComponent<TwoDPlayerController>().speed / 3) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if(guns[i].tag == "Gun" && guns[i].activeInHierarchy){
                shooterScript2D gun_script = guns[i].GetComponent<shooterScript2D>();
                if(!gun_script.is_dual){
                    ai_gun1.Velocity = gun_script.Velocity;
                    ai_gun1.spread = gun_script.spread;
                    ai_gun1.BulletsPerShot = gun_script.bulletsPerShot;
                    ai_gun1.FR = gun_script.FR;
                    ai_gun1.damage_boss_field = gun_script.bulletDamage;
                }
                else{
                    ai_gun2.gameObject.SetActive(true);
                    ai_gun2.Velocity = gun_script.Velocity;
                    ai_gun2.spread = gun_script.spread;
                    ai_gun2.BulletsPerShot = gun_script.bulletsPerShot;
                    ai_gun2.FR = gun_script.FR;
                }
            }
        }

    }
}
