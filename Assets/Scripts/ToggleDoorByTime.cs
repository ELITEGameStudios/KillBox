using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDoorByTime : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    //[SerializeField] private bool opens_pre_round;
    [SerializeField] private float time, current_time;
    [SerializeField] private int repititions, target_repititions;
    [SerializeField] private Door door;
    [SerializeField] private Text target_ui;

    void Awake(){
        ChangeUiColor();
    }

    void Update(){
        
        if(LvlStarter.main.HasStarted && Application.isFocused){
            CheckTime();
            current_time -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    public void CheckTime()
    {
        if(repititions != 0){

            if(current_time <= 0){
                door.Toggle();
                repititions -= 1;
                current_time = time;
                ChangeUiColor();

            }

        }
        
        if(repititions < 0 || repititions > 0){
            target_ui.text = ((int)current_time+1).ToString();
        }
        else{
            target_ui.text = "∞";
        }
    }

    public void Reset(){
        ChangeUiColor();
        current_time = time;
        repititions = target_repititions;

    }

    public void ChangeUiColor(){
        if(door.IsOpen){
            target_ui.color = Color.green;
        }
        else{
            target_ui.color = Color.black;
        }
    }

}
