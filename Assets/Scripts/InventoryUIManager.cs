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
    private Button purchase_button;

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
    [SerializeField] private bool firstFrame, uiInitialized, menuIsOpened;

    [Header("Weapon Stat Texts")]
    [SerializeField] private Text frText;
    [SerializeField] private Text rangeText, capacityText, spreadText, dmgText, bpsText;
    public int loopyCharsPerSecond;


    [Header("Purchase token")]
    [SerializeField] private Text purchaseCostDisplay;
    [SerializeField] private GameObject tokenGraphicObject;

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

    public void OpenMenu()
    {
        background.gameObject.SetActive(true);
        CancelInvoke(nameof(CloseMenuInvoke));
        GetComponent<Animator>().SetTrigger("Open");
        Player.main.movement.OnOpenShop(0);
        activeMenu = true;
    }

    public void CloseMenuInvoke()
    {
        background.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        if (target_item == null) return;
        if (!uiInitialized) { InitializeUI(); uiInitialized = true; }
        OwnedCheck();
        TargetCheck();
    }
    void OwnedCheck()
    {
        List<WeaponItem> owned_guns = GunHandler.Instance.owned_weapons; 

        isOwned = false;

        for(int i = 0; i < owned_guns.Count; i++){
            if(owned_guns[i].name == target_key){
                isOwned = true;
            }
        }
    }

    void TargetCheck()
    {
        equippedPrimary.SetActive(false);
        equippedSecondary.SetActive(false);
        equippedDual.SetActive(false);
        tokenGraphicObject.SetActive(false);

        // Setting equip button animators
        primary_element.GetAnimator().SetBool("Equippable", false);
        secondary_element.GetAnimator().SetBool("Equippable", false);
        dual_element.GetAnimator().SetBool("Equippable", false);

        is_purchasable = target_item.Compare(GameManager.main.ScoreCount);

        if(target_item != null && !isOwned){
            bool needsBaseUpgrade = target_item.tier > 1 && target_item.tier < 4;

            if (!KillBox.currentGame.hasUpgradedArsenal && needsBaseUpgrade)
            {
                // Locked high tier weapons before SHARD
                purchase_button.interactable = false;
                purchase_display.text = "Defeat SHARD to purchase this weapon...";
                // costsText.text = "?";
                costsText.text = target_item.price.ToString();
                costsText.color = Color.Lerp(Color.white, tier_colors[target_item.tier], 0.8f);
                return;
            }

            if( Player.main.specialUpgrade != UpgradesList.SpecialUpgrades.GOLDEN && target_item.tier == 4 ){
                // Locked gold weapon without midas special
                purchase_display.text = Player.main.specialUpgrade == UpgradesList.SpecialUpgrades.NONE ? "Defeat MIDAS to purchase this weapon..." : "You chose your path...";
                purchase_button.interactable = false;
                // costsText.text = "?";
                costsText.text = target_item.price.ToString();
                costsText.color = Color.Lerp(Color.white, tier_colors[target_item.tier], 0.8f);
                return;
            }

            tokenGraphicObject.SetActive(true);
            purchaseCostDisplay.text = target_item.price.ToString();

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
            else if(target_item.weapon == SupportLibrary.serenity){
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
            tokenGraphicObject.SetActive(false);
            purchase_button.interactable = false;
            //purchase_button_graphic.color = dim_shade;
            purchase_display.text = "Owned";

            costsText.text = "";
            // costsText.color = Color.Lerp(Color.white, tier_colors[target_item.tier], 0.8f);

            primary_button.GetComponent<Button>().interactable = false;
            secondary_button.GetComponent<Button>().interactable = false;
            dual_button_obj.GetComponent<Button>().interactable = false;

            // What a nice repeating set of code (at least it works [right?])
            // Checks if the selected and owned weapon is not the current primary, secondary, or dual weapon
            if (target_item != GunHandler.Instance.primary_weapon)
            {
                primary_button.GetComponent<Button>().interactable = true;
                primary_element.GetAnimator().SetBool("Equippable", true);
            }
            else
            {
                // primary_element.GetAnimator().SetBool("Equippable", false);
                equippedPrimary.SetActive(true);
            }

            if(target_item != GunHandler.Instance.backup_weapon){
                if(GunHandler.Instance.owned_weapons.Count > 1){
                    secondary_button.GetComponent<Button>().interactable = true;
                    secondary_element.GetAnimator().SetBool("Equippable", true);
                }
            }
            else{
                // secondary_element.GetAnimator().SetBool("Equippable", false);
                equippedSecondary.SetActive(true);
            }

            if(GunHandler.Instance.owns_dual && GunHandler.Instance.dual_weapon != target_item){
                dual_button_obj.GetComponent<Button>().interactable = true;
                dual_element.GetAnimator().SetBool("Equippable", true);
            }
            else if(GunHandler.Instance.owns_dual){
                equippedDual.SetActive(true);
                // dual_element.GetAnimator().SetBool("Equippable", false);
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

        // -------------- Setting Weaopon Stat Texts ----------------------
        float fireRate = target_item.weapon.fire_rate;
        int damage = target_item.weapon.damage;
        float capacity = target_item.weapon.cooldown_units * target_item.weapon.bullets_per_shot / target_item.weapon.fire_rate;
        float spread = target_item.weapon.spread;
        
        if (fireRate < 0.05f) { frText.text = "INSANE"; frText.color = tier_colors[4]; }
        else if (fireRate < 0.15f) { frText.text = "FAST"; frText.color = tier_colors[3]; }
        else if (fireRate < 0.25f) { frText.text = "AVERAGE"; frText.color = tier_colors[2]; }
        else if (fireRate < 0.7f) { frText.text = "SLOW"; frText.color = tier_colors[1]; }
        else { frText.text = "ABYSMAL"; frText.color = tier_colors[0]; }

        if (damage >= 300 ) { dmgText.text = "INSANE"; dmgText.color = tier_colors[4]; }
        else if (damage >= 150) { dmgText.text = "POWERFUL"; dmgText.color = tier_colors[3]; }
        else if (damage >= 75) { dmgText.text = "STRONG"; dmgText.color = tier_colors[2]; }
        else if (damage >= 25) { dmgText.text = "MEDIOCRE"; dmgText.color = tier_colors[1]; }
        else { dmgText.text = "SOFT"; dmgText.color = tier_colors[0]; }

        if ( capacity <= 0) { capacityText.text = "INFINITE"; capacityText.color = tier_colors[4]; }
        else if ( capacity <= 7) { capacityText.text = "PLENTIFUL"; capacityText.color = tier_colors[3]; }
        else if ( capacity < 20) { capacityText.text = "GOOD"; capacityText.color = tier_colors[2]; }
        else if ( capacity <= 50) { capacityText.text = "SMALL"; capacityText.color = tier_colors[1]; }
        else { capacityText.text = "EMPTY"; capacityText.color = tier_colors[0]; }

        if (spread == 0 ) { spreadText.text = "PINPOINT"; spreadText.color = tier_colors[4]; }
        else if (spread <= 5) { spreadText.text = "GREAT"; spreadText.color = tier_colors[3]; }
        else if (spread <= 15) { spreadText.text = "ALRIGHT"; spreadText.color = tier_colors[2]; }
        else if (spread <= 30) { spreadText.text = "BAD"; spreadText.color = tier_colors[1]; }
        else { spreadText.text = "LOST"; spreadText.color = tier_colors[0]; }

        bpsText.text = target_item.weapon.bullets_per_shot.ToString(); bpsText.color = Color.white;

        if (target_item.loopyDesc != "" && LoopyScript.weapons != null)
        {
            LoopyScript.weapons.AddState(
                new LoopyState(LoopyPose.NEUTRAL, target_item.loopyDesc, target_item.loopyDesc.Length / loopyCharsPerSecond),
                true
            );
        }
        
        // Required functions
        OwnedCheck();
        TargetCheck();
        ButtonCheck();
        //OwnedCheck();
    }

    public void PurchaseCall() {
        if (GameManager.main.EscapeRoom())
        {
            WeaponItemList.Instance.GetItem(target_key).owned = true;
            GunHandler.Instance.NewItem(WeaponItemList.Instance.GetItem(target_key));
        }
        else if (
            (!KillBox.currentGame.hasUpgradedArsenal && target_item.tier > 1 && target_item.tier < 4) ||
            (Player.main.specialUpgrade != UpgradesList.SpecialUpgrades.MASTERY && target_item.tier == 4)
        ) 
        {}
        // { return; }
        // else
        // {
            GameManager.main.shopScript.PurchaseGun(target_key);
        // }
        OwnedCheck();
        TargetCheck();

        // Backup();

        for(int i = 0; i < Instance.main_buttons.Count; i++){
            Instance.main_buttons[i].EquipDisplay();
        }
    }

    public void EquipCall(bool dual){
        // Equips primary or dual weapon

        if(Instance.target_item.owned){
            if(!dual)
            {
                GunHandler.Instance.EquipWeapon(target_item.name);
                primary_button.GetComponent<Button>().interactable = false;
                primary_element.GetAnimator().SetTrigger("Equip");

                KillboxEventSystem.TriggerSetNewWeaponEvent(target_item, 0);
            }
            else
            {
                GunHandler.Instance.EquipWeapon(Instance.target_key, dual: true);
                dual_button_obj.GetComponent<Button>().interactable = false;
                dual_element.GetAnimator().SetTrigger("Equip");

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
        // Equips backup weapon

        // if (Instance.target_item.owned && (target_item != GunHandler.Instance.primary_weapon))
        if (Instance.target_item.owned)
        {

            GunHandler.Instance.SetBackup(Instance.target_key);
            secondary_button.GetComponent<Button>().interactable = false;
            secondary_element.GetAnimator().SetTrigger("Equip");

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

            

            if(CustomKeybinds.main.PressingInteract() && !firstFrame && activeMenu){
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
        if (pressedThisFrame && background.gameObject.activeInHierarchy)
        {
            
            GameManager.main.SetInGameButtonHandlers(true);
            GetComponent<Animator>().SetTrigger("Close");
            onBackButton.Invoke();
            activeMenu = false;
            Player.main.movement.OnCloseShop();
            KillboxEventSystem.TriggeCloseShopEvent();
            Invoke(nameof(CloseMenuInvoke), 0.25f);

            CustomKeybinds.main.performedBackFunctionThisFrame = true;
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
