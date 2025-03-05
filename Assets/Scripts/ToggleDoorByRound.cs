using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDoorByRound : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private bool opens_pre_round;
    [SerializeField] private int target;
    [SerializeField] private Door door;
    [SerializeField] private Text target_ui;

    void Awake(){
        target_ui.text = target.ToString();
    }

    void Update(){
        CheckRound();
    }

    // Update is called once per frame
    public void CheckRound()
    {
        if(!LvlStarter.main.HasStarted && door.OpenByDefault){
            door.Open();
            UI.SetActive(false);
            return;
        }
        
        if(GameManager.main.LvlCount >= target && !door.IsOpen){
            if(LvlStarter.main.HasStarted){
                door.Open();
                UI.SetActive(false);
            }
        }
    }

    public void Reset(){
        UI.SetActive(!door.IsOpen);
    }

}
