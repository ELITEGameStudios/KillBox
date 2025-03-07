using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OverdriveManager : EquipmentBase
{
    public GameObject CamEffectObj;

    void Start()
    {
        CamEffectObj.SetActive(false);
    }

    void Update(){

        // float weight_value, H, S, V;

        // Color.RGBToHSV(floor_color.CurrentColor, out H, out S, out V);

        // if(H > 298/360 || H < 60)
        // {
        //     weight_value = 0.35f;
        // }
        // else
        // {
        //     weight_value = 1;
        // }

        // //startAnim
        // if (startEffect){
        //     start_effect_clock = 0f;
        //     startEffect = false;
        // }

        // if(start_effect_clock < 1){
        //     start_effect_clock += Time.deltaTime * 2;
        //     CamEffectObj.GetComponent<Volume>().weight = start_effect_clock * weight_value;
        // }
        // else{
        //     if(start_effect_clock > 1){
        //         start_effect_clock = 1;
        //         CamEffectObj.GetComponent<Volume>().weight = start_effect_clock * weight_value;
        //     }
        // }

        // //endAnim
        // if(endEffect){
        //     end_effect_clock = 1f;
        //     endEffect = false;
        // }

        // if(end_effect_clock > 0){
        //     end_effect_clock -= Time.deltaTime * 2;
        //     CamEffectObj.GetComponent<Volume>().weight = end_effect_clock * weight_value;
        // }
        // else{
        //     if(end_effect_clock < 0){
        //         end_effect_clock = 0;
        //         CamEffectObj.GetComponent<Volume>().weight = end_effect_clock * weight_value;
        //     }
        // }

        // if(EquipmentManager.instance.equipmentType == EquipmentType.OVERDRIVE && EquipmentManager.instance.usingEquipment){
        //     GunHandler.Instance.cooldown.ResetCooldown();
        // }
    }

    public override void GamemodeStart()
    {
        // CamEffectObj.SetActive(true);
        // startEffect = true;
        // StartCoroutine(Lifespan());
        // activate_button.GetComponent<Animator>().Play("equipment_button_exit");
        // StartCoroutine(ButtonDeactivate());
        // manager.UpdateReqEquipmentKills();
    }
}
