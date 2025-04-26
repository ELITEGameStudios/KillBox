using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour, IBackButtonListener
{
    public int[] current_levels {get; private set;} = new int[] {0, 0, 0, 0, 0};

    [SerializeField] private ShopScript shop {get {return GameManager.main.shopScript;}}

    public Color purchasable, error, purchasable_text_color;
    [SerializeField] private Color[] desc_panel_colors;
    [SerializeField] private Image purchase_button_graphic, description_panel, backgroundImage;
    [SerializeField] private Text description_display, purchase_display, costsText, oldStat, newStat;
    [SerializeField] private Text[] level_displays, costDisplays, levelDisplays2, costDisplays2;
    [SerializeField] private Slider[] slider_displays;
    [SerializeField] private Button purchase_button;
    [SerializeField] private PlayerHealth health_script;

    [SerializeField] private TwoDPlayerController movement_script;

    public int target_key {get; private set;}

    public int[] max_levels {get; private set;}
    
    private bool can_purchase = false;

    // public bool purchasable {get; private set;}

    private Upgrade target_upgrade;
    
    public static UpgradesManager Instance;
    
    [SerializeField] private UnityEvent onPurchaseAttempt, onBackButton;


    void Awake(){

        if(Instance == null) { Instance = this; }
        else if(Instance != this) { Destroy(this); }
        
        max_levels = UpgradesList.max_levels;

        for(int i = 0; i < Instance.level_displays.Length; i++){
            Instance.level_displays[i].text = Instance.current_levels[i] + "/" + Instance.max_levels[i]; 

            if(levelDisplays2[i] != null ){ levelDisplays2[i].text = Instance.current_levels[i] + "/" + Instance.max_levels[i]; }

            Instance.slider_displays[i].maxValue =Instance.max_levels[i];
            Instance.slider_displays[i].value = 0;
            SetKey(1);
            ChooseUpgrade();
        }
    }

    public void CheckUpgrade(int target, Text text, Button button, Image graphic, Color text_color){
        Upgrade upgrade =  UpgradesList.GetUpgrade(target, Instance);
        bool purchasable = upgrade.max_level > Instance.current_levels[target] ? upgrade.Compare(GameManager.main.ScoreCount, Instance.current_levels[target]) : false;

        // If the selected upgrade is maxed out
        if(Instance.current_levels[target] >= upgrade.max_level){
            button.interactable = false;
            //graphic.color = Instance.error;
            text.text = "This Is MAXED!";
            text.color = text_color;

            return;
        }
        
        // If the selected upgrade can be purchased
        if(purchasable){
            button.interactable = true;
            //graphic.color = Instance.purchasable;
            text.color = Instance.purchasable_text_color;
            //Instance.purchase_display.text = "Costs "+ Instance.target_upgrade.costs[Instance.current_levels[Instance.target_key]].ToString() +" Tokens";
        }
        else{
            button.interactable = false;
            //graphic.color = Instance.error;
            text.color = text_color;
        }

        text.text = "Costs "+ upgrade.costs[Instance.current_levels[target]].ToString() + (upgrade.costs[Instance.current_levels[target]] > 1 ? "Tokens" : "Token");
        
    }

    public void ChooseUpgrade(){

        Instance.target_upgrade = UpgradesList.GetUpgrade(Instance.target_key, Instance);
        // description_panel.color = desc_panel_colors[target_key];
        backgroundImage.color = desc_panel_colors[target_key];
        description_display.text = UpgradesList.descriptions[target_key];



        // If the selected upgrade is maxed out
        if(Instance.current_levels[Instance.target_key] >= Instance.target_upgrade.max_level){
            can_purchase = false;
            purchase_button.interactable = false;
            purchase_button_graphic.color = Instance.error;
            purchase_display.text = "This Is MAXED!";
            return;

        }

        if(GameManager.main != null){
            // Check for if the upgrade is purchasable 
            can_purchase = target_upgrade.Compare(GameManager.main.ScoreCount, Instance.current_levels[Instance.target_key]);
            // Set the description text of the ui

            // If the selected upgrade can be purchased
            if(Instance.can_purchase){
                purchase_button.interactable = true;
                purchase_button_graphic.color = Instance.purchasable;
                purchase_display.text = "Purchase "+ Instance.target_upgrade.name + " " + (Instance.current_levels[target_key]+1).ToString();
            }
            else{
                purchase_button.interactable = false;
                purchase_button_graphic.color = Instance.error;
                purchase_display.text = "You Need "+ Instance.target_upgrade.CostDifference(GameManager.main.ScoreCount, Instance.current_levels[Instance.target_key]).ToString() + 
                    (target_upgrade.CostDifference(GameManager.main.ScoreCount, Instance.current_levels[Instance.target_key]) > 1 ? " More Tokens" : " More Token");
            }
        }
        else{
                purchase_button.interactable = false;
                purchase_button_graphic.color = error;
                purchase_display.text = "Choose an upgrade to PURCHASE";

        }

        for(int i = 0; i < costDisplays.Length; i++){
            try {
                costDisplays[i].text = UpgradesList.GetUpgrade(i, this).costs[current_levels[i]].ToString(); 
                if(costDisplays2[i] != null ){costDisplays2[i].text = UpgradesList.GetUpgrade(i, this).costs[current_levels[i]].ToString(); }
            }
            catch (System.Exception) { 
                costDisplays[i].text = "MAX"; 
                if(costDisplays2[i] != null ){costDisplays2[i].text = "MAX"; }
            }
        }
        
        // oldStat.text = target_upgrade.values[current_levels[target_key]].ToString();
        try{oldStat.text = target_key == 1? (DifficultyManager.main.defaultHealth + ((int) target_upgrade.values[current_levels[target_key]-1] - 250)).ToString() : target_upgrade.values[current_levels[target_key]-1].ToString(); }
        catch{oldStat.text = ""; }

        try{newStat.text = target_key == 1? (DifficultyManager.main.defaultHealth + ((int) target_upgrade.values[current_levels[target_key]] - 250)).ToString() : target_upgrade.values[current_levels[target_key]].ToString(); }
        catch{newStat.text = ""; }

        if( target_upgrade.costs.Length > current_levels[target_key]){
            costsText.text = target_upgrade.costs[current_levels[target_key]].ToString();
        }
    }

    public void BuyUpgrade(){

        if(Instance.can_purchase){

            
            int result = Instance.shop.PurchaseUpgrade(Instance.target_upgrade, Instance.current_levels[Instance.target_key]);
            if(result == 1){
                Instance.current_levels[Instance.target_key]++;
                UpdateStats();
                ChooseUpgrade();
            }
            
        }
        else
        {
            return;
        }
    }

    void UpdateStats(bool reset = false){
        for(int i = 0; i < Instance.level_displays.Length; i++){
            Instance.level_displays[i].text =Instance.current_levels[i] + "/" + Instance.max_levels[i]; 
            if(levelDisplays2[i] != null ){ Instance.levelDisplays2[i].text =Instance.current_levels[i] + "/" + Instance.max_levels[i]; }
            Instance.slider_displays[i].value = Instance.current_levels[i];
        }

        if(reset){
            Player.main.movement.speed = 5;
        }
        else if(Instance.current_levels[0] > 0){
            Player.main.movement.speed = UpgradesList.speed.values[Instance.current_levels[0] - 1];
        }
        Player.main.health.MaxHealthCheck();
        GunHandler.Instance.primary_cooldown.CheckUpgrades(reset);
        GunHandler.Instance.secondary_cooldown.CheckUpgrades(reset);

        if(Instance.current_levels[4] > 0 && !GunHandler.Instance.owns_dual){
            GunHandler.Instance.PurchaseDual();
        }

        ChooseUpgrade();
        // UpdateStats();
    }

    public void ResetUpgrades(){
        for (int i = 0; i < current_levels.Length; i++)
        {
            current_levels[i] = 0;
        }
        UpdateStats(true);
        SetKey(0);
    }

    public Image GetBackground(){return backgroundImage;}

    void Update(){
        if(can_purchase && backgroundImage.gameObject.activeInHierarchy){
            
            if(DetectInputDevice.main.isKBM) {
                purchase_display.text = "Press " + CustomKeybinds.main.Interact.ToString() + " to Purchase "+ Instance.target_upgrade.name + " " + (Instance.current_levels[target_key]+1).ToString();
            }
            else if(DetectInputDevice.main.isController) {
                purchase_display.text = "Press Y to Purchase "+ Instance.target_upgrade.name + " " + (Instance.current_levels[target_key]+1).ToString();
            }

            

            if(CustomKeybinds.main.PressingInteract()){
                // onPurchaseAttempt.Invoke();
                BuyUpgrade();
            }
        }
    }

    public void SetKey(int key){
        Instance.target_key = key;
        KillboxEventSystem.TriggerUpgradeButtonSelectEvent(UpgradesList.GetUpgrade(Instance.target_key, Instance));
        ChooseUpgrade();
    }

    public void OnBackButton(bool pressedThisFrame)
    {
        if(pressedThisFrame && backgroundImage.gameObject.activeInHierarchy){
            GameManager.main.SetInGameButtonHandlers(true);
            onBackButton.Invoke();
            KillboxEventSystem.TriggeCloseShopEvent();
        }
    }

    public void SetSecondaryDisplays(Text[] lvl, Text[] costs){
        levelDisplays2 = lvl;
        costDisplays2 = costs;
    }
}
