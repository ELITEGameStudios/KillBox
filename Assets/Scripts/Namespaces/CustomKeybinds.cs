using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class CustomKeybinds : MonoBehaviour
{
    public KeyCode Shoot {get; private set;}
    public KeyCode Shoot2 {get; private set;}
    public KeyCode Ultramode {get; private set;}
    public KeyCode Pause {get; private set;}
    public KeyCode Interact {get; private set;}
    public KeyCode SwitchWeapon {get; private set;}
    public KeyCode Dash {get; private set;} = KeyCode.Mouse1;
    public KeyCode AddRound {get; private set;} = KeyCode.Period;
    public KeyCode DecreaseRound {get; private set;} = KeyCode.Comma;
    public KeyCode AddScore {get; private set;} = KeyCode.Quote;
    public KeyCode DecreaseScore {get; private set;} = KeyCode.Semicolon;

    private int gamepadInteractPressedFrameCounter = 0;
    private bool gamepadInteractPressedLastFrame = false;

    private int gamepadBackPressedFrameCounter = 0;
    private bool gamepadBackPressedLastFrame = false;

    public bool ControllerInteract() { return DetectInputDevice.main.gamepad != null ? DetectInputDevice.main.gamepad.yButton.isPressed : false; }
    public bool ControllerDash() { return DetectInputDevice.main.gamepad != null ? DetectInputDevice.main.gamepad.rightShoulder.IsPressed() : false; }
    public bool ControllerBack() { return DetectInputDevice.main.gamepad != null ? DetectInputDevice.main.gamepad.bButton.IsPressed() : false; }

    [SerializeField]
    private Text shoot_txt, shoot2_txt, ultramode_txt, pause_txt, interact_txt, switch_txt;
    public bool editing_key {get; private set;}
    public string key_to_edit {get; private set;}
    public static CustomKeybinds main {get; private set;}
    [SerializeField] private HotkeyManager hotkeyManager;

    void Awake(){
        if(main == null){
            main = this;
        }
        else{
            Destroy(this);
        }
        
        LoadKeybinds();
        // DetectInputDevice.main.gamepad.GetChildControl<ButtonControl>("right_shoulder");
    }

    void SetDefaults(){

        Shoot = KeyCode.Mouse0;
        Shoot2 = KeyCode.Mouse1;
        Ultramode = KeyCode.F;
        Interact = KeyCode.E;
        SwitchWeapon = KeyCode.Space;
        Pause = KeyCode.Escape;

    }

    void LoadKeybinds(){

        if(KeybindsSave.LoadPlayer() != null){
            KeybindData data = KeybindsSave.LoadPlayer();

            Shoot = (KeyCode)data.Shoot;
            Shoot2 = (KeyCode)data.Shoot2;
            Ultramode = (KeyCode)data.Ultramode;
            Interact = (KeyCode)data.Interact;
            SwitchWeapon = (KeyCode)data.SwitchWeapon;
            Pause = (KeyCode)data.Pause;
        }
        else{
            SetDefaults();
            KeybindsSave.SavePlayer(this);
        }
    }

    public void SetKey(string option){
        key_to_edit = option;
        editing_key = true;
    }

    void FinalSetKey(KeyCode new_key){

        switch(key_to_edit){
            case "_shoot":
                Shoot = new_key;
                break;
            case "_shoot2":
                Shoot2 = new_key;
                break;
            case "_ultra":
                Ultramode= new_key;
                break;
            case "_interact":
                Interact = new_key;
                break;
            case "_switch":
                SwitchWeapon = new_key;
                break;
            case "_pause":
                Pause = new_key;
                break;
        }
    
        editing_key = false;
        KeybindsSave.SavePlayer(this);
    }

    
    public bool PressingInteract(bool ignoreFrameCounts = false) { 
        if(ignoreFrameCounts){

            return ControllerInteract() ||
            Input.GetKey(Interact); 
        }
        else{
            return (ControllerInteract() && !gamepadInteractPressedLastFrame) ||
            Input.GetKeyDown(Interact); 
        }
    }

    public bool PressingBack(bool ignoreFrameCounts = false) { 
        if(ignoreFrameCounts){

            return ControllerBack() ||
            Input.GetKey(Pause); 
        }
        else{
            return (ControllerBack() && !gamepadInteractPressedLastFrame) ||
            Input.GetKeyDown(Pause); 
        }
    }

    public KeyCode GetInput(){

        foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode)){
                Debug.Log("KeyCode down: " + kcode);

                return kcode;
            }
        }   

        return KeyCode.None;
    }

    public void Cancel(){
        editing_key = false;
    }

    bool CheckControllerPress(bool condition, int counter){

        if(condition){
            counter++;
            if(counter > 1){ return true; }
            return false;
        }

        counter= 0;
        return false;
    }

    void Update(){

        // if(CheckControllerPress(ControllerInteract(), gamepadInteractPressedFrameCounter)){
        //     gamepadInteractPressedLastFrame = true;
        //     gamepadInteractPressedFrameCounter++;
        // }
        // else{
        //     gamepadInteractPressedLastFrame = true;
        //     gamepadInteractPressedFrameCounter++;

        // }

        if(PressingBack()){
            hotkeyManager.PauseCheck();
            KillboxEventSystem.TriggerBackButtonEvent(true);
        }
        else if(PressingBack(true)){
            KillboxEventSystem.TriggerBackButtonEvent(false);
        }

        if(ControllerInteract()){
            gamepadInteractPressedFrameCounter++;
            if(gamepadInteractPressedFrameCounter > 1){ gamepadInteractPressedLastFrame = true; }
        }
        else{
            gamepadInteractPressedFrameCounter = 0;
            gamepadInteractPressedLastFrame = false;
        }

        if(ControllerBack()){
            gamepadBackPressedFrameCounter++;
            if(gamepadBackPressedFrameCounter > 1){ gamepadBackPressedLastFrame = true; }
        }
        else{
            gamepadBackPressedFrameCounter = 0;
            gamepadBackPressedLastFrame = false;
        }

        // shoot_txt.text = Shoot.ToString();
        // shoot2_txt.text = Shoot2.ToString();
        // ultramode_txt.text = Ultramode.ToString();
        // interact_txt.text = Interact.ToString();
        // switch_txt.text = SwitchWeapon.ToString();
        // pause_txt.text = Pause.ToString();
        
        // if(editing_key){

        //     switch(key_to_edit){
        //         case "_shoot":
        //             shoot_txt.text = "[Click any key]";
        //             break;
        //         case "_shoot2":
        //             shoot2_txt.text = "[Click any key]";
        //             break;
        //         case "_ultra":
        //             ultramode_txt.text= "[Click any key]";
        //             break;
        //         case "_interact":
        //             interact_txt.text = "[Click any key]";
        //             break;
        //         case "_switch":
        //             switch_txt.text = "[Click any key]";
        //             break;
        //         case "_pause":
        //             pause_txt.text = "[Click any key]";
        //             break;
        //     }
        //     if(GetInput() != KeyCode.None){
        //         FinalSetKey(GetInput());
        //     }
        // }
    }
}
