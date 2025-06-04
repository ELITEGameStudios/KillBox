using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectRetirector : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Button selectedButton, redirectButton, deselectRedirectButton; 

    public void OnSelect(BaseEventData baseEventData){
    //     selectedButton = baseEventData.selectedObject.GetComponent<Button>();
    //     if(!selectedButton.interactable){
    //         EventSystem.current.SetSelectedGameObject(redirectButton.gameObject);
    //         // redirectButton.Select();
    //     }
    }

    void Update(){
        // selectedButton = baseEventData.selectedObject.GetComponent<Button>();
        try{

            if(( !selectedButton.interactable || !selectedButton.gameObject.activeInHierarchy ) && EventSystem.current.currentSelectedGameObject == selectedButton.gameObject){
                redirectButton.Select();
            }
        }
        catch{
            
        }

    }

    public void OnDeselect(BaseEventData baseEventData){
        // if(!baseEventData.selectedObject.GetComponent<Button>().interactable && deselectRedirectButton != null){
        //     baseEventData.selectedObject.GetComponent<Button>().
        //     deselectRedirectButton.Select();
        // }
        // Debug.Log(!baseEventData.selectedObject.GetComponent<Button>().interactable);
    }
}
