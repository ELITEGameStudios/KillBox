using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDoorByToggle : MonoBehaviour
{ 

    [SerializeField] private GameObject UI;
    //[SerializeField] private bool opens_pre_round;
    [SerializeField] private int channel;
    [SerializeField] private bool openChannelState;
    [SerializeField] private Door door;
    [SerializeField] private Text target_ui;

    void Awake(){
        ChangeUiColor();
    }

    void Update(){
        
        if(Application.isFocused){
            CheckChannel();
            // current_time -= Time.deltaTime;
        }

        target_ui.text = channel.ToString();
    }

    // Update is called once per frame
    public void CheckChannel()
    {
        if(door.RoundEnded && (door.OpensOnRoundEnd || door.ClosesOnRoundEnd) )
        { return; }
        
        if(ToggleChannelManager.main.GetChannel(channel) == openChannelState){
            if(!door.IsOpen){
                OpenDoor();

            }
        }
        else if(door.IsOpen){
            door.Close();
            ChangeUiColor();
        }
    }

    public void OpenDoor(){
        door.Open();
        ChangeUiColor();
    }

    public void Reset(){
        CheckChannel();
        ChangeUiColor();
        // repititions = target_repititions;

    }

    public void ChangeUiColor(){
        if(door.IsOpen){
            target_ui.color = Color.cyan;
        }
        else{
            target_ui.color = Color.black;
        }
    }

}
