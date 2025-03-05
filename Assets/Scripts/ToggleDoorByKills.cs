using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoorByKills : MonoBehaviour
{

    [SerializeField] private Door door;
    [SerializeField] private int target;

    [SerializeField] private bool toggled = false;
    [SerializeField] private bool destroy_on_toggle;

    // Update is called once per frame
    void Update()
    {
        if(Player.main.kills_in_round >= target && !toggled){
            door.Toggle();
            toggled = true;
            //if(destroy_on_toggle){ Destroy(this); }
        }

        Debug.Log(Player.main.kills_in_round);
    }

    public void Reset(){
        toggled = false;
    }
}
