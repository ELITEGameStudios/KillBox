using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class EquipmentManager : MonoBehaviour
{
    public int equipmentKills; //{get; private set;}

    public bool usingEquipment;
    [SerializeField] private int req_equipment_kills = 30;
    [SerializeField] private GameObject activate_button;

    // [SerializeField] private GameObject[] use_equipment_button_list, equipment_list, equippedEquipmentDisplay;

    public Slider equipment_slider;
    [SerializeField] private Text equipmentText;
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Animator equipmentAnimator;

    private float timer;
    private float[] activeTimes = new float[]{12, 8, 10, 20};

    public static EquipmentManager instance {get; private set;}
    public EquipmentType equipmentType; // {get; private set;}, I want to see the value for now
    public EquipmentBase[] equipmentScripts; // {get; private set;}, I want to see the value for now
    public int equipment_index {get {return (int)equipmentType;}}


    public enum EquipmentType{
        ULTRAMODE,
        HOTSHOT,
        OVERDRIVE,
        MINIONS
    }

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(this); }

        equipment_slider = GameplayUI.instance.GetEquipmentSlider();
        equipmentText = GameplayUI.instance.GetEquipmentHeader();
        equipmentImage = GameplayUI.instance.GetEquipmentImage();
        equipmentAnimator = GameplayUI.instance.GetEquipmentAnimator();
        SetEquipmentType(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!usingEquipment)
        {
            if (equipmentKills >= req_equipment_kills)
            {
                if(!equipmentAnimator.GetBool("charged")){ equipmentAnimator.SetBool("charged", true); }
                
                if (DetectInputDevice.main.isController) { equipmentText.text = "PRESS X TO USE EQUIPMENT"; }
                else if (DetectInputDevice.main.isKBM) { equipmentText.text = "PRESS " + CustomKeybinds.main.Ultramode.ToString() + " TO USE EQUIPMENT"; }

                // if (!activate_button.activeInHierarchy)  For mobile eventually
                // {
                //     activate_button.SetActive(true);
                //     activate_button.transform.GetChild(1).GetComponent<Button>().interactable = true;
                //     activate_button.GetComponent<Animator>().Play("equipment_button_startup");
                // }
            }
            else
            {
                equipmentText.text = ""; 
            }
        }
        else{
            if(timer <= 0) {
                ResetUltraKills(0);
                equipmentScripts[(int)equipmentType].GamemodeEnd();
                equipmentAnimator.SetBool("active", false);
                /*Deactivate equipment*/
            
            }
            else {
                timer -= Time.deltaTime;
                equipment_slider.value = timer;
            }
        }

    }

    public void SetEquipmentType(EquipmentType type){
        equipmentType = type;
        equipment_slider.maxValue = req_equipment_kills;
        equipment_slider.value = equipmentKills;

        KillBox.Save();
    }
    public void ActivateEquipment()
    {
        EquipmentBase equipment = equipmentScripts[(int)equipmentType];
        equipment.GamemodeStart();

        equipment_slider.maxValue = equipment.time;
        equipment_slider.value = equipment.time;
        timer = equipment.time;

        equipmentAnimator.SetBool("active", true);
        equipmentAnimator.SetBool("charged", false);
        equipmentAnimator.Play("Activate");


        //For mobile eventually
        // activate_button.GetComponent<Animator>().Play("equipment_button_exit");
        // StartCoroutine(ButtonDeactivate());
        UpdateReqEquipmentKills();
    }

    public void UpdateReqEquipmentKills(){
        req_equipment_kills = (int)Mathf.Clamp(req_equipment_kills * 2, 30, 300);
        equipmentKills = 0;

        
        if(KillBox.currentGame.freeplay){ req_equipment_kills = 0; }
    }

    public void ResetUltraKills(int kills){
        equipmentKills = kills;
        equipment_slider.maxValue = req_equipment_kills;
        equipment_slider.value = kills;
    }

    public void AddEquipmentKill()
    {
        if (usingEquipment) return;

        equipmentKills++;
        equipment_slider.value = equipmentKills;
        if (!equipmentAnimator.GetBool("charged")){
            equipmentAnimator.Play("hit");
        }
    }

    IEnumerator ButtonDeactivate(){
        activate_button.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(0.5f);
        activate_button.SetActive(false);
    }

    public void DeactivateButton(){
        activate_button.SetActive(false);
        if(usingEquipment){ 
            // endEffect = true; 
            // startEffect = false;
        }
    }
}
