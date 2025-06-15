using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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

    [SerializeField] private SpriteRenderer primaryGunGraphic, dualGunGraphic, playerSprite;
    [SerializeField] private ParticleSystem appearParticleAffect;
    public SpriteRenderer PlayerSprite {get {return playerSprite; }}
    public SpriteRenderer PrimaryGunGraphic {get {return primaryGunGraphic; }}
    public SpriteRenderer DualGunGraphic {get {return dualGunGraphic; }}

    public Player(GameObject _obj, PlayerHealth _health, SpriteRenderer primary, SpriteRenderer secondary, SpriteRenderer playerSprite, ParticleSystem appearEffect){
        obj = _obj;
        health = _health;
        kills = 0;
        kills_in_round = 0;
        tf = obj.transform;
        rb = obj.GetComponent<Rigidbody2D>();
        movement = obj.GetComponent<TwoDPlayerController>();

        primaryGunGraphic = primary;
        dualGunGraphic = secondary;
        appearParticleAffect = appearEffect;
        this.playerSprite = playerSprite;

        if (player == null || main == null || main.tf == null){
            player = this;
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
        tf.GetChild(0).GetComponent<Light2D>().intensity = lightIntensity;
        appearParticleAffect.Play();
    }

    public static Player main
    {
        get { return player; }
        set { }
    } 

}
