using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HotshotEquipManager : MonoBehaviour
{
    public bool has_hotshot, startEffect, endEffect;

    [SerializeField]
    private float UltraTime, start_effect_clock, end_effect_clock;

    public GameObject hotshot_grenade, activate_button;

    [SerializeField]
    private GameManager manager;

    [SerializeField] private Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        has_hotshot = false;
    }

    void Update(){
        if(DetectInputDevice.main.isController){ buttonText.text = "PRESS X TO USE EQUIPMENT"; }
        else if(DetectInputDevice.main.isKBM){ buttonText.text = "PRESS "+CustomKeybinds.main.Ultramode.ToString()+" TO USE EQUIPMENT"; }
        
        if(has_hotshot){
            manager.equipment_slider.value -= Time.deltaTime;
        }
    }

    public void GamemodeStart()
    {
        has_hotshot = true;
        GameObject clone = Instantiate(hotshot_grenade, transform);
        clone.transform.SetParent(null);
        StartCoroutine(Lifespan());
        activate_button.GetComponent<Animator>().Play("equipment_button_exit");
        StartCoroutine(ButtonDeactivate());
        manager.UpdateReqEquipmentKills();
    }

    IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(UltraTime);
        has_hotshot = false;
        manager.player_has_hotshot = false;
        manager.ResetUltraKills(0);

    }
    public void DeactivateButton(){
        activate_button.SetActive(false);
        if(has_hotshot){ 
            endEffect = true; 
            startEffect = false;
            has_hotshot = false;
        }
    }
    IEnumerator ButtonDeactivate(){
        activate_button.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        activate_button.SetActive(false);
    }
}
