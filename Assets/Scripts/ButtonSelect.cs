using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] GameObject selectableObject;
    [SerializeField] bool notButton, reselect;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(!notButton){
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
        else{
            EventSystem.current.SetSelectedGameObject(selectableObject);
        }
        // button.Select();
        // button.onClick.Invoke();
    }

    void Update(){
        if(reselect && EventSystem.current.currentSelectedGameObject == null){
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }
}
