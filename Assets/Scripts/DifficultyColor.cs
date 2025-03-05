using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyColor : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private MaskableGraphic graphic;

    [SerializeField] bool getBest;
    // [SerializeField] private Text text;


    // Update is called once per frame
    void Update()
    {
        if(!getBest){
            graphic.color = colors[DifficultyManager.main.index];        
        }
        else{
            for(int i = 0; i < GameManager.main.personalBests.Length; i++){
                if(GameManager.main.PBInt == GameManager.main.personalBests[i]){
                    graphic.color = colors[i];    
                }
            }
        }
    }
}
