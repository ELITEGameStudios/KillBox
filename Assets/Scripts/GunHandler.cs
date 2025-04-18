using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    public WeaponItem primary_weapon {get; private set;}
    
    public CooldownScript cooldown {get; private set;}
    public CooldownScript primary_cooldown {get; private set;}
    public CooldownScript secondary_cooldown {get; private set;}

    public WeaponItem equipped_weapon {get; private set;}
    public WeaponItem backup_weapon {get; private set;}
    public WeaponItem dual_weapon {get; private set;}
    public List<WeaponItem> owned_weapons = new List<WeaponItem>();
    public bool current_is_primary {get; private set;}
    public bool owns_dual {get; private set;}
    public bool in_ui {get; private set;}

    public bool has_dual {get; private set;}

    [SerializeField] private GameObject dual_gameObject, main_object;

    [SerializeField] private Button primary_button, secondary_button;

    [SerializeField] private shooterScript2D shooter_script, dual_shooter_script;
    
    [SerializeField] private Dictionary<string, GameObject> special_guns = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject kunai_obj, coolingDownObject;
    [SerializeField] private AudioSource equippedAudio;
    public static GunHandler Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null){ Instance = this; }
        else if(Instance != this){ Destroy(this); }

    }

    void Start(){

        primary_button = GameplayUI.instance.GetPrimaryWeaponButton();
        secondary_button = GameplayUI.instance.GetBackupWeaponButton();
        
        Instance.NewItem(WeaponItemList.Instance.GetItem("Pistol"));
        // Instance.EquipWeapon("Pistol");
        if(GameManager.main.EscapeRoom()){
            Instance.NewItem(WeaponItemList.Instance.GetItem("Revolver"));
        }
        
        Instance.special_guns.Add("_kunai", kunai_obj);

    }



    public void NewItem(WeaponItem item, bool auto_equip = false)
    {
        // Adds item to owned list
        if(item.owned){
            Instance.owned_weapons.Add(item);
            ChallengeFields.UpdateArsenal(item);

            if(primary_weapon == null){
                primary_weapon = item;
                EquipWeapon();
                return;
            }

            // Adds other weapon as backup if you have none
            //if(backup_weapon == null){
            //    backup_weapon = primary_weapon;
            //    primary_weapon = item;
//
            //    if(auto_equip){
            //        EquipWeapon();
            //    }
            //    else{
            //        EquipWeapon(backup: true);
            //    }
            //}

            else{

                if(auto_equip){
                    if(equipped_weapon == backup_weapon){
                        SetBackup(item.name);
                        // backup_weapon = item;
                        // EquipWeapon();
                    }
                    else{

                        primary_weapon = item;
                        EquipWeapon();
                    }
                }
            }
        }

        UIRefresh();
    }

    public void SetUIStatus(bool _status){
        in_ui = _status;
    }

    public void PurchaseDual(){
        dual_gameObject.SetActive(true);
        owns_dual = true;
        EquipWeapon(key: "Pistol", dual: true);
    }

    public void SetCooldown(int id, CooldownScript instance){
        if(id == 0 && primary_cooldown == null){
            primary_cooldown = instance;
            cooldown = primary_cooldown;
        }
        else if(id == 1 && secondary_cooldown == null){
            secondary_cooldown = instance;
        }

        cooldown.AnimCheck();
    }
    
    public void EquipWeapon(string key = "", bool backup = false, bool dual = false)
    {

        WeaponItem item = WeaponItemList.Instance.GetItem(key);

        if(backup_weapon == null && backup && item == null){ return; }


        if( (!backup && primary_weapon.special_key != "") || (backup && backup_weapon.special_key != "")){
            Debug.Log("Entered Special Block");
            DisableAllWeaponObjects();
        
            if(backup){
                special_guns[backup_weapon.special_key].SetActive(true);
                Instance.equipped_weapon = backup_weapon;
                Debug.Log("Backup: " + backup_weapon.special_key);
            }
            else{
  
                if(item != null){
                    if(backup_weapon == null && primary_weapon != null){
                        backup_weapon = primary_weapon;
                    }

                    else if(backup_weapon == item && primary_weapon != null){
                        backup_weapon = primary_weapon;
                    }
                    primary_weapon = item;
                }
                
                special_guns[primary_weapon.special_key].SetActive(true);
                Debug.Log("Backup: " + primary_weapon.special_key);
                //Instance.shooter_script.SetWeapon(primary_weapon.weapon);
                Instance.equipped_weapon = primary_weapon;
            }
        }
        else{
            
            if(!main_object.activeInHierarchy){
                DisableAllWeaponObjects();
                main_object.SetActive(true);
            }
        
            if(!dual){
                if(backup){
                    Instance.shooter_script.SetWeapon(backup_weapon);
                    Instance.equipped_weapon = backup_weapon;
                }
                else{
                    if(item != null){
                        if(backup_weapon == item && primary_weapon != null){
                            backup_weapon = primary_weapon;
                        }

                        primary_weapon = item;
                    }
                    Instance.shooter_script.SetWeapon(primary_weapon);
                    Instance.equipped_weapon = primary_weapon;
                }
            }
            else{
                if(item == primary_weapon){
                    if(dual_weapon == null){
                        primary_weapon = owned_weapons[0];
                    }
                    else{
                        primary_weapon = dual_weapon;
                    }
                }
                else if(item == backup_weapon){
                    if(dual_weapon == null){
                        backup_weapon = owned_weapons[0];
                    }
                    else{
                        backup_weapon = dual_weapon;
                    }
                }

                Instance.dual_weapon = item;
                Instance.dual_shooter_script.SetWeapon(item);
                owns_dual = true;
            }
        }
        

        if(equipped_weapon == primary_weapon){
            current_is_primary = true;
            primary_button.interactable = false;
            secondary_button.interactable = true;

            Instance.cooldown = primary_cooldown;
        }
        else{
            current_is_primary = false;
            primary_button.interactable = true;
            secondary_button.interactable = false;

            Instance.cooldown = secondary_cooldown;
        }

        UIRefresh();
        if(GameManager.main.started_game){
            equippedAudio.Play();
        }

        if(primary_cooldown != null && secondary_cooldown != null){
            Instance.primary_cooldown.UpdateCooldownTime();
            Instance.secondary_cooldown.UpdateCooldownTime();
        }

        KillboxEventSystem.TriggerSwitchWeaponEvent(new WeaponEventData(item, !backup, has_dual));

    }

    public void DisableAllWeaponObjects(){
        main_object.SetActive(false);
        dual_gameObject.SetActive(false);
        kunai_obj.SetActive(false);
    }

    public void ResetWeapons(){

        EquipWeapon("Pistol");
        backup_weapon = null;
        dual_weapon = null;
        
        for(int i = 0; i < owned_weapons.Count; i++){
            owned_weapons[i].Reset();
        }

        owned_weapons.Clear();
        
        has_dual = false;
        dual_gameObject.SetActive(false);
        kunai_obj.SetActive(false);



        NewItem(WeaponItemList.Instance.GetItem("Pistol"));
        UIRefresh();
    }
    public void SwapWeapon(bool backup){
        
        WeaponEventData data;
        if(backup)
        {
            // if(backup_weapon == null){ return; }
            Instance.EquipWeapon(backup: true);
            data = new WeaponEventData(backup_weapon, false, has_dual);
        }
        else
        {
            Instance.EquipWeapon();
            data = new WeaponEventData(primary_weapon, true, has_dual);
        }

        KillboxEventSystem.TriggerSwitchWeaponEvent(data);
    }

    public void SetBackup(string key)
    {
        WeaponItem item = WeaponItemList.Instance.GetItem(key);
        if (item.owned)
        {
            if(backup_weapon == null){
                Instance.backup_weapon = item;
            }
            else if(item == primary_weapon){
                primary_weapon = backup_weapon;
            }
            Instance.backup_weapon = item;


            if(!Instance.current_is_primary){
                EquipWeapon(backup: true);
            }
            
            
        }

        UIRefresh();
    }

    private void UIRefresh(){
        GameplayUI.instance.WeaponsUIRefresh(this);
    }

    public void OnShoot(){
        if(current_is_primary){
            InventoryUIManager.Instance.primary_element.ShootAnimation();
        }
        else{
            InventoryUIManager.Instance.secondary_element.ShootAnimation();
        }

        if(has_dual){
            InventoryUIManager.Instance.dual_element.ShootAnimation();
        }
    }
    
    void Update(){
        if(cooldown != null){
            if(!GameplayUI.instance.GetCooldownText().gameObject.activeInHierarchy){
                GameplayUI.instance.GetCooldownText().gameObject.SetActive(true);
            }
            // if(cooldown.cooling_down){
            //     GameplayUI.instance.GetCooldownText().gameObject.SetActive(true);
            // }
            // else{
            //     GameplayUI.instance.GetCooldownText().gameObject.SetActive(false);
            // }
        }
    }
}
