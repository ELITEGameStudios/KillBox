using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeybindData
{
    public int Shoot {get; private set;}
    public int Shoot2 {get; private set;}
    public int Ultramode {get; private set;}
    public int Pause {get; private set;}
    public int Interact {get; private set;}
    public int SwitchWeapon {get; private set;}
    
    public KeybindData (CustomKeybinds data){
        Shoot = (int)data.Shoot;
        Shoot2 = (int)data.Shoot2;
        Ultramode = (int)data.Ultramode;
        Pause = (int)data.Pause;
        Interact = (int)data.Interact;
        SwitchWeapon = (int)data.SwitchWeapon;
    }
}
