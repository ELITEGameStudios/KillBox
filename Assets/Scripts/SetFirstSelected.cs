using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// CONTROLLER MENU BASE
public class SetFirstSelected : MonoBehaviour
{
    public GameObject firstSelected;

    void OnEnable(){
        if(firstSelected)EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
