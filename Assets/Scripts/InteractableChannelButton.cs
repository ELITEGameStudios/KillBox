using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableChannelButton : MonoBehaviour
{
    [SerializeField] private float clickableRange;
    [SerializeField] private Text inputText;
    [SerializeField] private bool inRange;
    [SerializeField] private ChannelButton button;



    void Update(){

        if(inRange){InRangePeriodic();}

        if(Vector2.Distance(transform.position, Player.main.tf.position) <= clickableRange){
            SetText();
            inRange = true;
        }
        else{
            inputText.gameObject.SetActive(false);
            inRange = false;
        }
    }

    void InRangePeriodic(){
        if(CustomKeybinds.main.PressingInteract()){
            if(button.IsInteractable){
                button.Interact();
            }
        }
    }

    void SetText(){
        if(button.IsInteractable){
            inputText.gameObject.SetActive(true);
        }
        else{
            inputText.gameObject.SetActive(false);
            return;
        }
        if(DetectInputDevice.main.isKBM){ 
            inputText.text = CustomKeybinds.main.Interact.ToString();    
        }
        else if(DetectInputDevice.main.isController){
            inputText.text = "Y";    
        }
    }
}
