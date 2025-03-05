using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextViaInputDevice : MonoBehaviour
{
    [SerializeField] private Text text; 
    public string kbmText; 
    public string gamepadText; 

    // Update is called once per frame
    void Update()
    {
        if(DetectInputDevice.main.isKBM){
            text.text = kbmText;
        }
        else if(DetectInputDevice.main.isController){
            text.text = gamepadText;
        }
    }
}
