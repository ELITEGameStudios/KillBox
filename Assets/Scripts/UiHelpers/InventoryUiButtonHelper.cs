using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUiButtonHelper : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    // Start is called before the first frame update
    public InventoryUIElement host;
    public void OnSelect(BaseEventData baseEventData){
        // if(baseEventData.selectedObject.GetComponent<Button>() == mainButton){
            host.OnSelect();

        // }
    }

    public void OnDeselect(BaseEventData baseEventData){
        host.OnDeselect();
        
    }
}
