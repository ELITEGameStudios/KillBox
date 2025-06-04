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
            graphic.color = colors[KillBox.currentGame.difficultyIndex];        
        }
        else{
            graphic.color = colors[MainMenuManager.instance.selectedDifficulty];    
        }
    }
}
