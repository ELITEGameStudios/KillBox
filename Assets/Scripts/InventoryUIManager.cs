using System.Collections;
using System.Collections.Generic;
using KillboxWeaponClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour, IBackButtonListener, IShopUIEventListener
{
    [SerializeField]
    private Button purchase_button, dual_button;

    [SerializeField]
    private Image purchase_button_graphic, targetImageDisplay, targetImageDisplayPanel, background;

    [SerializeField]
    private Text name_display, purchase_display, costsText, piercingText;

    public Color dim_shade, purchasable, error, purchasable_shade;

    public Color[] tier_colors, backgroundTierColors;

    public static InventoryUIManager Instance {get; private set;}

    public List<InventoryUIElement> main_buttons {get; private set;}

    public bool isOwned {get; private set;}
    public bool is_purchasable {get; private set;}
    [SerializeField] private bool activeMenu;

    [SerializeField]
    private ShopScript shop {get {return GameManager.main.shopScript;}}

    private WeaponItem target_item;

    [SerializeField]
    private GameObject primary_button, secondary_button, dual_button_obj, equippedPrimary, equippedSecondary, equippedDual;
    public InventoryUIElement primary_element, secondary_element, dual_element;
    
    [SerializeField] private UnityEvent onPurchaseAttempt, onBackButton;
    [SerializeField] private bool firstFrame, uiInitialized;

    public string target_key;
    public Image GetBackground() {return background;}

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        main_buttons = new List<InventoryUIElement>();
        activeMenu = false;
    }
    public void Initialize(){
        OnSetTargetKey("Pistol");
    }

    public void AddMainButton(InventoryUIElement target){
        Instance.main_buttons.Add(target);
    }

    public void InitializeUI(){
        foreach (InventoryUIElement item in main_buttons)
        {
            item.InitializeUIElement();
        }
    }

    public void UpdateUI(){
        if(target_item == null) return;
        if(!uiInitialized){InitializeUI(); uiInitialized = true;}
        OwnedCheck();
        TargetCheck();
    }
    void OwnedCheck()
    {
        List<WeaponItem> owned_guns = GunHandler.Instance.owned_weapons; 

        isOwned = false;

        bool is_purchasable = target_item.Compare(GameManager.main.ScoreCount);

        for(int i = 0; i < owned_guns.Count; i++){
            if(owned_guns[i].name == target_key){
                isOwned = true;
            }
        }

        for(int i = 0; i < main_buttons.Count; i++){
            if(isOwned){
                main_buttons[i].OnOwnedCheck();
            }
            else{

                //if(is_purchasable)
                main_buttons[i].OnOwnedCheck();//, purchasable: true);
                //else
                //    main_buttons[i].OnOwnedCheck(false);
            }
        }
    }

    void TargetCheck()
    {
        equippedPrimary.SetActive(false);
        equippedSecondary.SetActive(false);
        equippedDual.SetActive(false);

        is_purchasable = target_item.Compare(GameManager.main.ScoreCount);

        if(target_item != null && !isOwned){

            if(is_purchasable){
                purchase_button.interactable = true;
                //purchase_button_graphic.color = purchasable;
                
                if(DetectInputDevice.main.isKBM) {
                    purchase_display.text = "Press " + CustomKeybinds.main.Interact.ToString() + " to Purchase "+ target_item.name;
                }
                else if(DetectInputDevice.main.isController) {
                    purchase_display.text = "Press Y to Purchase "+ target_item.name;
                }

                // purchase_display.text = "Purchase "+ target_item.name;
                costsText.text = target_item.price.ToString();
                costsText.color = Color.Lerp(Color.white, tier_colors[target_item.tier], 0.8f);
            }

            else if(!isOwned){

                purchase_button.interactable = false;
                //purchase_button_graphic.color = error;

                purchase_display.text = "You Don't Have Enough Tokens For " + target_item.name;
                if(target_item.price == -1){ costsText.text = "?"; }
                else{ costsText.text = target_item.price.ToString(); }
                costsText.color = Color.Lerp(Color.white, tier_colors[target_item.tier], 0.8f);

                if(target_item.non_purchase_desc != "Unpurchasable"){
                    purchase_display.text = target_item.non_purchase_desc;
                }
            }

            name_display.text = target_key;
            background.color = backgroundTierColors[target_item.tier];
            if(target_item.weapon.pool == 10){
                piercingText.text = "Explodes on Collision";
            }
            else if(target_item.weapon == SupportLibrary.slow_field_large){
                piercingText.text = "Slows Enemies in a Large Area";
            }
            else if(target_item.weapon == SpecialistLibrary.kunais_2){
                piercingText.text = "This Weapon is Decent...";
            }
            else{
                piercingText.text = target_item.weapon.penetration > 0 ?
                "PIERCING: " + target_item.weapon.penetration + "\n(Goes Through 1 Enemy | Ignores Walls)"
                : "Standard"; 
            }
        }

        else if(isOwned){
            purchase_button.interactable = false;
            //purchase_button_graphic.color = dim_shade;
            purchase_display.text = "Owned";

            costsText.text = "";
            // costsText.color = Color.Lerp(Color.white, tier_colors[target_item.tier], 0.8f);

            primary_button.GetComponent<Button>().interactable = false;
            secondary_button.GetComponent<Button>().interactable = false;
            dual_button_obj.GetComponent<Button>().interactable = false;

            if(target_item != GunHandler.Instance.primary_weapon){
                primary_button.GetComponent<Button>().interactable = true;
            }
            else{
                equippedPrimary.SetActive(true);
            }

            if(target_item != GunHandler.Instance.backup_weapon){
                if(GunHandler.Instance.owned_weapons.Count > 1){
                    secondary_button.GetComponent<Button>().interactable = true;
                }
            }
            else{
                equippedSecondary.SetActive(true);
            }

            if(GunHandler.Instance.owns_dual && GunHandler.Instance.dual_weapon != target_item){
                dual_button_obj.GetComponent<Button>().interactable = true;
            }
            else if(GunHandler.Instance.owns_dual){
                equippedDual.SetActive(true);
            }

        }
        
        name_display.text = target_key;
        targetImageDisplay.sprite = target_item.graphic;
        targetImageDisplay.color = tier_colors[target_item.tier];
        targetImageDisplayPanel.color = Color.Lerp(Color.clear, tier_colors[target_item.tier], 0.48f);

        for(int i = 0; i < Instance.main_buttons.Count; i++){
            Instance.main_buttons[i].OnTargetCheck();
        }
    }

    void ButtonCheck(){
        if(Instance.isOwned && Instance.target_item != GunHandler.Instance.primary_weapon){
            primary_button.GetComponent<Button>().interactable = true;
        }
        else{
            primary_button.GetComponent<Button>().interactable = false;
        }
    }

    void ActivateMenu(bool _active){
        activeMenu = _active;
    }

    public void OnSetTargetKey(string key)
    {

        target_key = key;
        target_item = WeaponItemList.Instance.GetItem(target_key);

        KillboxEventSystem.TriggerWeaponButtonSelectEvent(target_item);
        
        OwnedCheck();
        TargetCheck();
        ButtonCheck();
        //OwnedCheck();
    }

    public void PurchaseCall(){
        if(GameManager.main.EscapeRoom()){
            WeaponItemList.Instance.GetItem(target_key).owned = true;
            GunHandler.Instance.NewItem(WeaponItemList.Instance.GetItem(target_key));
        }
        else{
            shop.PurchaseGun(target_key);
        }
        OwnedCheck();
        TargetCheck();

        // Backup();

        for(int i = 0; i < Instance.main_buttons.Count; i++){
            Instance.main_buttons[i].EquipDisplay();
        }
    }

    public void EquipCall(bool dual){

        if(Instance.target_item.owned){
            if(!dual)
            {
                GunHandler.Instance.EquipWeapon(target_item.name);
                primary_button.GetComponent<Button>().interactable = false;

                KillboxEventSystem.TriggerSetNewWeaponEvent(target_item, 0);
            }
            else
            {
                GunHandler.Instance.EquipWeapon(Instance.target_key, dual: true);
                dual_button_obj.GetComponent<Button>().interactable = false;

                KillboxEventSystem.TriggerSetNewWeaponEvent(target_item, 2);
            }
        }

        OwnedCheck();
        TargetCheck();

        for(int i = 0; i < Instance.main_buttons.Count; i++){
            Instance.main_buttons[i].EquipDisplay();
        }
    }
    public void Backup(){
        if(Instance.target_item.owned && (target_item != GunHandler.Instance.primary_weapon)){

            GunHandler.Instance.SetBackup(Instance.target_key);
            secondary_button.GetComponent<Button>().interactable = false;

            KillboxEventSystem.TriggerSetNewWeaponEvent(target_item, 1);
        }

        OwnedCheck();
        TargetCheck();

        for(int i = 0; i < Instance.main_buttons.Count; i++){
            Instance.main_buttons[i].EquipDisplay();
        }
    }

    public void EquipInGameplay(int id){

        switch (id){
            case 0 : {
                //Instance.target_key = GunHandler.Instance.primary_weapon.name;
                //Instance.target_item = WeaponItemList.Instance.GetItem(target_key);
//
                //if(Instance.target_item.owned && Instance.target_item != GunHandler.Instance.primary_weapon){
                //    GunHandler.Instance.EquipWeapon(Instance.target_key);
                //    //primary_button.SetActive(false);
                //}

                GunHandler.Instance.EquipWeapon(Instance.target_key);

                break;
            }
            case 1 : {
                //Instance.target_key = GunHandler.Instance.backup_weapon.name;
                //Instance.target_item = WeaponItemList.Instance.GetItem(target_key);
//
                //if(Instance.target_item.owned && Instance.target_item != GunHandler.Instance.backup_weapon){
                //    GunHandler.Instance.EquipWeapon(Instance.target_key);
                //    //primary_button.SetActive(false);
                //}

                GunHandler.Instance.EquipWeapon(backup: true);

                break;
            }
            case 2 : {
                //Instance.target_key = GunHandler.Instance.dual_weapon.name;
                //Instance.target_item = WeaponItemList.Instance.GetItem(target_key);
//
                //if(Instance.target_item.owned && Instance.target_item != GunHandler.Instance.dual_weapon){
                //    GunHandler.Instance.EquipWeapon(Instance.target_key);
                //    //primary_button.SetActive(false);
                //}

                GunHandler.Instance.EquipWeapon(Instance.target_key, dual: true);

                break;
            }
        }

        //if(Instance.target_item.owned){
        //}

        OwnedCheck();
        TargetCheck();

        for(int i = 0; i < Instance.main_buttons.Count; i++){
            Instance.main_buttons[i].EquipDisplay();
        }
    }

    void Update(){
        if(is_purchasable && target_item != null && !isOwned && background.gameObject.activeInHierarchy){
            
            if(DetectInputDevice.main.isKBM) {
                purchase_display.text = "Press " + CustomKeybinds.main.Interact.ToString() + " to Purchase and Equip "+ target_item.name;
            }
            else if(DetectInputDevice.main.isController) {
                purchase_display.text = "Press Y to Purchase and Equip "+ target_item.name;
            }

            

            if(CustomKeybinds.main.PressingInteract() && !firstFrame){
                Debug.Log("FAKEE");
                onPurchaseAttempt.Invoke();
            }
        }

    }

    void LateUpdate(){
        if(firstFrame){
            firstFrame = false;
        }
    }
    public void OnBackButton(bool pressedThisFrame)
    {
        if(pressedThisFrame && background.gameObject.activeInHierarchy){
            onBackButton.Invoke();
            GameManager.main.SetInGameButtonHandlers(true);
            KillboxEventSystem.TriggeCloseShopEvent();
            activeMenu = false;
        }
    }

    public void OnOpenShop(int shopId)
    {
        if(background.gameObject.activeInHierarchy){
            activeMenu = true;
            firstFrame = true;
            OnSetTargetKey("Pistol");
        }
    }

    public void OnCloseShop()
    {}

    public void OnSetNewWeapon(WeaponItem weaponItem, int slot)
    {}
}
