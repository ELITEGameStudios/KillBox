using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BossBarManager : MonoBehaviour
{

    [SerializeField] private GameObject boss_bar;

    [SerializeField] private Text name_txt, health_txt;

    [SerializeField] private Slider slider;

    [SerializeField] private List<BossDisplayObj> queue;
    [SerializeField] private Volume boss_volume;
    [SerializeField] private float volume_weight;

    public static BossBarManager Instance {get; private set;}
    void Awake()
    {
        queue = new List<BossDisplayObj>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    public void AddToQueue(GameObject boss_input, string name){

        BossDisplayObj new_object = new BossDisplayObj(boss_input, name);
        // MainAudioSystem.main.TriggerBossMusic(name);

        queue.Add(new_object);

    }

    void ManageVolume(){
        // print("LLLL");
        if(QualityControl.main.BossShaderVolume.isOn){

            if(queue.Count > 0 && volume_weight < 1 || PortalScript.main.Mode == 3 && volume_weight < 1 && PortalScript.main.gameObject.activeInHierarchy){ volume_weight += 0.25f * Time.deltaTime; }
            else if (queue.Count == 0 && volume_weight > 0){ volume_weight -= 0.25f * Time.deltaTime; } 
            else if (queue.Count > 0){ volume_weight = 1; }
            else{ volume_weight = 0; }

            boss_volume.weight = volume_weight;
        }
        else{
            boss_volume.weight = 0;
        }
    }

    void Update(){

        for(int i = 0; i < queue.Count; i++){
            if(queue[i]._object == null || queue[i].health.CurrentHealth <= 0){
                queue.RemoveAt(i);
            }
        }

        ManageVolume();

        if (queue.Count > 0){

            if(!boss_bar.activeInHierarchy){
                boss_bar.SetActive(true);
            }

            health_txt.text =   queue[0].health.CurrentHealth.ToString() + " | " + queue[0].health.maxHealth.ToString();
            name_txt.text = queue[0]._name;
            slider.maxValue =  queue[0].health.maxHealth;
            slider.value =  queue[0].health.CurrentHealth;
        }
        else{
            if(boss_bar.activeInHierarchy){
                boss_bar.SetActive(false);
            }

            //if(BossAudio.Instance.Check){
            //    BossAudio.Instance.Check = false;
            //}
            // if(MainAudioSystem.main.exterior_track){
            //     MainAudioSystem.main.PlayMainLoop();
            // }
        }
    }
}

public class BossDisplayObj{

    public GameObject _object {get; private set;}
    public string _name {get; private set;}

    public EnemyHealth health {get; private set;}
    public BossDisplayObj(GameObject obj, string name){
        _object = obj;
        _name = name;

        health = obj.GetComponent<EnemyHealth>();
    }
}