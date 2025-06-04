using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamoScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private bool pulsing;
    [SerializeField] private float distance, invisibleRange, opaqueRange, pulseTime, pulseTimer, opacity;

    // Update is called once per frame
    void Update()
    {
        if(!pulsing){

            distance = Vector2.Distance(Player.main.tf.position, transform.position);
            opacity = (distance - invisibleRange) / (opaqueRange - invisibleRange);
            renderer.color = Color.Lerp(Color.clear, Color.black, opacity);
        }
        else if(pulseTimer > 0){

            opacity = pulseTimer / pulseTime; 
            renderer.color = Color.Lerp(Color.clear, Color.black, opacity);
            pulseTimer -= Time.deltaTime;
            
        }
        else{
            pulsing = false;
        }

    }

    public void Pulse(float pulse){
        pulseTime = pulse;
        pulseTimer = pulseTime;
        pulsing = true;
    }
}
