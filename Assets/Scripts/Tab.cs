using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tab : MonoBehaviour
{
    [SerializeField]
    private string key;

    [SerializeField]
    private TabSystemMaster master;
    
    [SerializeField]
    private bool is_enabled = false, not_disablable;

    [SerializeField]
    private UnityEvent on_enable, on_disable;

    // Start is called before the first frame update

    public void SetMaster(TabSystemMaster input){ master = input; }
    public string GetKey() { return key; }
    public bool Enabled() { return is_enabled; }
    public void OnClick(){ master.SwitchTab(this, !not_disablable); }


    public void Enable(){
        on_enable.Invoke();
        is_enabled = true;
    }

    public void Disable(){
        on_disable.Invoke();
        is_enabled = false;
    }


}
