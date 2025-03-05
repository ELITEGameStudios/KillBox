using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OverdriveManager : MonoBehaviour
{
    public bool is_overdrive, startEffect, endEffect;

    [SerializeField]
    private float UltraTime, start_effect_clock, end_effect_clock;

    [SerializeField]
    private FloorColorScript floor_color;

    public GameObject CamEffectObj, activate_button;

    [SerializeField]
    private GameManager manager;

    [SerializeField] private Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        is_overdrive = false;
        CamEffectObj.SetActive(false);
    }

    void Update(){

        if(DetectInputDevice.main.isController){ buttonText.text = "PRESS X TO USE EQUIPMENT"; }
        else if(DetectInputDevice.main.isKBM){ buttonText.text = "PRESS "+CustomKeybinds.main.Ultramode.ToString()+" TO USE EQUIPMENT"; }

        float weight_value, H, S, V;

        Color.RGBToHSV(floor_color.CurrentColor, out H, out S, out V);

        if(H > 298/360 || H < 60)
        {
            weight_value = 0.35f;
        }
        else
        {
            weight_value = 1;
        }

        //startAnim
        if (startEffect){
            start_effect_clock = 0f;
            startEffect = false;
        }

        if(start_effect_clock < 1){
            start_effect_clock += Time.deltaTime * 2;
            CamEffectObj.GetComponent<Volume>().weight = start_effect_clock * weight_value;
        }
        else{
            if(start_effect_clock > 1){
                start_effect_clock = 1;
                CamEffectObj.GetComponent<Volume>().weight = start_effect_clock * weight_value;
            }
        }

        //endAnim
        if(endEffect){
            end_effect_clock = 1f;
            endEffect = false;
        }

        if(end_effect_clock > 0){
            end_effect_clock -= Time.deltaTime * 2;
            CamEffectObj.GetComponent<Volume>().weight = end_effect_clock * weight_value;
        }
        else{
            if(end_effect_clock < 0){
                end_effect_clock = 0;
                CamEffectObj.GetComponent<Volume>().weight = end_effect_clock * weight_value;
            }
        }

        if(is_overdrive){
            manager.equipment_slider.value -= Time.deltaTime;
            GunHandler.Instance.cooldown.ResetCooldown();
        }
    }

    public void GamemodeStart()
    {
        is_overdrive = true;
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
        is_overdrive = false;
        manager.player_is_overdrive = false;
        manager.ResetUltraKills(0);
        yield return new WaitForSeconds(1);
        CamEffectObj.SetActive(false);
    }
    public void DeactivateButton(){
        activate_button.SetActive(false);
        if(is_overdrive){ 
            endEffect = true; 
            startEffect = false;
            is_overdrive = false;
        }
    }

    IEnumerator ButtonDeactivate(){
        activate_button.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        activate_button.SetActive(false);
    }
}
