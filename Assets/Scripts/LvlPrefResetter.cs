using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlPrefResetter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ResetAll()
    {
        PlayerPrefs.GetInt("E2Rate", 0);
        //PlayerPrefs.GetInt("Level", 0);
        //PlayerPrefs.GetInt("Score", 0);
        //PlayerPrefs.GetInt("Health", 150);
        //PlayerPrefs.GetInt("Dual", 0);
        //PlayerPrefs.GetInt("Gun", 0);


        PlayerPrefs.SetInt("AudioTime", 0);
        PlayerPrefs.SetInt("E2Rate", 0);
        //PlayerPrefs.SetInt("Level", 0);
        //PlayerPrefs.SetInt("Score", 0);
        //PlayerPrefs.SetInt("Health", 150);
        //PlayerPrefs.SetInt("Dual", 0);
        //PlayerPrefs.SetInt("Gun", 0);
        PlayerPrefs.Save();
    }
}
