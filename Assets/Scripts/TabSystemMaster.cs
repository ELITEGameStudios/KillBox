using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabSystemMaster : MonoBehaviour, IBackButtonListener
{
    [SerializeField]
    private List<Tab> tabs;
    private List<string> keys = new List<string>();

    [SerializeField]
    private UnityEvent on_any_enabled, on_all_disabled;

    private bool any_enabled;
    [SerializeField] private bool closeOnBackKeybind;
    private bool passThisFrame;
    private int counter;
    
    void Awake(){
        for (int i = 0; i < tabs.Count; i++){
            keys.Add(tabs[i].GetKey());
        }
    }

    public void SwitchTab(string key){
        foreach (Tab i in tabs)
        {
            if (i.GetKey() == key){
                i.Enable();
                if(!any_enabled){
                    any_enabled = true;
                    on_any_enabled.Invoke();
                }
            }
            
            else{
                i.Disable();
            }
        }
    }

    public void SwitchTab(Tab tab, bool disablable = true){

        foreach (Tab i in tabs)
        {
            if (i == tab){
                if(i.Enabled() && disablable){
                    i.Disable();
                }
                else{
                    i.Enable();
                }

                if(!any_enabled){
                    any_enabled = true;

                    on_any_enabled.Invoke();
                }
            }

            else{
                i.Disable();
            }
        }
        
        CheckEnabled();
    }
    public void CloseTabs(){
        foreach (Tab i in tabs) { i.Disable(); }
        CheckEnabled();
    }

    public void CheckEnabled(){
        
        foreach (Tab i in tabs){
            if(i.Enabled())
            {
                any_enabled = true;
                return;
            }
        }

        if(any_enabled == true){
            any_enabled = false;
            on_all_disabled.Invoke();
        }
    }

    void Update(){
        if(passThisFrame){
            if(counter > 20){
                passThisFrame = !passThisFrame;
                counter = 0;
            }
            else{
                counter++;
            }
        }
    }

    public void SetBackButtonNav(bool backNav){
        closeOnBackKeybind = backNav;
        if(backNav){passThisFrame = true;}
    }

    public void OnBackButton(bool pressedThisFrame)
    {
        if(pressedThisFrame && closeOnBackKeybind && !passThisFrame){
            CloseTabs();
        }
    }
}
