using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponsMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    
    [SerializeField] private Button selectedButton; 

    public void OnSelect(BaseEventData baseEventData){
        selectedButton = baseEventData.selectedObject.GetComponent<Button>();
        selectedButton.onClick.Invoke();
    }
    public void OnDeselect(BaseEventData baseEventData){
        
    }

}
