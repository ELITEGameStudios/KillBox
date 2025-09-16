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
    [SerializeField] private Color[] desc_panel_colors, upgradeColors;
    [SerializeField] private Image purchase_button_graphic, description_panel, backgroundImage;
    [SerializeField] private Text upgradeNameDisplay, purchase_display, costsText;
    [SerializeField] private Text[] level_displays, costDisplays, levelDisplays2, costDisplays2, statNameDisplays, statValueDisplays;
    [SerializeField] private Slider[] slider_displays;
    [SerializeField] private Button purchase_button;
    [SerializeField] private PlayerHealth health_script;

    [SerializeField] private TwoDPlayerController movement_script;
    [SerializeField] private SpecialUpgradeButton[] specialUpgradeButtons;

    public int target_key {get; private set;}

    public int[] max_levels {get; private set;}
    
    private bool can_purchase = false, activeMenu = false;
    public bool[] isPurchasable;

    // public bool purchasable {get; private set;}

    private Upgrade target_upgrade;
    
    public static UpgradesManager Instance;
    
    [SerializeField] private UnityEvent onPurchaseAttempt, onBackButton;


    void Awake(){

        if(Instance == null) { Instance = this; }
        else if(Instance != this) { Destroy(this); }
        isPurchasable = new bool[5];
        max_levels = UpgradesList.max_levels;

        for(int i = 0; i < level_displays.Length; i++){
            level_displays[i].text = current_levels[i] + "/" + max_levels[i]; 

            if(levelDisplays2[i] != null ){ levelDisplays2[i].text = current_levels[i] + "/" + max_levels[i]; }

            slider_displays[i].maxValue =max_levels[i];
            slider_displays[i].value = 0;
            SetKey(1);
            ChooseUpgrade();
        }
    }

    public void OpenMenu()
    {
        backgroundImage.gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Open");
        CancelInvoke(nameof(CloseMenuInvoke));
        activeMenu = true;
        Player.main.movement.OnOpenShop(0);
    }

    public void CloseMenuInvoke()
    {
        backgroundImage.gameObject.SetActive(false);
    }

    public void CheckUpgrade(int target, Text text, Button button, Image graphic, Color text_color)
    {
        Upgrade upgrade = UpgradesList.GetUpgrade(target, Instance);
        bool purchasable = upgrade.max_level > current_levels[target] ? upgrade.Compare(GameManager.main.ScoreCount, current_levels[target]) : false;

        // If the selected upgrade is maxed out
        if (current_levels[target] >= upgrade.max_level)
        {
            button.interactable = false;
            //graphic.color = error;
            text.text = "This Is MAXED!";
            // text.color = text_color;
            isPurchasable[target] = false;

            return;
        }

        isPurchasable[target] = purchasable;
        button.interactable = true;

        // If the selected upgrade can be purchased
        if (purchasable)
        {
            //graphic.color = purchasable;
            // text.color = purchasable_text_color;
            //purchase_display.text = "Costs "+ target_upgrade.costs[current_levels[target_key]].ToString() +" Tokens";
        }
        else
        {
            // button.interactable = false;
            //graphic.color = error;
            text.color = text_color;
        }

        // text.text = "Costs "+ upgrade.costs[current_levels[target]].ToString() + (upgrade.costs[current_levels[target]] > 1 ? "Tokens" : "Token");

    }

    public void ChooseUpgrade(){

        target_upgrade = UpgradesList.GetUpgrade(target_key, Instance);
        // description_panel.color = desc_panel_colors[target_key];
        backgroundImage.color = desc_panel_colors[target_key];

        if (target_upgrade != null) {
            upgradeNameDisplay.text = target_upgrade.name;
            upgradeNameDisplay.color = upgradeColors[target_key];
        }
        else{
            // upgradeNameDisplay.text = target_upgrade.name;
            upgradeNameDisplay.text = "";
        }

        if (LoopyScript.upgrades != null)
        {

            LoopyScript.upgrades.AddState(
                new LoopyState(
                    LoopyPose.NEUTRAL,
                    UpgradesList.descriptions[target_key],
                    0.1f
                ),
                priority: true
            );
        }
            



        // If the selected upgrade is maxed out
            if (current_levels[target_key] >= target_upgrade.max_level) {
                can_purchase = false;
                purchase_button.interactable = false;
                purchase_button_graphic.color = error;
                purchase_display.text = "This Is MAXED!";
                return;

            }

        if(GameManager.main != null){
            // Check for if the upgrade is purchasable 
            can_purchase = target_upgrade.Compare(GameManager.main.ScoreCount, current_levels[target_key]);
            // Set the description text of the ui

            // If the selected upgrade can be purchased
            if(can_purchase){
                purchase_button.interactable = true;
                purchase_button_graphic.color = purchasable;
                purchase_display.text = "Purchase "+ target_upgrade.name + " " + (current_levels[target_key]+1).ToString();
            }
            else{
                purchase_button.interactable = false;
                purchase_button_graphic.color = error;
                purchase_display.text = "You Need "+ target_upgrade.CostDifference(GameManager.main.ScoreCount, current_levels[target_key]).ToString() + 
                    (target_upgrade.CostDifference(GameManager.main.ScoreCount, current_levels[target_key]) > 1 ? " More Tokens" : " More Token");
            }
        }
        else{
                purchase_button.interactable = false;
                purchase_button_graphic.color = error;
                purchase_display.text = "Choose an upgrade to PURCHASE";

        }
        costsText.text = target_upgrade.costs[current_levels[target_key]].ToString();

        for (int i = 0; i < costDisplays.Length; i++)
        {
            try
            {
                costDisplays[i].text = UpgradesList.GetUpgrade(i, this).costs[current_levels[i]].ToString();
                if (costDisplays2[i] != null) { costDisplays2[i].text = UpgradesList.GetUpgrade(i, this).costs[current_levels[i]].ToString(); }
            }
            catch (System.Exception)
            {
                costDisplays[i].text = "MAX";
                if (costDisplays2[i] != null) { costDisplays2[i].text = "MAX"; }
            }
        }

        // oldStat.text = target_upgrade.values[current_levels[target_key]].ToString();
        for (int i = 0; i < statValueDisplays.Length; i++)
        {
            if (target_upgrade.stat_names.Length <= i)
            {
                statNameDisplays[i].text = ""; statValueDisplays[i].text = "";
                continue;
            }

            statNameDisplays[i].color = upgradeColors[target_key];
            statNameDisplays[i].text = target_upgrade.stat_names[i];
            try
            {
                statValueDisplays[i].text = target_key == 1 ?
                    (DifficultyManager.main.defaultHealth + ((int)target_upgrade.values[0][current_levels[target_key] - 1] - 250)).ToString() :
                    target_upgrade.values[i][current_levels[target_key]-1].ToString() + " -> " + target_upgrade.values[i][current_levels[target_key]].ToString();
            }
            catch { statValueDisplays[i].text = target_upgrade.defaultValues[i][KillBox.currentGame.difficultyIndex].ToString() + " -> " + target_upgrade.values[i][0]; } // if the player has not purchased this upgrade
        }

        if (target_upgrade.costs.Length > current_levels[target_key])
        {
            // costsText.text = target_upgrade.costs[current_levels[target_key]].ToString();
        }
    }

    public void BuyUpgrade(){

        if(can_purchase){

            
            int result = shop.PurchaseUpgrade(target_upgrade, current_levels[target_key]);
            if(result == 1){
                current_levels[target_key]++;
                UpdateStats();
                ChooseUpgrade();
            }
            
        }
        else
        {
            return;
        }
    }
    
    public void FreeUpgrade(int id)
    {
        if(current_levels[id] == max_levels[id]){ Debug.LogAssertion("Player has already maxed this stat"); return; }
        current_levels[id]++;
        SetKey(id);
        UpdateStats();
        ChooseUpgrade();
    }


    void UpdateStats(bool reset = false){
        for(int i = 0; i < level_displays.Length; i++){
            level_displays[i].text =current_levels[i] + "/" + max_levels[i]; 
            if(levelDisplays2[i] != null ){ levelDisplays2[i].text =current_levels[i] + "/" + max_levels[i]; }
            slider_displays[i].value = current_levels[i];
        }

        if (reset)
        {
            Player.main.movement.speed = 5;
        }
        else if (current_levels[0] > 0)
        {
            Player.main.movement.speed = UpgradesList.speed.values[0][current_levels[0] - 1];
            Player.main.movement.SetDashCooldown(UpgradesList.speed.values[1][current_levels[0] - 1]);
            GameplayUI.instance.GetDashUI().UpdateDisplay(UpgradesList.speed.values[1][current_levels[0] - 1]);
        }
        Player.main.health.MaxHealthCheck();
        GunHandler.Instance.primary_cooldown.CheckUpgrades(reset);
        GunHandler.Instance.secondary_cooldown.CheckUpgrades(reset);

        if(current_levels[4] > 0 && !GunHandler.Instance.owns_dual){
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

    void Update()
    {
        if (can_purchase && backgroundImage.gameObject.activeInHierarchy)
        {

            if (DetectInputDevice.main.isKBM)
            {
                purchase_display.text = "Press " + CustomKeybinds.main.Interact.ToString() + " to Purchase " + target_upgrade.name + " " + (current_levels[target_key] + 1).ToString();
            }
            else if (DetectInputDevice.main.isController)
            {
                purchase_display.text = "Press Y to Purchase " + target_upgrade.name + " " + (current_levels[target_key] + 1).ToString();
            }



            if (CustomKeybinds.main.PressingInteract() && activeMenu)
            {
                // onPurchaseAttempt.Invoke();
                BuyUpgrade();
            }
        }
        
        
        foreach (SpecialUpgradeButton button in specialUpgradeButtons)
        {
            if (Player.main.specialUpgrade == button.targetUpgrade)
            {
                if (!button.unlocked) { button.SetUnlocked(); }
                continue;
            }
            else
            {
                if (button.unlocked) { button.SetLocked(); }
            }
        }
    }

    public void SetKey(int key){
        target_key = key;
        KillboxEventSystem.TriggerUpgradeButtonSelectEvent(UpgradesList.GetUpgrade(target_key, Instance));
        ChooseUpgrade();
    }

    public void OnBackButton(bool pressedThisFrame)
    {
        if (pressedThisFrame && backgroundImage.gameObject.activeInHierarchy)
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

    public void SetSecondaryDisplays(Text[] lvl, Text[] costs){
        levelDisplays2 = lvl;
        costDisplays2 = costs;
    }
}
