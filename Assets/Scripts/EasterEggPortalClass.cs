using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasterEggPortalClass : MonoBehaviour
{
    public SpriteRenderer renderer;
    public ParticleSystem particles;
    public TpScript tp_script;
    public float H, S, V;
    public float speed, portal_scale_up_speed;
    public bool has_reached_soulbox_requirement = false;
    public int soulbox_counter;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        tp_script = gameObject.GetComponent<TpScript>();

    }

    // Update is called once per frame
    void Update()
    {
        Color.RGBToHSV(renderer.color, out H, out S, out V);

        //RGB changer
        if (H >= 1)
        {
            H = 0;
        }

        H += speed;
        renderer.color = Color.HSVToRGB(H + speed, S, V);
        particles.startColor = Color.HSVToRGB(H + speed, 0.5f, V);

        if(soulbox_counter >= 10){
            has_reached_soulbox_requirement = true;
            tp_script.IsActive = true;

            if (gameObject.transform.localScale.x < 1){
                gameObject.transform.localScale += new Vector3(portal_scale_up_speed * Time.deltaTime, portal_scale_up_speed * Time.deltaTime, portal_scale_up_speed * Time.deltaTime);
            }
        }
        else
        {
            tp_script.IsActive = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Enemy" && !has_reached_soulbox_requirement){
            if (col.gameObject.GetComponent<EnemyHealth>() == null){
                col.gameObject.GetComponentInParent<EnemyHealth>().Die();
            } 
            else
                col.gameObject.GetComponent<EnemyHealth>().Die();
            soulbox_counter++;
        }
    }
}
