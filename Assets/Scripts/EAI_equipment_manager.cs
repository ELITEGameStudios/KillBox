using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EAI_equipment_manager : MonoBehaviour
{
    public bool has_ally, startEffect, endEffect;

    [SerializeField]
    private float UltraTime, start_effect_clock, end_effect_clock;

    public GameObject Ally, activate_button; 
    public GameObject[] clone;

    [SerializeField]
    private GameManager manager;

    [SerializeField] private Text buttonText;
    
    // Start is called before the first frame update
    void Start()
    {
        has_ally = false;
    }

    void Update(){
        if(DetectInputDevice.main.isController){ buttonText.text = "PRESS X TO USE EQUIPMENT"; }
        else if(DetectInputDevice.main.isKBM){ buttonText.text = "PRESS "+CustomKeybinds.main.Ultramode.ToString()+" TO USE EQUIPMENT"; }
        
    }

    public void GamemodeStart()
    {
        has_ally = true;
        clone = new GameObject[5];

        StartCoroutine(Lifespan());
        activate_button.GetComponent<Animator>().Play("equipment_button_exit");
        StartCoroutine(ButtonDeactivate());
        manager.UpdateReqEquipmentKills();

        if (has_ally)
        {
            manager.ResetUltraKills(0);
        }
    }

    public void DeactivateButton(){
        activate_button.SetActive(false);
        if(has_ally){ 
            endEffect = true; 
            startEffect = false;
            has_ally = false;
        }
        for (int i = 0; i < clone.Length; i++)
        {
            if(clone[i] != null)
            {
                Destroy(clone[i]);
            }
        }
    }

    IEnumerator Lifespan()
    {
        float interval = 0.5f;
        for (int i = 0; i < clone.Length; i++)
        {
            interval = 0.5f;
            while (interval > 0)
            {
                interval -= Time.deltaTime;
                yield return null;
            }

            clone[i] = Instantiate(Ally, transform);
            clone[i].transform.SetParent(null);

        }

        float time = UltraTime;


        while (time > 0)
        {
            time -= Time.deltaTime;
            manager.equipment_slider.maxValue = UltraTime;
            manager.equipment_slider.value  = time;

            yield return null;
        }

        has_ally = false;
        manager.player_has_ally = false;

        for (int i = 0; i < clone.Length; i++)
        {
            if(clone[i] != null)
            {
                Destroy(clone[i]);
            }
        }

        
        manager.ResetUltraKills(0);

    }

    IEnumerator ButtonDeactivate(){
        activate_button.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        activate_button.SetActive(false);
    }
}
