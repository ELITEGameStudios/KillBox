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
    public Text GetEnemiesLeftText(){return enemiesLeftText;}

    [Header("Health")]
    [SerializeField] private Text healthText;
    [SerializeField] private Slider healthSlider;
    public Slider GetHealthSlider(){return healthSlider;}
    public Text GetHealthText(){return healthText;}
    
    
    [Header("Cooldown graphics")]
    [SerializeField] private Image mainBarSliderImage;
    [SerializeField] private Image secondaryBarSliderImage;
    [SerializeField] private Slider cooldownSlider, primaryCooldownSlider, secondaryCooldownSlider;
    [SerializeField] private Text cooldownText;

    public Slider GetPrimaryCooldownSlider(){return primaryCooldownSlider;}
    public Slider GetSecondaryCooldownSlider(){return secondaryCooldownSlider;}
    public Image GetPrimaryCooldownSliderImage(){return mainBarSliderImage;}
    public Image GetSecondaryCooldownSliderImage(){return secondaryBarSliderImage;}


    [Header("Equipment graphics")]
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Slider equipmentSlider;
    [SerializeField] private Text equipmentHeader;

    public Text GetEquipmentHeader(){return equipmentHeader;}
    public Image GetEquipmentImage(){return secondaryBarSliderImage;}
    public Slider GetEquipmentSlider(){return cooldownSlider;}

    
    [Header("Weapon buttons")]
    [SerializeField] private Button primaryWeaponButton;
    [SerializeField] private Button backupWeaponButton;
    [SerializeField] private GameObject[] weapon_ui_overlays;

    public Button GetPrimaryWeaponButton(){return primaryWeaponButton;}
    public Button GetBackupWeaponButton(){return backupWeaponButton;}
    
    [Header("Dash indicator")]
    [SerializeField] private Text dashTimerText;
    [SerializeField] private Image dashIndicator;
    [SerializeField] private Color canDashColor, cannotDashColor;

    [Header("Weapon and Stat Upgrade Tabs")]
    [SerializeField] private TabSystemMaster upgradeTabs;
    public TabSystemMaster GetUpgradeTabMaster(){return upgradeTabs;}


    [Header("Death UI")]
    [SerializeField] private GameObject selfResButton;
    public GameObject GetSelfResButton(){return selfResButton;}


    [Header("Round Display UI")]
    [SerializeField] private Animator lvlDisplayAnimator;
    public Animator GetLevelDisplayAnimator(){return lvlDisplayAnimator;}

    public static GameplayUI instance {get; private set;}

    // Start is called before the first frame update
    void Awake(){
        if(instance == null) { instance = this; }
        else if(instance != this) { Destroy(this); }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.main != null){

            if(KillBox.currentGame.started){
                SetAllText(roundKeepers, KillBox.currentGame.round.ToString());
                SetAllText(scorekeepers, GameManager.main.ScoreCount.ToString());
                SetAllText(pbKeepers, KillBox.main.PBInt.ToString());
                
                if(Player.main.health != null){
                    healthSlider.value = Player.main.health.CurrentHealth;
                    healthText.text = Player.main.health.CurrentHealth.ToString();
                }
                
                enemiesLeftText.text = EnemyCounter.main.enemiesInScene.ToString();

                if(Player.main.movement.canDash){
                    dashIndicator.color = canDashColor;
                    dashTimerText.text = "";
                }
                else{
                    dashIndicator.color = cannotDashColor;
                    dashTimerText.text = ((int)(Player.main.movement.GetDashCooldownTimer()+1)).ToString();
                }
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

        for(int i = 0; i < InventoryUIManager.Instance.main_buttons.Count; i++){
            InventoryUIManager.Instance.main_buttons[i].EquipDisplay();
        }
    }

    public void Initialize(){
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
