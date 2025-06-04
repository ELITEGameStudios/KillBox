using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPalatte : MonoBehaviour
{
    public Color primary, secondary, charlie, delta, echo;
    public Color standard, easy, extreme;

    public static MainPalatte main;

    void Awake(){
        if (main != null && main != this){
            Destroy(this);
        }
        else{
            main = this;
        }
    }

    public Color GetColorByID(int id){
        switch (id){
            case 0:
                return primary;
            case 1:
                return secondary;
            case 2:
                return charlie;
            case 3:
                return delta;
            case 4:
                return echo;
            case 5:
                return easy;
            case 6:
                return standard;
            case 7:
                return extreme;
        }
        return primary;
    }

    public Color GetColorByMode(){
        switch (MainMenuManager.instance.selectedDifficulty){
            case 0:
                return easy;
            case 1:
                return standard;
            case 2:
                return extreme;
        }
        return Color.white;
    }
}
