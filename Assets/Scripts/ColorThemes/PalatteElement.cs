using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalatteElement : MonoBehaviour
{
    public int element; 
    
    [SerializeField]
    private Image img;

    [SerializeField]
    private Text txt;
    
    [SerializeField]
    private bool followsGameModeColor, isText;



    void Awake(){
        if (img == null && !isText){
            img = gameObject.GetComponent<Image>();
            if (img == null){
                Destroy(this);
            }
        }
        else if(isText){
            txt = gameObject.GetComponent<Text>();
            if (txt == null){
                Destroy(this);
            }
        }
        

    }
    void Start(){
        if(!followsGameModeColor){
            if(!isText){ img.color = MainPalatte.main.GetColorByID(element); }
            else{ txt.color = MainPalatte.main.GetColorByID(element); }
        }
    }

    public void ChangeColor(int value){
        element = value;
        if(!isText){ img.color = MainPalatte.main.GetColorByID(value); }
        else{ txt.color = MainPalatte.main.GetColorByID(value); }
    }

    void Update(){
        if(!followsGameModeColor){ return; }
        Color color = Color.white;
        // Color color = MainPalatte.main.GetColorByMode(); Uncomment this once main palatte details are fixed
        if(!isText){ 
            if(img.color != color){
                img.color = color;
            }
        }
        else{ 
            if(txt.color != color){
                txt.color = color;
            }
        }
    }

}
