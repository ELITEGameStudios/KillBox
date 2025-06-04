using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private Button[] buttonList; // Use only if list bool is true

    [SerializeField] GameObject selectableObject;
    [SerializeField] bool notButton, reselect;
    [SerializeField] bool useList;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (useList){
            foreach (Button button in buttonList){
                if (button.interactable){
                    EventSystem.current.SetSelectedGameObject(button.gameObject);
                    return;
                }
            }    
        }

        if (!notButton)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(selectableObject);
        }
        // button.Select();
        // button.onClick.Invoke();
    }

    void Update(){
        if(reselect && EventSystem.current.currentSelectedGameObject == null){
            if (useList)
            {
                foreach (Button button in buttonList)
                {
                    if (button.interactable)
                    {
                        EventSystem.current.SetSelectedGameObject(button.gameObject);
                        return;
                    }
                }
            }

            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }
}
