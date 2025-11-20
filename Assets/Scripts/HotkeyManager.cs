using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotkeyManager : MonoBehaviour
{
    [SerializeField] private bool equipment_pressed, switch_weapon_pressed, pause_pressed;
    [SerializeField] private bool equipment_lock, switch_weapon_lock, pause_lock;
    [SerializeField] private bool upgrades_lock, weapons_lock;
    // [SerializeField] private float equipment_float, switch_weapon_float, upgrades_float, weapons_float, pause_float;
    InputManager hotkeys;

    public static HotkeyManager instance { get; private set; }

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(this); }
        hotkeys = new InputManager();

        // hotkeys.Gameplay.equipment.started += ctx => equipment_pressed = true;
        // hotkeys.Gameplay.equipment.canceled += ctx => equipment_pressed = false;

        // hotkeys.Gameplay._switch.started += ctx => switch_weapon_pressed = true;
        // hotkeys.Gameplay._switch.canceled += ctx => switch_weapon_pressed = false;

        hotkeys.Gameplay.equipment.performed += EquipmentInputCall;
        hotkeys.Gameplay._switch.performed += SwitchWeaponInputCall;
        
        // hotkeys.Gameplay.pause.performed += ctx => PauseCheck();
        // hotkeys.Gameplay.pause.started += ctx => PauseCheck();
        // hotkeys.Gameplay.pause.canceled += ctx => pause_pressed = false;
    }
        // CustomKeybinds.main.SetHotkeyManager(this);

    void OnEnable() { hotkeys.Gameplay.Enable(); }
    void OnDisable() { hotkeys.Gameplay.Disable(); }

    public void EquipmentInputCall(InputAction.CallbackContext ctx)
    {
                EquipmentManager.instance.ActivateEquipment();
        
    }
    public void SwitchWeaponInputCall(InputAction.CallbackContext ctx)
    {
        if(GunHandler.Instance.backup_weapon != null)
        {
            GunHandler.Instance.EquipWeapon(backup: GunHandler.Instance.current_is_primary); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isMobilePlatform)
        {

            if (!equipment_pressed) { equipment_lock = false; }
            if (!switch_weapon_pressed) { switch_weapon_lock = false; }
            // if (!pause_pressed) { pause_lock = false; }

            if (equipment_lock) { equipment_pressed = false; }
            if (switch_weapon_lock) { switch_weapon_pressed = false; }
            // if (pause_lock) { pause_pressed = false; }

            if (equipment_pressed && !equipment_lock) { equipment_lock = true; }
            if (switch_weapon_pressed && !switch_weapon_lock) { switch_weapon_lock = true; }
            // if (pause_pressed && !pause_lock) { pause_lock = true; }

        }

        if (CustomKeybinds.main.PressingBack()){
            PauseCheck();
        }

        // OLD INPUT SYSTEM SWAP WEAPON
        // if (Input.GetKeyDown(CustomKeybinds.main.SwitchWeapon) || switch_weapon_pressed)
        // {
        // }


        // FOR FREEPLAY MODE ONLY
        if (GameManager.main != null)
        {
            // OLD INPUT SYSTEM EQUIPMENT
            // if (Input.GetKeyDown(CustomKeybinds.main.Ultramode) || equipment_pressed)
            // {
            //     EquipmentManager.instance.ActivateEquipment();
            // }

            if (GameManager.main.freeplay)
            {

                if (Input.GetKeyDown(CustomKeybinds.main.AddRound))
                {
                    GameManager.main.AddRound(1);
                }
                if (Input.GetKeyDown(CustomKeybinds.main.DecreaseRound))
                {

                    if (GameManager.main.LvlCount > 1) GameManager.main.AddRound(-1);
                }



                if (Input.GetKeyDown(CustomKeybinds.main.AddScore))
                {
                    GameManager.main.AddScore(1);
                }
                if (Input.GetKeyDown(CustomKeybinds.main.DecreaseScore))
                {
                    if (GameManager.main.ScoreCount > 0) GameManager.main.AddScore(-1);
                }
            }
        }
    }


    public void PauseCheck()
    {
        if (PauseHandler.main.paused) { MainMenuManager.instance.Resume(); }
        else if (!GameplayUI.instance.anyShopMenuOpen) { MainMenuManager.instance.Pause(); }
    }
}
