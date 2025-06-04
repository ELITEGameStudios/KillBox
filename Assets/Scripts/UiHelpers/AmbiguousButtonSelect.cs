using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AmbiguousButtonSelect : MonoBehaviour
{
    [SerializeField] private Selectable[] selectables;

    public void SelectAvailable(){
        foreach (Selectable selectable in selectables){
            if(selectable.gameObject.activeInHierarchy){
                EventSystem.current.SetSelectedGameObject(selectable.gameObject);
                return;
            }
        }
    }
}
