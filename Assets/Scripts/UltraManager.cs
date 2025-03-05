using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UltraManager : MonoBehaviour
{
    public bool IsUltra, startEffect, endEffect;

    [SerializeField]
    private float UltraTime, start_effect_clock, end_effect_clock;

    public GameObject CamEffectObj, activate_button;

    [SerializeField]
    private GameManager manager;
    [SerializeField] private Text buttonText;
    
    // Start is called before the first frame update
    void Start()
    {
        IsUltra = false;
        CamEffectObj.SetActive(false);
    }

    void Update(){

        if(DetectInputDevice.main.isController){ buttonText.text = "PRESS X TO USE EQUIPMENT"; }
        else if(DetectInputDevice.main.isKBM){ buttonText.text = "PRESS "+CustomKeybinds.main.Ultramode.ToString()+" TO USE EQUIPMENT"; }
        
        //startAnim
        if(startEffect){
            start_effect_clock = 0f;
            startEffect = false;
        }

        if(start_effect_clock < 1){
            start_effect_clock += Time.deltaTime * 2;
            CamEffectObj.GetComponent<Volume>().weight = start_effect_clock;
        }
        else{
            if(start_effect_clock > 1){
                start_effect_clock = 1;
                CamEffectObj.GetComponent<Volume>().weight = start_effect_clock;
            }
        }

        //endAnim
        if(endEffect){
            end_effect_clock = 1f;
            endEffect = false;
        }

        if(end_effect_clock > 0){
            end_effect_clock -= Time.deltaTime * 2;
            CamEffectObj.GetComponent<Volume>().weight = end_effect_clock;
        }
        else{
            if(end_effect_clock < 0){
                end_effect_clock = 0;
                CamEffectObj.GetComponent<Volume>().weight = end_effect_clock;
            }
        }

        if(IsUltra){
            manager.equipment_slider.value -= Time.deltaTime;
        }
    }

    public void GamemodeStart()
    {
        IsUltra = true;
        CamEffectObj.SetActive(true);
        startEffect = true;
        StartCoroutine(Lifespan());
        activate_button.GetComponent<Animator>().Play("equipment_button_exit");
        StartCoroutine(ButtonDeactivate());
        manager.UpdateReqEquipmentKills();
    }

    IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(UltraTime);
        endEffect = true;
        IsUltra = false;
        manager.player_is_ultra = false;
        manager.ResetUltraKills(0);
        yield return new WaitForSeconds(1);
        CamEffectObj.SetActive(false);
    }
    public void DeactivateButton(){
        activate_button.SetActive(false);
        if(IsUltra){ 
            endEffect = true; 
            startEffect = false;
            IsUltra = false;
        }
    }
    IEnumerator ButtonDeactivate(){
        activate_button.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        activate_button.SetActive(false);
    }
}
