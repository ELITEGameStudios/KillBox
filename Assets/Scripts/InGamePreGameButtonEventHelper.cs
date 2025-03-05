using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePreGameButtonEventHelper : MonoBehaviour
{
    [SerializeField] private InGameButtonHandler handler;
    [SerializeField] private Text buttonText;
    [SerializeField] private bool usesHeader;
    [SerializeField] private string kbmMessage, controllerMessage, header;
    // Start is called before the first frame update
    public void SetHandler(InGameButtonHandler newHandler)
    {   
        handler = newHandler;
    }

    void Update(){
        if(DetectInputDevice.main.isKBM){
            if(usesHeader){
                buttonText.text = "Click to " + kbmMessage;
            }
            else{
                buttonText.text = kbmMessage;
            }
        }
        else if(DetectInputDevice.main.isController){
            if(usesHeader){
                buttonText.text = "Press Y to " + controllerMessage;
            }
            else{
                buttonText.text = controllerMessage;
            }
        }
    }

    public void ClickEvent()
    {
        handler.InvokeObject();
    }
}
