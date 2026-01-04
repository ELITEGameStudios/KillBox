using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("Common texts")]
    [SerializeField] private Text[] scorekeepers;
    [SerializeField] private Text[] roundKeepers, pbKeepers;
    [SerializeField] private Text enemiesLeftText;
    [SerializeField] private Text phaseText;
    public Text GetEnemiesLeftText(){return enemiesLeftText;}
    public void SetPhaseText(int phase = 0){ phaseText.text = "PHASE  " + (phase == 0 ? GameManager.main.GetPhase().ToString() : phase.ToString()); }

    [Header("Health")]
    [SerializeField] private Text healthText, shieldText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Animator healthAnimator;
    public Slider GetHealthSlider(){return healthSlider;}
    public Text GetHealthText(){return healthText;}
    public Animator GetHealthAnimator(){return healthAnimator;}
    
    
    [Header("Cooldown graphics")]
    [SerializeField] private Image mainBarSliderImage;
    [SerializeField] private Image secondaryBarSliderImage;
    [SerializeField] private Slider primaryCooldownSlider, secondaryCooldownSlider;
    [SerializeField] private Text cooldownText;

    public Slider GetPrimaryCooldownSlider(){return primaryCooldownSlider;}
    public Slider GetSecondaryCooldownSlider(){return secondaryCooldownSlider;}
    public Image GetPrimaryCooldownSliderImage(){return mainBarSliderImage;}
    public Image GetSecondaryCooldownSliderImage(){return secondaryBarSliderImage;}


    [Header("Equipment graphics")]
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Slider equipmentSlider;
    [SerializeField] private Text equipmentHeader;
    [SerializeField] private Animator equipmentAnimator;

    public Text GetEquipmentHeader(){return equipmentHeader;}
    public Image GetEquipmentImage(){return secondaryBarSliderImage;}
    public Slider GetEquipmentSlider(){return equipmentSlider;}
    public Animator GetEquipmentAnimator(){return equipmentAnimator;}

    
    [Header("Weapon buttons")]
    [SerializeField] private Button primaryWeaponButton;
    [SerializeField] private Button backupWeaponButton;
    [SerializeField] private GameObject[] weapon_ui_overlays;
    [SerializeField] private Animator primaryAnimator;
    [SerializeField] private Animator secondaryAnimator;
    [SerializeField] private Animator dualAnimator;

    public Button GetPrimaryWeaponButton(){return primaryWeaponButton;}
    public Button GetBackupWeaponButton(){return backupWeaponButton;}
    public Animator GetPrimaryAnimator(){return primaryAnimator;}
    public Animator GetSecondaryAnimator(){return secondaryAnimator;}
    public Animator GetDualAnimator(){return dualAnimator;}
    
    [Header("Dash indicator")]
    // [SerializeField] private Text dashTimerText;
    // [SerializeField] private Image dashIndicator;
    // [SerializeField] private Color canDashColor, cannotDashColor;
    [SerializeField] private DashElementScript dashElement;
    public DashElementScript GetDashUI(){return dashElement;}


    [Header("Death UI")]
    [SerializeField] private GameObject selfResButton;
    public GameObject GetSelfResButton(){return selfResButton;}


    [Header("Round Display UI")]
    [SerializeField] private Animator lvlDisplayAnimator;
    public Animator GetLevelDisplayAnimator(){return lvlDisplayAnimator;}

    [Header("Pause button")]
    [SerializeField] private GameObject pauseButtonObject;
    public GameObject GetPauseButton(){return pauseButtonObject;}

    [Header("Progress bar element")]
    [SerializeField] private GameObject progressBarObject;
    public GameObject GetProgressBarObject(){ return progressBarObject; }

    [Header("Shop Menus")]
    [SerializeField] private UpgradesManager upgradesManager;
    [SerializeField] private InventoryUIManager inventoryUIManager;
    [SerializeField] private Text pauseMenuShortcutSignifier;
    [SerializeField] private SpecialUpgradeButton[] specialUpgradeDisplays;
    public SpecialUpgradeButton[] GetSpecialUpgradeButtons() { return specialUpgradeDisplays; }
    public bool anyShopMenuOpen{  get {
        return upgradesManager.GetBackground().gameObject.activeInHierarchy ||
               inventoryUIManager.GetBackground().gameObject.activeInHierarchy; } }

    public static GameplayUI instance { get; private set; }

    // Start is called before the first frame update
    void Awake(){
        if(instance == null) { instance = this; }
        else if(instance != this) { Destroy(this); }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.main != null){

            if (KillBox.currentGame.started)
            {
                SetAllText(roundKeepers, KillBox.currentGame.round.ToString());
                SetAllText(scorekeepers, GameManager.main.ScoreCount.ToString());
                SetAllText(pbKeepers, KillBox.main.PBInt.ToString());
                SetPhaseText();

                if (Player.main.health != null)
                {
                    healthSlider.value = Player.main.health.CurrentHealth;
                    healthText.text = Player.main.health.CurrentHealth.ToString();
                    shieldText.text = "| + " + Player.main.health.currentShieldHP.ToString();
                    healthAnimator.SetBool("Shield", Player.main.health.hasShield && Player.main.health.currentShieldHP > 0);
                }

                enemiesLeftText.text = EnemyCounter.main.enemiesInScene.ToString();
                pauseMenuShortcutSignifier.text = CustomKeybinds.main.GetKeybindString(CustomKeybinds.main.Pause);


                // Legacy Dash Code
                // if(Player.main.movement.canDash){
                //     dashIndicator.color = canDashColor;
                //     dashTimerText.text = "";
                // }
                // else{
                //     dashIndicator.color = cannotDashColor;
                //     dashTimerText.text = ((int)(Player.main.movement.GetDashCooldownTimer()+1)).ToString();
                // }
            }
            
        }
    }

    public void WeaponsUIRefresh(GunHandler handler){
        if(handler.current_is_primary){
            weapon_ui_overlays[0].SetActive(false);
            weapon_ui_overlays[1].SetActive(true);
        }

        else{
            weapon_ui_overlays[0].SetActive(true);
            weapon_ui_overlays[1].SetActive(false);
        }

        if(handler.owns_dual){
            weapon_ui_overlays[2].SetActive(false);
        }

        primaryAnimator.SetBool("Active", handler.current_is_primary);
        secondaryAnimator.SetBool("Active", !handler.current_is_primary);
        dualAnimator.SetBool("Active", handler.owns_dual);

        primaryAnimator.GetComponent<InventoryUIElement>().EquipDisplay();
        secondaryAnimator.GetComponent<InventoryUIElement>().EquipDisplay();
        dualAnimator.GetComponent<InventoryUIElement>().EquipDisplay();

        for (int i = 0; i < InventoryUIManager.Instance.main_buttons.Count; i++)
        {
            InventoryUIManager.Instance.main_buttons[i].EquipDisplay();
        }
    }

    public void PauseGame(bool pause)
    {
        GameManager.main.pauseHandler.PausePlay(pause ? 0 : 1);
        MainMenuManager.instance.OpenMenuViaState(pause ? MainMenuManager.MenuState.PAUSED : MainMenuManager.MenuState.NONE, doCoroutine: false, immediate: true);
        pauseButtonObject.SetActive(!GameManager.main.pauseHandler.paused);
    }
    
    public void Initialize()
    {
        // InventoryUIManager.Instance.InitializeUI();
        // if(QualityControl.main.ShadowIndex == 0) {QualityControl.main.ShadowToggle.isOn = false;}
        // else {QualityControl.main.ShadowToggle.isOn = true;}
    }

    void SetAllText(Text[] texts, string message){
        foreach (Text text in texts){
            text.text = message;
        }
    }
}
