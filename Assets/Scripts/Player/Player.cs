using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private SpriteRenderer primaryGunGraphic, dualGunGraphic;
    public SpriteRenderer PrimaryGunGraphic {get {return primaryGunGraphic; }}
    public SpriteRenderer DualGunGraphic {get {return dualGunGraphic; }}

    public Player(GameObject _obj, PlayerHealth _health, SpriteRenderer primary, SpriteRenderer secondary){
        obj = _obj;
        health = _health;
        kills = 0;
        kills_in_round = 0;
        tf = obj.transform;
        rb = obj.GetComponent<Rigidbody2D>();
        movement = obj.GetComponent<TwoDPlayerController>();

        primaryGunGraphic = primary;
        dualGunGraphic = secondary;

        if (player == null || main == null || main.tf == null){
            player = this;
        }
    }

    private static Player player;

    public void AddKill(){
        kills++;
        kills_in_round++;
    }

    public void NewRound(){
        kills_in_round = 0;
    }

    public static Player main {
        get {return player;}
        set{}
    } 

}
