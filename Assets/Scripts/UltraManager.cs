using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UltraManager : EquipmentBase
{
    public bool IsUltra;

    [SerializeField]
    private float UltraTime, start_effect_clock, end_effect_clock;

    public GameObject CamEffectObj, activate_button;

    [SerializeField]
    private GameManager manager;
    [SerializeField] private Text buttonText;


    void Update()
    {
        //startAnim
        if (startEffect)
        {
            start_effect_clock = 0f;
            startEffect = false;
        }

        if (start_effect_clock < 1)
        {
            start_effect_clock += Time.deltaTime * 2;
            // CamEffectObj.GetComponent<Volume>().weight = start_effect_clock;
        }
        else
        {
            if (start_effect_clock > 1)
            {
                start_effect_clock = 1;
                // CamEffectObj.GetComponent<Volume>().weight = start_effect_clock;
            }
        }

        //endAnim
        if (endEffect)
        {
            end_effect_clock = 1f;
            endEffect = false;
        }

        if (end_effect_clock > 0)
        {
            end_effect_clock -= Time.deltaTime * 2;
            // CamEffectObj.GetComponent<Volume>().weight = end_effect_clock;
        }
        else
        {
            if (end_effect_clock < 0)
            {
                end_effect_clock = 0;
                // CamEffectObj.GetComponent<Volume>().weight = end_effect_clock;
            }
        }
    }

    public override void GamemodeStart()
    {
        // CamEffectObj.SetActive(true);
        // throw new System.NotImplementedException();
    }

    public override void GamemodeEnd()
    {
        
    }
}
