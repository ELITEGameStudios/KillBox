using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static BossRoundManager;
using static UpgradesList;

public class Player
{
    public GameObject obj {get; private set;} 
    public TwoDPlayerController movement {get; private set;} 
    public Transform tf {get; private set;} 
    public Rigidbody2D rb {get; private set;} 
    public PlayerHealth health {get; private set;}
    public int kills {get; private set;}
    public int kills_in_round {get; private set;}
    public float lightIntensity;
    public SpecialUpgradeEnum specialUpgradeEnum;
    public SpecialUpgrade currentSpecialUpgrade;
    public SpecialUpgrade[] specialUpgrades;


    [SerializeField] private SpriteRenderer primaryGunGraphic, dualGunGraphic, playerSprite;
    [SerializeField] private ParticleSystem appearParticleAffect;
    public SpriteRenderer PlayerSprite {get {return playerSprite; }}
    public SpriteRenderer PrimaryGunGraphic {get {return primaryGunGraphic; }}
    public SpriteRenderer DualGunGraphic {get {return dualGunGraphic; }}
    public List<BossType> defeatedBossesList {get; private set; }

    public Player(GameObject _obj, PlayerHealth _health, SpriteRenderer primary, SpriteRenderer secondary, SpriteRenderer playerSprite, ParticleSystem appearEffect){
        obj = _obj;
        health = _health;
        kills = 0;
        kills_in_round = 0;
        tf = obj.transform;
        rb = obj.GetComponent<Rigidbody2D>();
        movement = obj.GetComponent<TwoDPlayerController>();
        specialUpgradeEnum = SpecialUpgradeEnum.NONE;

        primaryGunGraphic = primary;
        dualGunGraphic = secondary;
        appearParticleAffect = appearEffect;
        this.playerSprite = playerSprite;

        if (player == null || main == null || main.tf == null){
            player = this;
        }

        specialUpgrades = new SpecialUpgrade[4]
        {
            obj.AddComponent<DualWieldSpecial>(),
            obj.AddComponent<GoldWeaponSpecial>(),
            obj.AddComponent<NecroSpecial>(),
            obj.AddComponent<UpgradesSpecial>(),
        };

        defeatedBossesList = new();
    }

    public void AddDefeatedBoss(BossType bossType)
    {
        defeatedBossesList.Add(bossType);
    }

    public void AddSpecialUpgrade(SpecialUpgradeEnum upgrade)
    {
        if(currentSpecialUpgrade != null){ 
            currentSpecialUpgrade.OnDeactivate();
            GameplayUI.instance.GetSpecialUpgradeButtons()[(int)specialUpgradeEnum-1].SetLocked(); 
        }

        if(upgrade == SpecialUpgradeEnum.NONE)
        {
            currentSpecialUpgrade = null; 
        }
        else
        {
            currentSpecialUpgrade = specialUpgrades[(int)upgrade - 1];
            currentSpecialUpgrade.OnInit();
        }
        
        specialUpgradeEnum = upgrade;
        
        if(specialUpgradeEnum != SpecialUpgradeEnum.NONE){
            GameplayUI.instance.GetSpecialUpgradeButtons()[(int)specialUpgradeEnum-1].SetUnlocked(); 
        }
    }

    private static Player player;

    public void AddKill()
    {
        kills++;
        kills_in_round++;
        EquipmentManager.instance.AddEquipmentKill();
    }

    public void NewRound(){
        kills_in_round = 0;
        health.ResetHealth();
    }

    public void SetSpecialUpgrade(SpecialUpgradeEnum specialUpgrade){
        this.specialUpgradeEnum = specialUpgrade;
    }

    public void Dissapear()
    {
        playerSprite.enabled = false;
        primaryGunGraphic.enabled = false;
        dualGunGraphic.enabled = false;
        lightIntensity = tf.GetChild(0).GetComponent<Light2D>().intensity;
        tf.GetChild(0).GetComponent<Light2D>().intensity = 0;
    }

    public void Appear()
    {
        playerSprite.enabled = true;
        primaryGunGraphic.enabled = true;
        dualGunGraphic.enabled = true;

        PulseEffectManager.instance.AddEffect(tf.position, expandRate: 1f, strength:-0.005f);
        tf.GetChild(0).GetComponent<Light2D>().intensity = lightIntensity;
        appearParticleAffect.Play();
    }

    public static Player main
    {
        get { return player; }
        set { }
    } 

}
