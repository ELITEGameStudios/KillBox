using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorHSVWheelText : MonoBehaviour
{
    float H;
    float S;
    float V;

    //i need to add alpha channel functionality
    float alpha;

    public bool image;

    // Update is called once per frame
    void Update()
    {
        if(!image){
            Color.RGBToHSV(gameObject.GetComponent<Text>().color, out H, out S, out V);
        }
        else{
            Color.RGBToHSV(gameObject.GetComponent<Image>().color, out H, out S, out V);
        }

        if(H == 1){
            H = 0;
        }
        else{
            H+= 0.1f * Time.deltaTime;
        }

        if(!image){
            gameObject.GetComponent<Text>().color = Color.HSVToRGB(H, S, V);
        }
        else{
            gameObject.GetComponent<Image>().color = Color.HSVToRGB(H, S, V);
        }
    }
}
