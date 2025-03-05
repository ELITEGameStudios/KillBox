using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{
    public Transform bars_carrier, gun_holder;
    public Slider[] current_sliders;
    public bool duals, combined;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (combined)
        {
            current_sliders[0].value = 0;
            current_sliders[1].value = 0;
            current_sliders[2].value = 0;
            current_sliders[3].value = 0;
            current_sliders[4].value = 0;
            current_sliders[5].value = 0;
        }

        for (int i = 0; i < gun_holder.childCount; i++)
        {

            shooterScript2D gun = gun_holder.GetChild(i).gameObject.GetComponent<shooterScript2D>();



            if (gun.gameObject.activeInHierarchy && !gun.is_dual && !duals && !combined)
            {
                current_sliders[0].value = gun.bulletDamage;
                current_sliders[1].value = 1 / gun.FR;
                current_sliders[2].value = 50 - gun.spread;
                current_sliders[3].value = gun.Velocity;
                current_sliders[4].value = 100 - ((gun.cooldown_units * (1 / gun.FR)) * gun.bulletsPerShot); 
                current_sliders[5].value = gun.bulletsPerShot;
                break;
            }

            else if (gun.gameObject.activeInHierarchy && gun.is_dual && duals && !combined)
            {
                current_sliders[0].value = gun.bulletDamage;
                current_sliders[1].value = 1 / gun.FR;
                current_sliders[2].value = 50 - gun.spread;
                current_sliders[3].value = gun.Velocity;
                current_sliders[4].value = 100 - ((gun.cooldown_units * (1 / gun.FR)) * gun.bulletsPerShot);
                current_sliders[5].value = gun.bulletsPerShot;
                break;
            }

            else if (gun.gameObject.activeInHierarchy && combined)
            {
                current_sliders[0].value += gun.bulletDamage;
                current_sliders[1].value += 1 / gun.FR;
                current_sliders[2].value += 50 - gun.spread;
                current_sliders[3].value += gun.Velocity;
                current_sliders[4].value += 100 - ((gun.cooldown_units * (1 / gun.FR)) * gun.bulletsPerShot);
                current_sliders[5].value += gun.bulletsPerShot;
            }
        }
    }
}
