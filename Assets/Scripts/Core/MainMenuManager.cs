using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public int selectedDifficulty;
    public static MainMenuManager instance {get; private set;}


    void Awake(){
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
    }
}
