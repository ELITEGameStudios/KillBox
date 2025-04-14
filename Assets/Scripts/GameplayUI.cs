using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("Common texts")]
    [SerializeField] private Text[] scorekeepers;
    [SerializeField] private Text[] roundKeepers, pbKeepers;
    [SerializeField] private Text healthText, enemiesLeftText;

    [Header("Sliders")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider cooldownSlider, primaryCooldownSlider, secondaryCooldownSlider;
    
    [Header("Weapon buttons")]
    [SerializeField] private Button primaryWeaponButton;
    [SerializeField] private Button backupWeaponButton;
    
    [Header("Dash indicator")]
    [SerializeField] private Text dashTimerText;
    [SerializeField] private Image dashIndicator;
    [SerializeField] private Color canDashColor, cannotDashColor;

    public Slider GetCooldownSlider(){return cooldownSlider;}
    public Slider GetPrimaryCooldownSlider(){return primaryCooldownSlider;}
    public Slider GetSecondaryCooldownSlider(){return secondaryCooldownSlider;}
    public Button GetPrimaryWeaponButton(){return primaryWeaponButton;}
    public Button GetBackupWeaponButton(){return backupWeaponButton;}
    public Text GetEnemiesLeftText(){return enemiesLeftText;}

    public static GameplayUI instance {get; private set;}



    // Start is called before the first frame update
    void Awake(){
        if(instance == null) { instance = this; }
        else if(instance != this) { Destroy(this); }
    }

    // Update is called once per frame
    void Update()
    {
        if(KillBox.currentGame.started){
            SetAllText(roundKeepers, KillBox.currentGame.round.ToString());
            SetAllText(scorekeepers, GameManager.main.ScoreCount.ToString());
            SetAllText(pbKeepers, KillBox.main.PBInt.ToString());
            
            healthSlider.value = Player.main.health.CurrentHealth;
            healthText.text = Player.main.health.CurrentHealth.ToString();
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

    void SetAllText(Text[] texts, string message){
        foreach (Text text in texts){
            text.text = message;
        }
    }
}
