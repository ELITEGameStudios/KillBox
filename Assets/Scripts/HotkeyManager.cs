using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotkeyManager : MonoBehaviour
{
    public PauseHandler pauseHandler;
    public GameManager manager;
    

    [SerializeField] private TwoDPlayerController p_control;
    [SerializeField] private bool mobile;

    public GameObject inv_bg, upgrades_bg, inv_bt, upgrades_bt, pause_bg, pause_bt; 
    private bool equipment_pressed, switch_weapon_pressed, upgrades_pressed, weapons_pressed, pause_pressed; 
    private bool equipment_lock, switch_weapon_lock, upgrades_lock, weapons_lock, pause_lock; 
    private float equipment_float, switch_weapon_float, upgrades_float, weapons_float, pause_float; 
    InputManager hotkeys; 

    void Awake()
    {
        
        hotkeys = new InputManager();

        hotkeys.Gameplay.equipment.started  += ctx => equipment_pressed = true;
        hotkeys.Gameplay.equipment.canceled += ctx => equipment_pressed = false;

        hotkeys.Gameplay._switch.started  += ctx => switch_weapon_pressed = true;
        hotkeys.Gameplay._switch.canceled += ctx => switch_weapon_pressed = false;

        //hotkeys.Gameplay.pause.performed += ctx => pause_pressed = ctx.ReadValue<bool>();
        hotkeys.Gameplay.pause.started += ctx => pause_pressed = true;
        hotkeys.Gameplay.pause.canceled += ctx => pause_pressed = false;
    }

    void OnEnable()
    {
        hotkeys.Gameplay.Enable();
    }
    void OnDisable()
    {
        hotkeys.Gameplay.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isMobilePlatform){

            if(!equipment_pressed){equipment_lock = false;}
            if(!switch_weapon_pressed){switch_weapon_lock = false;}
            if(!pause_pressed){pause_lock = false;}

            if(equipment_lock){equipment_pressed = false;}
            if(switch_weapon_lock){switch_weapon_pressed = false;}
            if(pause_lock){pause_pressed = false;}

            if(equipment_pressed && !equipment_lock) {equipment_lock = true;}
            if(switch_weapon_pressed && !switch_weapon_lock) {switch_weapon_lock = true;}
            if(pause_pressed && !pause_lock) {pause_lock = true;}

        //if(pause_float == 1){
        //    pause_pressed = true;
        //}
        //else{
        //    pause_pressed= false;}
//
        //if(equipment_float == 1){
        //    equipment_pressed = true;
        //}
        //else{
        //    equipment_pressed= false;}
//
        //if(switch_weapon_float == 1){
        //    switch_weapon_pressed = true;
        //}
        //else{
        //    switch_weapon_pressed= false;}


            if(pause_pressed){

                if(!inv_bg.activeInHierarchy && !upgrades_bg.activeInHierarchy){
                    if(pause_bg.activeInHierarchy && !pause_bt.activeInHierarchy){
                        pause_bg.SetActive(false);
                        pause_bt.SetActive(true);
                        GunHandler.Instance.SetUIStatus(false);

                        pauseHandler.PausePlay(1);
                    }
                    else{
                        pause_bg.SetActive(true);
                        pause_bt.SetActive(false);

                        GunHandler.Instance.SetUIStatus(true);
                        pauseHandler.PausePlay(0);
                    }
                }
                
            }   

            //if(Input.GetKeyDown(KeyCode.R)){
            //    if(inv_bg.activeInHierarchy && !inv_bt.activeInHierarchy){
            //        inv_bg.SetActive(false);
            //        inv_bt.SetActive(true);
            //    }
            //    else{
            //        inv_bg.SetActive(true);
            //        inv_bt.SetActive(false);
            //    }
            //}   

            //if(Input.GetKeyDown(KeyCode.T)){
            //    if(upgrades_bg.activeInHierarchy && !upgrades_bt.activeInHierarchy){
            //        upgrades_bg.SetActive(false);
            //        upgrades_bt.SetActive(true);
            //    }
            //    else{
            //        upgrades_bg.SetActive(true);
            //        upgrades_bt.SetActive(false);
            //    }
            //}  

            if(Input.GetKeyDown(CustomKeybinds.main.SwitchWeapon) || switch_weapon_pressed){
                if(GunHandler.Instance.current_is_primary){
                    GunHandler.Instance.EquipWeapon(backup: true);
                }
                else{
                    GunHandler.Instance.EquipWeapon();
                }
            }   

            if(Input.GetKeyDown(CustomKeybinds.main.Ultramode) || equipment_pressed){
                if(manager.ultra_kills >= manager.ReqUltraKills){
                    EquipmentManager.instance.ActivateEquipment();
                }
            }   

            // if(Input.GetKeyDown("p")){
            //     p_control.controller = !p_control.controller;
                
            // }   

            // FOR FREEPLAY MODE ONLY
            if(GameManager.main.freeplay){

                if(Input.GetKeyDown(CustomKeybinds.main.AddRound)){
                    GameManager.main.AddRound(1);
                }   
                if(Input.GetKeyDown(CustomKeybinds.main.DecreaseRound)){

                    if(GameManager.main.LvlCount > 1) GameManager.main.AddRound(-1);
                }   



                if(Input.GetKeyDown(CustomKeybinds.main.AddScore)){
                    GameManager.main.AddScore(1);
                }   
                if(Input.GetKeyDown(CustomKeybinds.main.DecreaseScore)){
                    if(GameManager.main.ScoreCount > 0) GameManager.main.AddScore(-1);
                }   

            }
        }
        //if(Input.GetKeyDown("i")){
        //    GameManager.main.AddRound();
        //    
        //}   

    }

    public void PauseCheck(){

        if(Input.GetKeyDown(CustomKeybinds.main.Pause) || pause_pressed){

            if(!inv_bg.activeInHierarchy && !upgrades_bg.activeInHierarchy){
                if(PauseHandler.main.paused){
                    PauseHandler.main.PausePlay(1);
                    GunHandler.Instance.SetUIStatus(false);
                }

                else{
                    PauseHandler.main.PausePlay(0);
                    GunHandler.Instance.SetUIStatus(true);
                }

                // if(pause_bg.activeInHierarchy && !pause_bt.activeInHierarchy){
                //     pause_bg.SetActive(false);
                //     pause_bt.SetActive(true);

                //     pauseHandler.PausePlay(1);
                // }
                // else{
                //     pause_bg.SetActive(true);
                //     pause_bt.SetActive(false);

                //     GunHandler.Instance.SetUIStatus(true);
                //     pauseHandler.PausePlay(0);
                // }
            }
            
        }   
    }
}
