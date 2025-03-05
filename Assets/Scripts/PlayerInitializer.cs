using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInitializer : MonoBehaviour
{
    private Player self;
    [SerializeField] private SpriteRenderer pWeapon, dWeapon;

    void Awake(){
        self = new Player(gameObject, gameObject.GetComponent<PlayerHealth>(), pWeapon, dWeapon);
    }

    void Update(){
        if (Player.main == null || Player.main.tf == null)
        {
            self = new Player(gameObject, gameObject.GetComponent<PlayerHealth>(), pWeapon, dWeapon);
        }
    }
}
